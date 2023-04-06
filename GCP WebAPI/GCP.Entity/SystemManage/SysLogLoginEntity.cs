using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace GCP.Entity.SystemManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "sysloglogin")]
    public partial class SysLogLoginEntity : BaseEntity
    {
        [JsonProperty, Column(Name = "basecreatorid", DbType = "bigint")]
        public System.Int64? BaseCreatorId { get; set; }

        [JsonProperty, Column(Name = "browser", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String Browser { get; set; }

        [JsonProperty, Column(Name = "extraremark", StringLength = 500, DbType = "nvarchar(500)")]
        public System.String ExtraRemark { get; set; }

        [JsonProperty, Column(Name = "ipaddress", StringLength = 20, DbType = "varchar(20)")]
        public System.String IpAddress { get; set; }

        [JsonProperty, Column(Name = "iplocation", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String IpLocation { get; set; }

        [JsonProperty, Column(Name = "logstatus", DbType = "int")]
        public System.Int32? LogStatus { get; set; }

        [JsonProperty, Column(Name = "os", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String OS { get; set; }

        [JsonProperty, Column(Name = "remark", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String Remark { get; set; }
    }
}