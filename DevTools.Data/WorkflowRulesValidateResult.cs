namespace DevTools.Data
{
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

        public List<string> SuccessEvents { get; set; }

        public List<RuleValidateResult> RulesValidateResult { get; set; }

        public WorkflowRulesValidateResult()
        {
            SuccessEvents = new List<string>();
            RulesValidateResult = new List<RuleValidateResult>();
        }
    }
}
