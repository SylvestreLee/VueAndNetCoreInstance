using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "ins_resultaudit")]
    public partial class InsResultAuditEntity : BaseEntity
    {
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "auditcount", DbType = "bigint")]
        public System.Int64? AuditCount { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonIgnore, Column(IsIgnore =true)]
        public System.Int64? ChildID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "inspectionid", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? InspectionID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l2rabegintime", DbType = "datetime")]
        public System.DateTime? L2RABeginTime { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l2raendtime", DbType = "datetime")]
        public System.DateTime? L2RAEndTime { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l2raremarks", StringLength = -1, DbType = "text")]
        public System.String? L2RARemarks { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l2resultaudit", DbType = "int")]
        public System.Int32? L2ResultAudit { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l2resultauditor", DbType = "bigint")]
        public System.Int64? L2ResultAuditor { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l3rabegintime", DbType = "datetime")]
        public System.DateTime? L3RABeginTime { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l3raendtime", DbType = "datetime")]
        public System.DateTime? L3RAEndTime { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l3raremarks", StringLength = -1, DbType = "text")]
        public System.String? L3RARemarks { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l3resultaudit", DbType = "bigint")]
        public System.Int64? L3ResultAudit { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l3resultauditor", DbType = "bigint")]
        public System.Int64? L3ResultAuditor { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonIgnore, Column(IsIgnore =true)]
        public System.Int64? ParentID { get; set; }
    }
}
