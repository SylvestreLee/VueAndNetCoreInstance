using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace GCP.Entity.SystemManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "sysuserbelong")]
    public partial class SysUserBelongEntity : BaseEntity
    {
        [JsonProperty, Column(Name = "belongid", DbType = "bigint")]
        public System.Int64 BelongId { get; set; }

        [JsonProperty, Column(Name = "belongtype", DbType = "int")]
        public System.Int32 BelongType { get; set; }

        [JsonProperty, Column(Name = "userid", DbType = "bigint")]
        public System.Int64 UserId { get; set; }

        [JsonProperty, Column(IsIgnore = true)]
        public List<System.Int64>? UserIds { get; set; }
    }
}
