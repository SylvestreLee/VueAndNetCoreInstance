using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.SystemManage
{
    /// <summary>
    /// 创 建：sht
    /// 日 期：2021-08-02 10:58
    /// 描 述：实体查询类
    /// </summary>
    public class LogOperateEPBListParam : DateTimeParam
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public Int64? TypeID { get; set; }
    }
}
