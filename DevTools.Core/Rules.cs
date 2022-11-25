using Newtonsoft.Json;
using RulesEngine.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DevTools.Core
{
    public class Rules
    {
        public async Task<List<WorkflowRulesValidateResult>> ValidateRules(string workflowRules, Dictionary<string, object> inputs, JsonNodeOptions? jsonNodeOptions = null, JsonDocumentOptions? jsonDocumentOptions = null)
        {
            if (string.IsNullOrWhiteSpace(workflowRules))
                throw new ArgumentNullException("workflowRules");
            if (inputs is null)
                throw new ArgumentNullException("inputs");
            var workflows = JsonConvert.DeserializeObject<Workflow[]>(workflowRules);
            List<RuleParameter> ruleParameters = new List<RuleParameter>();
            foreach (var input in inputs)
            {
                var ruleParameter = new RuleParameter(input.Key, input.Value);
                ruleParameters.Add(ruleParameter);
            }
            RulesEngine.RulesEngine rulesEngine = new RulesEngine.RulesEngine(workflows);
            List<WorkflowRulesValidateResult> workflowRulesValidateResults = new List<WorkflowRulesValidateResult>();
            foreach (var workflow in workflows!)
            {
                WorkflowRulesValidateResult workflowRulesValidateResult = new WorkflowRulesValidateResult()
                {
                    WorkflowName = workflow.WorkflowName
                };
                List<RuleResultTree> resultList = await rulesEngine.ExecuteAllRulesAsync(workflow.WorkflowName,ruleParameters.ToArray());
                foreach (var result in resultList)
                {
                    workflowRulesValidateResult.RulesValidateResult.Add(
                        new RuleValidateResult()
                        {
                            RuleName = result.Rule.RuleName,
                            Expression = result.Rule.Expression,
                            Result = result.IsSuccess,
                            Message = result.ExceptionMessage
                        }
                    );
                }
                workflowRulesValidateResults.Add(workflowRulesValidateResult);
            }
            return workflowRulesValidateResults;
        }
    }

    public class WorkflowRulesValidateResult
    {
        public string WorkflowName { get; set; } = null!;

        public List<RuleValidateResult> RulesValidateResult { get; set; }

        public WorkflowRulesValidateResult()
        {
            RulesValidateResult = new List<RuleValidateResult>();
        }
    }

    public class RuleValidateResult
    {
        public string RuleName { get; set; } = null!;

        public string Expression { get; set; } = null!;

        public bool Result { get; set; }

        public string? Message { get; set; }
    }
}
