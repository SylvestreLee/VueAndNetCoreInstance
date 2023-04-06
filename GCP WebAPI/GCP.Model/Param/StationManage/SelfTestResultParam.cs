using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.StationManage
{
    /// <summary>
    /// 创 建：sht
    /// 日 期：2021-06-21 15:10
    /// 描 述：实体查询类
    /// </summary>
    public class SelfTestResultListParam : DateTimeParam
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
        /// 自检类型
        /// </summary>
        public long? SelfTestType { get; set; }
    }
}
