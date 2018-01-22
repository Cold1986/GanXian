using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GanXian.Model
{
    /// <summary>
    /// 订单日志类型
    /// </summary>
    public enum EnumOrderLogType
    {
        [Description("正常")]
        normal = 0,
        [Description("付款失败")]
        fail = 1,
        [Description("异常")]
        error = 2

    }
}
