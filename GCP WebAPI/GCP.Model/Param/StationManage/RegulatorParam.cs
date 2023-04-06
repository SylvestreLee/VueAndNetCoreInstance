using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.StationManage
{
    /// <summary>
    /// 创 建：manage
    /// 日 期：2021-05-31 16:39
    /// 描 述：实体查询类
    /// </summary>
    public class RegulatorListParam
    {
        /// <summary>
        /// ID
        /// </summary>
        public long? ID { get; set; }

        /// <summary>
        /// 对象分组名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 辖区编号
        /// </summary>
        public string? AreaCode { get; set; }
    }
}
