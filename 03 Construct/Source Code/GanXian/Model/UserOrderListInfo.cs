using System;
using System.Collections.Generic;
using System.Text;

namespace GanXian.Model
{
    public class UserOrderListInfo : salesslip
    {
        public List<UserShopcartsInfo> Order2ProductsList { get; set; }

        public string orderName { get; set; }
        public string orderPhone { get; set; }
    }
}
