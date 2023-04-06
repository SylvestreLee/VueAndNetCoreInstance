using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace GCP.Entity.SystemManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "syslogapi")]
    public partial class SysLogApiEntity : BaseEntity
    {
        [JsonProperty, Column(Name = "basecreatorid", DbType = "bigint")]
        public System.Int64? BaseCreatorId { get; set; }

        [JsonProperty, Column(Name = "executeparam", StringLength = 4000, DbType = "nvarchar(4000)")]
        public System.String ExecuteParam { get; set; }

        [JsonProperty, Column(Name = "executeresult", StringLength = 4000, DbType = "nvarchar(4000)")]
        public System.String ExecuteResult { get; set; }

        [JsonProperty, Column(Name = "executetime", DbType = "int")]
        public System.Int32 ExecuteTime { get; set; }

        [JsonProperty, Column(Name = "executeurl", StringLength = 100, IsNullable = false, DbType = "varchar(100)")]
        public System.String ExecuteUrl { get; set; }

        [JsonProperty, Column(Name = "logstatus", DbType = "int")]
        public System.Int32 LogStatus { get; set; }

        [JsonProperty, Column(Name = "remark", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String Remark { get; set; }
    }
}