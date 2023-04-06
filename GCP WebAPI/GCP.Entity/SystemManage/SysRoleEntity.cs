using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace GCP.Entity.SystemManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "SysRole")]
    public partial class SysRoleEntity : BaseEntity
    {
        [JsonProperty, Column(Name = "Remark", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? Remark { get; set; }

        [JsonProperty, Column(Name = "RoleName", StringLength = 50, IsNullable = false, DbType = "nvarchar(50)")]
        public System.String RoleName { get; set; }

        [JsonProperty, Column(Name = "RoleSort", DbType = "int")]
        public System.Int32? RoleSort { get; set; }

        /// 角色对应的菜单，页面和按钮
        /// </summary>
        [JsonProperty, Column(IsIgnore = true)]
        public string MenuIds { get; set; }

        [JsonProperty, Column(Name = "PlatformType", DbType = "int")]
        public System.Int32? PlatformType { get; set; }
    }
}