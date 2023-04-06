using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.EnumManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "Enum_LogOperateEPB_Type")]
    public partial class EnumLogOperateEPBTypeEntity : BaseEntity
    {
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "ExecuteUrl", StringLength = 100, IsNullable = false, DbType = "nvarchar(100)")]
        public System.String ExecuteUrl { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "Name", StringLength = 10, IsNullable = false, DbType = "nvarchar(10)")]
        public System.String Name { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "Remarks", StringLength = -1, DbType = "text")]
        public System.String? Remarks { get; set; }
    }
}
