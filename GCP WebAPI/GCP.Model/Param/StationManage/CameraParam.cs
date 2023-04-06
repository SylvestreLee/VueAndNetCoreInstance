using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.StationManage
{
    /// <summary>
    /// 创 建：sht
    /// 日 期：2021-06-16 14:25
    /// 描 述：实体查询类
    /// </summary>
    public class CameraListParam
    {
        /// <summary>
        /// 检测站下拉树查询条件
        /// </summary>
        public string? StationTreeValue { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string? ChannelCodingID { get; set; }
        /// <summary>
        /// 硬盘录像机ID
        /// </summary>
        public long VideoServerID { get; set; }
        /// <summary>
        /// 开始视频通道ID
        /// </summary>
        public int StartChannelID { get; set; }
        /// <summary>
        /// 结束视频通道ID
        /// </summary>
        public int EndChannelID { get; set; }
    }
}
