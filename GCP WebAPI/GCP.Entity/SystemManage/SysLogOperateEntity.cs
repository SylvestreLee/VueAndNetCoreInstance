using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace GCP.Entity.SystemManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "syslogoperate")]
    public partial class SysLogOperateEntity : BaseEntity
    {
        [JsonProperty, Column(Name = "basecreatorid", DbType = "bigint")]
        public System.Int64? BaseCreatorId { get; set; }

        [JsonProperty, Column(IsIgnore = true)]
        public System.String? PersonName { get; set; }

        [JsonProperty, Column(Name = "businesstype", StringLength = 50, DbType = "varchar(50)")]
        public System.String BusinessType { get; set; }

        [JsonProperty, Column(Name = "executeparam", StringLength = 4000, DbType = "nvarchar(4000)")]
        public System.String ExecuteParam { get; set; }

        [JsonProperty, Column(Name = "executeresult", StringLength = 4000, DbType = "nvarchar(4000)")]
        public System.String ExecuteResult { get; set; }

        [JsonProperty, Column(Name = "executetime", DbType = "int")]
        public System.Int32? ExecuteTime { get; set; }

        [JsonProperty, Column(Name = "executeurl", StringLength = 100, DbType = "nvarchar(100)")]
        public System.String ExecuteUrl { get; set; }

        [JsonProperty, Column(Name = "ipaddress", StringLength = 20, DbType = "varchar(20)")]
        public System.String IpAddress { get; set; }

        [JsonProperty, Column(Name = "iplocation", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String IpLocation { get; set; }

        [JsonProperty, Column(Name = "logstatus", DbType = "int")]
        public System.Int32? LogStatus { get; set; }

        [JsonProperty, Column(Name = "logtype", StringLength = 50, DbType = "varchar(50)")]
        public System.String LogType { get; set; }

        [JsonProperty, Column(Name = "remark", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String Remark { get; set; }
    }
}