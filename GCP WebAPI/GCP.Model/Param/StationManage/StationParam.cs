using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.StationManage
{
    /// <summary>
    /// 创 建：manage
    /// 日 期：2021-05-28 18:36
    /// 描 述：实体查询类
    /// </summary>
    public class StationListParam
    {
        /// <summary>
        /// 检测站编号
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 所属辖区
        /// </summary>
        public string? AreaCode { get; set; }

        /// <summary>
        /// 权限辖区
        /// </summary>
        public string? ObjectAreaCode { get; set; }
    }
}
