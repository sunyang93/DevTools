namespace DevTools.Data
{
    /// <summary>
    /// 章节
    /// </summary>
    public class Chapter
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// 副标题
        /// </summary>
        public string? Subtitle { get; set; }

        public decimal Money { get; set; }
    }
}