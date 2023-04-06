using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Util.Filter
{
    /// <summary>
    /// 标记表示这是输入字段
    /// </summary>
    public class 输入字段Attribute : Attribute
    {
    }

    /// <summary>
    /// 标记这是输出字段
    /// </summary>
    public class 输出字段Attribute : Attribute
    {
    }

    /// <summary>
    /// 标记这是输入输出都可以的字段
    /// </summary>
    public class 输入输出字段Attribute : Attribute
    {
    }
}
