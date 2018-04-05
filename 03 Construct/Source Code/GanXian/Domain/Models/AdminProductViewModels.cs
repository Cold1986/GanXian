using GanXian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.Models
{
    public class AdminProductViewModels
    {
        public List<products> productList { get; set; }

        public List<CheckBoxListInfo> checkBoxList { get; set; }
    }
}