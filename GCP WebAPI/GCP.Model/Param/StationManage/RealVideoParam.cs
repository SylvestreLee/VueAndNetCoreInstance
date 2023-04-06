using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.StationManage
{
    /// <summary>
    /// 创 建：manage
    /// 日 期：2021-07-27 09:10
    /// 描 述：实体查询类
    /// </summary>
    public class RealVideoListParam
    {
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
