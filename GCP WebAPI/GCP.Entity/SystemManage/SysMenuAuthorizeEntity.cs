using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace GCP.Entity.SystemManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "SysMenuAuthorize")]
    public partial class SysMenuAuthorizeEntity : BaseEntity
    {
        [JsonProperty, Column(Name = "AuthorizeId", DbType = "bigint")]
        public System.Int64? AuthorizeId { get; set; }

        [JsonProperty, Column(Name = "AuthorizeType", DbType = "int")]
        public System.Int32? AuthorizeType { get; set; }

        [JsonProperty, Column(Name = "MenuId", DbType = "bigint")]
        public System.Int64? MenuId { get; set; }

        [Column(IsIgnore = true)]
        public string AuthorizeIds { get; set; }
    }
}