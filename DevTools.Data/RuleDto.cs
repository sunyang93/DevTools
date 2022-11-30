using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DevTools.Data
{
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
}
