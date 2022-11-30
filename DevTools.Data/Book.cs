namespace DevTools.Data
{
    /// <summary>
    /// 书籍
    /// </summary>
    public class Book
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// 副标题
        /// </summary>
        public string? Subtitle { get; set; }

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
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 总字数
        /// </summary>
        public int TotalWordage { get; set; }

        /// <summary>
        /// 定价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateOnly PublishDate { get; set; }

        /// <summary>
        /// 章节
        /// </summary>
        public List<Chapter> Chapters { get; set; }

        public Book()
        {
            Chapters = new List<Chapter>();
        }
    }
}