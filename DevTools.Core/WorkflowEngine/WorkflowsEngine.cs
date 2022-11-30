using DevTools.Data;
using Newtonsoft.Json;
using RulesEngine.Extensions;
using RulesEngine.Models;

namespace DevTools.Core
{
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
}
