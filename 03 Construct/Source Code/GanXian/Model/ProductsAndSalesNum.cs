using System;
using System.Collections.Generic;
using System.Text;

namespace GanXian.Model
{
    public class ProductsAndSalesNum : products
    {
        /// <summary>
        /// 销售量
        /// </summary>
        public int soldNum { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public string typeName { get; set; }
    }
}
