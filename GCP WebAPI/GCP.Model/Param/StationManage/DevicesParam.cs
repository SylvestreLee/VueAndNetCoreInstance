using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.StationManage
{
    /// <summary>
    /// 创 建：sht
    /// 日 期：2021-06-16 14:24
    /// 描 述：实体查询类
    /// </summary>
    public class DevicesListParam
    {
        /// <summary>
        /// 检测站下拉树查询条件
        /// </summary>
        public string? StationTreeValue { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public long? DeviceType { get; set; }

        /// <summary>
        /// 所属检测线
        /// </summary>
        public long? LineID { get; set; }
    }
}
