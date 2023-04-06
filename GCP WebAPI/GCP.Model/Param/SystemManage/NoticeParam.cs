using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.SystemManage
{
    /// <summary>
    /// 创 建：sht
    /// 日 期：2021-06-09 09:57
    /// 描 述：实体查询类
    /// </summary>
    public class NoticeListParam : DateTimeParam
    {
        /// <summary>
        /// 通知公告名称
        /// </summary>
        public string? Name { get; set; }
    }
}
