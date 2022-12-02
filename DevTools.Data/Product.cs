namespace DevTools.Data
{
    /// <summary>
    /// 产品
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductNo { get; private set; }

        /// <summary>
        /// 店铺
        /// </summary>
        public string Store { get; private set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; private set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; private set; }

        /// <summary>
        /// 券后价
        /// </summary>
        public decimal CouponPrice { get; private set; }

        public Product(string store, decimal price, decimal quantity)
        {
            Random random = new();
            ProductNo = NoGenerator.Generate(random.NextInt64(1, 1_000_000), "Product");
            Store = store;
            Price = price;
            Quantity = quantity;
            TotalPrice = price * quantity;
            CouponPrice = TotalPrice;
        }
    }
}
