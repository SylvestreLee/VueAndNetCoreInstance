using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace GCP.Entity.SystemManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "syslogoperate")]
    public partial class LogOperateEPBEntity : BaseEntity
    {
        [JsonProperty, Column(Name = "basecreatorid", DbType = "bigint")]
        public System.Int64? BaseCreatorId { get; set; }

        [JsonProperty, Column(IsIgnore = true)]
        public System.String? PersonName { get; set; }

        [JsonIgnore, Column(Name = "executeparam", StringLength = 4000, DbType = "nvarchar(4000)")]
        public System.String ExecuteParam { get; set; }

        [JsonIgnore, Column(Name = "executeurl", StringLength = 100, DbType = "nvarchar(100)")]
        public System.String ExecuteUrl { get; set; }

        /// <summary>
        /// 方法ID
        /// </summary>
        [JsonProperty, Column(IsIgnore = true)]
        public System.Int64 MethodID { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        [JsonProperty, Column(IsIgnore = true)]
        public System.String MethodName { get; set; }

        /// <summary>
        /// 检测站
        /// </summary>
        [JsonProperty, Column(IsIgnore = true)]
        public System.Int64? StationID { get; set; }

        /// <summary>
        /// 检测站名称
        /// </summary>
        [JsonProperty, Column(IsIgnore = true)]
        public System.String? StationName { get; set; }

        /// <summary>
        /// 检测线
        /// </summary>
        [JsonProperty, Column(IsIgnore = true)]
        public System.Int64? LineID { get; set; }

        /// <summary>
        /// 检测线名称
        /// </summary>
        [JsonProperty, Column(IsIgnore = true)]
        public System.String? LineName { get; set; }

        /// <summary>
        /// 机动车ID
        /// </summary>
        [JsonProperty, Column(IsIgnore = true)]
        public System.Int64? VehicleID { get; set; }

        /// <summary>
        /// 机动车号牌颜色
        /// </summary>
        [JsonIgnore, Column(IsIgnore = true)]
        public System.String? VehiclePlateColor { get; set; }

        /// <summary>
        /// 机动车车牌号
        /// </summary>
        [JsonIgnore, Column(IsIgnore = true)]
        public System.String? VehiclePlateID { get; set; }

        /// <summary>
        /// 机动车号牌颜色名称
        /// </summary>
        [JsonIgnore, Column(IsIgnore = true)]
        public System.String? VehiclePlateColorName { get; set; }

        /// <summary>
        /// 机动车车牌号全名
        /// </summary>
        [JsonProperty, Column(IsIgnore = true)]
        public System.String? VehicleName { get; set; }

        ///// <summary>
        ///// 操作内容
        ///// </summary>
        //[JsonProperty, Column(IsIgnore = true)]
        //public System.String? Value { get; set; }
    }
}
