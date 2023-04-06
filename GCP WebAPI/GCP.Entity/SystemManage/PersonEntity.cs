using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.SystemManage
{

    [JsonObject(MemberSerialization.OptOut), Table(DisableSyncStructure = true, Name = "person")]
    public partial class PersonEntity : BaseEntity
    {
        /// <summary>
        /// 是否是系统管理员
        /// </summary>
        [Description("是否是系统管理员")]
        [JsonIgnore]
        [Column(Name = "issystem", DbType = "bit")]
        public System.Boolean? IsSystem { get; set; }

        [Description("上次登陆时间")]
        [JsonIgnore]
        [Column(Name = "lastlogintime", DbType = "smalldatetime")]
        public System.DateTime? LastLoginTime { get; set; }

        [Description("真实姓名")]
        [Column(Name = "name", StringLength = 20, IsNullable = false, DbType = "nvarchar(20)")]
        public System.String Name { get; set; }

        [Description("控制权限")]
        [Column(Name = "organizationid", DbType = "bigint")]
        public System.Int64? OrganizationID { get; set; }

        [Description("控制权限")]
        [Column(Name = "organizationtype", DbType = "bigint")]
        public System.Int64? OrganizationType { get; set; }

        [Description("MD5加密的密码")]
        [JsonIgnore]
        [Column(Name = "passwordmd5", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? PassWordMD5 { get; set; }

        [Description("密码")]
        [Column(IsIgnore = true)]
        public System.String? PassWord { get; set; }

        [Description("身份证")]
        [Column(Name = "pid", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String PID { get; set; }

        [Description("职位")]
        [Column(Name = "post", DbType = "bigint")]
        [JsonIgnore]
        public System.Int64? Post { get; set; }

        [Description("注册时间，前台不显示")]
        [JsonIgnore]
        [Column(Name = "regtime", DbType = "datetime")]
        public System.DateTime? RegTime { get; set; }

        [Description("备注")]
        [Column(Name = "remarks", StringLength = -1, DbType = "text")]
        public System.String? Remarks { get; set; }

        [Description("性别")]
        [Column(Name = "sex", StringLength = 2, DbType = "nvarchar(2)")]
        public System.String Sex { get; set; }

        [Description("电话")]
        [Column(Name = "tel", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String Tel { get; set; }

        [Description("登陆Token")]
        [JsonIgnore]
        [Column(Name = "token", StringLength = 32, DbType = "varchar(32)")]
        public System.String? Token { get; set; }

        [Description("用户名")]
        [Column(Name = "username", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String UserName { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Description("角色")]
        [Column(IsIgnore = true)]
        public System.String? RoleIds { get; set; }

        [Description("职位")]
        [Column(IsIgnore = true)]
        public System.String? PostName { get; set; }

        [Description("Join方法查询Organization所属Table")]
        [Column(IsIgnore = true)]
        [JsonIgnore]
        public System.String? OrganizationTable { get; set; }

        [Description("控制权限")]
        [Column(IsIgnore = true)]
        public System.String? OrganizationTypeName { get; set; }

        [Description("组织名称")]
        [Column(IsIgnore = true)]
        public System.String? OrganizationName { get; set; }

        [Description("需要显示的职位")]
        [Column(IsIgnore = true)]
        public List<long>? PostList { get; set; }

        [Description("需要上传的职位")]
        [Column(IsIgnore = true)]
        public System.String? PostInfo { get; set; }

        [Description("状态")]
        [Column(IsIgnore = true)]
        public System.String? StatusName { get; set; }

        [Description("前端上传的组织结构")]
        [Column(IsIgnore = true)]
        public System.String? OrgInfo { get; set; }
    }
}
