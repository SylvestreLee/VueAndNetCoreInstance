using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCP.Model.Param.RootManage
{
    /// <summary>
    /// 创 建：ycz
    /// 日 期：2021-08-23 13:41
    /// 描 述：实体查询类
    /// </summary>
    public class EndInspectParam
    {
        public long StationID { get; set; }
        public string InspectionID { get; set; }
        public DateTime JYRQ { get; set; }
        public long InspectionItem { get; set; }
        public long Result { get; set; }
        public string ReportNumber { get; set; }
        public int RealInspectionMethod { get; set; }
    }
}
