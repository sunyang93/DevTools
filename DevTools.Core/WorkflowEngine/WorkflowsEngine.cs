using DevTools.Data;
using Humanizer;
using Newtonsoft.Json;
using RulesEngine.Actions;
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
            Workflow[]? workflows = JsonConvert.DeserializeObject<Workflow[]>(JsonConvert.SerializeObject(workflowDtos));
            foreach(Workflow workflow in workflows!)
            {
                foreach (Rule? rule in workflow.Rules)
                {
                    rule.Actions = new RuleActions()
                    {
                        OnSuccess = new ActionInfo()
                        {
                            Name = "OnSuccessAction",
                            Context = new Dictionary<string, object>()
                        },
                        OnFailure=new ActionInfo()
                        {
                            Name = "OnFailureAction",
                            Context = new Dictionary<string, object>()
                        }
                    };
                }
            }
            ReSettings reSettings = new ReSettings
            {
                CustomActions = new Dictionary<string, Func<ActionBase>>
                {
                    {"OnSuccessAction", () => new OnSuccessAction() },
                    {"OnFailureAction", () => new OnFailureAction() },
                }
            };
            RulesEngine.RulesEngine rulesEngine = new(workflows, reSettings);
            List<WorkflowRulesValidateResult> workflowRulesValidateResults = new();
            foreach (Workflow workflow in workflows!)
            {
                WorkflowRulesValidateResult workflowRulesValidateResult = new()
                {
                    WorkflowName = workflow.WorkflowName
                };
                List<RuleParameter> ruleParameters = new();
                foreach (Input input in workflowDtos.FirstOrDefault(d => d.WorkflowName == workflow.WorkflowName)!.Inputs)
                {
                    RuleParameter ruleParameter = new RuleParameter(input.Name, input.Value);
                    ruleParameters.Add(ruleParameter);
                }
                List<RuleResultTree> resultList = await rulesEngine.ExecuteAllRulesAsync(workflow.WorkflowName, ruleParameters.ToArray());
                foreach (RuleResultTree result in resultList)
                {
                    if (result.IsSuccess)
                        workflowRulesValidateResult.SuccessEvents.Add(result.Rule.SuccessEvent);
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
