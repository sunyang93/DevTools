using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DevTools.Data
{
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
}
