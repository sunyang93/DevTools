using DevTools.Data;

namespace DevTools.Core
{
    public interface IWorkflowsEngine
    {
        Task<List<WorkflowRulesValidateResult>> Validate(IEnumerable<WorkflowDto> workflowDtos);
    }
}
