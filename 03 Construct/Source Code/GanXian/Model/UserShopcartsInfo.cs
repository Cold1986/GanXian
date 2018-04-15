using System;
using System.Collections.Generic;
using System.Text;

namespace GanXian.Model
{
    public class UserShopcartsInfo : products
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int num { get; set; }

        /// <summary>
        /// 用户微信id
        /// </summary>
        public string userOpenId { get; set; }

        /// <summary>
        /// 商品小计
        /// </summary>
        public decimal? productTotalPrice { get; set; }

        /// <summary>
        /// 创建时价格
        /// </summary>
        public decimal createdTimePrice { get; set; }
    }
}
