using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.StationManage
{
    /// <summary>
    /// 创 建：sht
    /// 日 期：2021-06-22 09:52
    /// 描 述：实体查询类
    /// </summary>
    public class DevicesWJListParam
    {
        /// <summary>
        /// 检测站下拉树查询条件
        /// </summary>
        public string? StationTreeValue { get; set; }
    }
}
