namespace DevTools.Data
{
    /// <summary>
    /// 博客
    /// </summary>
    public class Book
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// 作者
        /// </summary>
        public Author Author { get; set; } = null!;

        /// <summary>
        /// 链接
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; } = null!;

        /// <summary>
        /// 评分
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// 定价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateOnly PublishDate { get; set; }
    }
}