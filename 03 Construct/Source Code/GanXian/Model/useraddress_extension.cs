using System;
using System.Collections.Generic;
using System.Text;

namespace GanXian.Model
{
    public class useraddress_extension : useraddress
    {
        /// <summary>
        /// 省名字
        /// </summary>
        public string provinceName { get; set; }

        /// <summary>
        /// 市名字
        /// </summary>
        public string cityName { get; set; }

        /// <summary>
        /// 县名字
        /// </summary>
        public string countyName { get; set; }
    }
}
