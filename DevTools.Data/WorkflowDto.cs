using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DevTools.Data
{
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
}
