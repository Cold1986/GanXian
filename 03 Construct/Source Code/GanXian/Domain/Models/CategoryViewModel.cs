using GanXian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.Models
{
    public class CategoryViewModel
    {
        public List<ProductsAndSalesNum> productsAndSalesNum { get; set; }

        public List<tablist> tabList { get; set; }
    }
}