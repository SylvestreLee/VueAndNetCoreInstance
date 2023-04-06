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
        /// �Ƿ���ϵͳ����Ա
        /// </summary>
        [Description("�Ƿ���ϵͳ����Ա")]
        [JsonIgnore]
        [Column(Name = "issystem", DbType = "bit")]
        public System.Boolean? IsSystem { get; set; }

        [Description("�ϴε�½ʱ��")]
        [JsonIgnore]
        [Column(Name = "lastlogintime", DbType = "smalldatetime")]
        public System.DateTime? LastLoginTime { get; set; }

        [Description("��ʵ����")]
        [Column(Name = "name", StringLength = 20, IsNullable = false, DbType = "nvarchar(20)")]
        public System.String Name { get; set; }

        [Description("����Ȩ��")]
        [Column(Name = "organizationid", DbType = "bigint")]
        public System.Int64? OrganizationID { get; set; }

        [Description("����Ȩ��")]
        [Column(Name = "organizationtype", DbType = "bigint")]
        public System.Int64? OrganizationType { get; set; }

        [Description("MD5���ܵ�����")]
        [JsonIgnore]
        [Column(Name = "passwordmd5", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? PassWordMD5 { get; set; }

        [Description("����")]
        [Column(IsIgnore = true)]
        public System.String? PassWord { get; set; }

        [Description("���֤")]
        [Column(Name = "pid", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String PID { get; set; }

        [Description("ְλ")]
        [Column(Name = "post", DbType = "bigint")]
        [JsonIgnore]
        public System.Int64? Post { get; set; }

        [Description("ע��ʱ�䣬ǰ̨����ʾ")]
        [JsonIgnore]
        [Column(Name = "regtime", DbType = "datetime")]
        public System.DateTime? RegTime { get; set; }

        [Description("��ע")]
        [Column(Name = "remarks", StringLength = -1, DbType = "text")]
        public System.String? Remarks { get; set; }

        [Description("�Ա�")]
        [Column(Name = "sex", StringLength = 2, DbType = "nvarchar(2)")]
        public System.String Sex { get; set; }

        [Description("�绰")]
        [Column(Name = "tel", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String Tel { get; set; }

        [Description("��½Token")]
        [JsonIgnore]
        [Column(Name = "token", StringLength = 32, DbType = "varchar(32)")]
        public System.String? Token { get; set; }

        [Description("�û���")]
        [Column(Name = "username", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String UserName { get; set; }

        /// <summary>
        /// ��ɫId
        /// </summary>
        [Description("��ɫ")]
        [Column(IsIgnore = true)]
        public System.String? RoleIds { get; set; }

        [Description("ְλ")]
        [Column(IsIgnore = true)]
        public System.String? PostName { get; set; }

        [Description("Join������ѯOrganization����Table")]
        [Column(IsIgnore = true)]
        [JsonIgnore]
        public System.String? OrganizationTable { get; set; }

        [Description("����Ȩ��")]
        [Column(IsIgnore = true)]
        public System.String? OrganizationTypeName { get; set; }

        [Description("��֯����")]
        [Column(IsIgnore = true)]
        public System.String? OrganizationName { get; set; }

        [Description("��Ҫ��ʾ��ְλ")]
        [Column(IsIgnore = true)]
        public List<long>? PostList { get; set; }

        [Description("��Ҫ�ϴ���ְλ")]
        [Column(IsIgnore = true)]
        public System.String? PostInfo { get; set; }

        [Description("״̬")]
        [Column(IsIgnore = true)]
        public System.String? StatusName { get; set; }

        [Description("ǰ���ϴ�����֯�ṹ")]
        [Column(IsIgnore = true)]
        public System.String? OrgInfo { get; set; }
    }
}
