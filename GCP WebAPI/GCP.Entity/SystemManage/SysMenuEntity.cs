using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace GCP.Entity.SystemManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "SysMenu")]
    public partial class SysMenuEntity : BaseEntity
    {
        [JsonProperty, Column(Name = "Authorize", StringLength = 50, DbType = "varchar(50)")]
        public System.String Authorize { get; set; }

        [JsonProperty, Column(Name = "MenuIcon", StringLength = 50, DbType = "varchar(50)")]
        public System.String MenuIcon { get; set; }

        [JsonProperty, Column(Name = "MenuName", StringLength = 50, IsNullable = false, DbType = "nvarchar(50)")]
        public System.String MenuName { get; set; }

        [JsonProperty, Column(Name = "MenuSort", DbType = "int")]
        public System.Int32? MenuSort { get; set; }

        [JsonProperty, Column(Name = "MenuTarget", StringLength = 50, DbType = "varchar(50)")]
        public System.String MenuTarget { get; set; }

        [JsonProperty, Column(Name = "MenuType", DbType = "int")]
        public System.Int32 MenuType { get; set; }

        [JsonProperty, Column(Name = "MenuUrl", StringLength = 100, DbType = "varchar(100)")]
        public System.String MenuUrl { get; set; }

        [JsonProperty, Column(Name = "ParentId", DbType = "bigint")]
        public System.Int64 ParentId { get; set; }

        [JsonProperty, Column(Name = "Remark", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String Remark { get; set; }

        [JsonProperty, Column(Name = "SystemType")]
        public System.Int32? SystemType { get; set; }
        /// <summary>
        /// 菜单所属哪个平台1——监管系统2——登录系统
        /// </summary>
        [JsonProperty, Column(Name = "PlatformType", DbType = "int")]
        public System.Int32? PlatformType { get; set; }
    }
}
