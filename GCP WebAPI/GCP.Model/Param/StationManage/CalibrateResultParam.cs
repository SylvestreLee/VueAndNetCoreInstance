using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.StationManage
{
    /// <summary>
    /// 创 建：sht
    /// 日 期：2021-06-21 16:56
    /// 描 述：实体查询类
    /// </summary>
    public class CalibrateResultListParam : DateTimeParam
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        public long? DeviceType { get; set; }

        /// <summary>
        /// 检测站下拉树查询条件
        /// </summary>
        public string? StationTreeValue { get; set; }

        /// <summary>
        /// 标定类型
        /// </summary>
        public long? CalibrateType { get; set; }
    }
}
