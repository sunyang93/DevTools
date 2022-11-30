using Newtonsoft.Json;
using RulesEngine.Extensions;
using RulesEngine.Models;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DevTools.Core
{
    public interface IWorkflowsEngine
    {
        Task<List<WorkflowRulesValidateResult>> Validate(IEnumerable<WorkflowDto> workflowDtos);
    }
    public class WorkflowsEngine: IWorkflowsEngine
    {
        public async Task<List<WorkflowRulesValidateResult>> Validate(IEnumerable<WorkflowDto> workflowDtos)
        {
            if (workflowDtos is null)
                throw new ArgumentNullException(nameof(workflowDtos));
            var workflows = JsonConvert.DeserializeObject<Workflow[]>(JsonConvert.SerializeObject(workflowDtos));          
            RulesEngine.RulesEngine rulesEngine = new(workflows);
            List<WorkflowRulesValidateResult> workflowRulesValidateResults = new();
            foreach (var workflow in workflows!)
            {
                WorkflowRulesValidateResult workflowRulesValidateResult = new()
                {
                    WorkflowName = workflow.WorkflowName
                };
                List<RuleParameter> ruleParameters = new();
                foreach (var input in workflowDtos.FirstOrDefault(d => d.WorkflowName == workflow.WorkflowName)!.Inputs)
                {
                    var ruleParameter = new RuleParameter(input.Name, input.Value);
                    ruleParameters.Add(ruleParameter);
                }
                List<RuleResultTree> resultList = await rulesEngine.ExecuteAllRulesAsync(workflow.WorkflowName, ruleParameters.ToArray());
                foreach (var result in resultList)
                {
                    workflowRulesValidateResult.RulesValidateResult.Add(
                        new RuleValidateResult()
                        {
                            RuleName = result.Rule.RuleName,
                            Expression = result.Rule.Expression,
                            IsSuccess = result.IsSuccess,
                            ExceptionMessage = result.ExceptionMessage,
                            SuccessEvent = result.Rule.SuccessEvent
                        }
                    );
                }
                resultList.OnSuccess((eventName) =>
                {
                    workflowRulesValidateResult.IsSuccess = true;
                });
                resultList.OnFail(() =>
                {
                    workflowRulesValidateResult.IsSuccess = false;
                });
                workflowRulesValidateResults.Add(workflowRulesValidateResult);
            }
            return workflowRulesValidateResults;
        }
    }

    public class WorkflowDto
    {
        /// <summary>
        /// 工作流名称
        /// </summary>
        [Required]
        public string WorkflowName { get; set; } = null!;

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 输入参数集
        /// </summary>
        public List<Input> Inputs { get; set; }

        /// <summary>
        /// 规则集
        /// </summary>
        [Required]
        public List<RuleDto> Rules { get; set; }

        public WorkflowDto()
        {
            Inputs = new List<Input>();
            Rules = new List<RuleDto>();
        }
    }

    /// <summary>
    /// 输入参数
    /// </summary>
    public class Input
    {
        /// <summary>
        /// 参数名
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [Required]
        public object Value { get; set; } = null!;
    }

    /// <summary>
    /// 规则
    /// </summary>
    public class RuleDto
    {
        /// <summary>
        /// 规则名
        /// </summary>
        [Required]
        public string RuleName { get; set; } = null!;

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 表达式
        /// </summary>
        [Required]
        public string Expression { get; set; } = null!;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        public string SuccessEvent { get; set; } = string.Empty;

        /// <summary>
        /// 自定义属性
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }

        public RuleDto()
        {
            Properties = new Dictionary<string, object>();
        }
    }

    public class WorkflowRulesValidateResult
    {
        /// <summary>
        /// 工作流名称
        /// </summary>
        public string WorkflowName { get; set; } = null!;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        public List<RuleValidateResult> RulesValidateResult { get; set; }

        public WorkflowRulesValidateResult()
        {
            RulesValidateResult = new List<RuleValidateResult>();
        }
    }

    public class RuleValidateResult
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; } = null!;

        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; } = null!;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMessage { get; set; } = null!;

        public string SuccessEvent { get; set; } = null!;
    }
}
