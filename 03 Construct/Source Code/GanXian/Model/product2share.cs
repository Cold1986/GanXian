using System;
using System.Collections.Generic;
using System.Text;

namespace GanXian.Model
{
    public class product2share 
    {
        public products prods { get; set; }
        public string appId { get; set; }
        public string appSecret { get; set; }
        public string timestamp { get; set; }
        public string nonceStr { get; set; }
        public string url { get; set; }
        public string signature { get; set; }
    }
}
