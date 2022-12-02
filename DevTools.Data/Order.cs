using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTools.Data
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; private set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; private set; }

        /// <summary>
        /// 券后价
        /// </summary>
        public decimal CouponPrice { get; private set; }

        /// <summary>
        /// 产品
        /// </summary>
        public List<Product> Products { get; private set; }

        public Order(decimal totalPrice)
        {
            Random random = new();
            OrderNo = NoGenerator.Generate(random.NextInt64(1, 1_000_000), "Order");
            TotalPrice = totalPrice;
            CouponPrice = TotalPrice;
            Products = new List<Product>();
        }

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="products"></param>
        public void AddProducts(List<Product> products)
        {
            Products.AddRange(products);
        }
    }
}
