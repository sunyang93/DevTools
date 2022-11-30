namespace DevTools.Data
{
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
