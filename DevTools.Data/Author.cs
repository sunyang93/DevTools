namespace DevTools.Data
{
    /// <summary>
    /// 作者
    /// </summary>
    public class Author
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
    }
}