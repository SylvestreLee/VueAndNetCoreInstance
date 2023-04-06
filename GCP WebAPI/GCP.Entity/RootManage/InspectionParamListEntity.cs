using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "inspectionparamlist")]
    public partial class InspectionParamListEntity : BaseEntity
    {
        
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "defaultvalue", StringLength = 500, DbType = "nvarchar(500)")]
        public System.String? DefaultValue { get; set; }
        
        
        
        
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "name", StringLength = 500, IsNullable = false, DbType = "nvarchar(500)")]
        public System.String Name { get; set; }
        
        
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "paramtype", DbType = "bigint")]
        public System.Int64 ParamType { get; set; }
        
        
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "remarks", StringLength = 500, DbType = "nvarchar(500)")]
        public System.String? Remarks { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "valuetype", DbType = "smallint")]
        public System.Int16? ValueType { get; set; }
    }
}
