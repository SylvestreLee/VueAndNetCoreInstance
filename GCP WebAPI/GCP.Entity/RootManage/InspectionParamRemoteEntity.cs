using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "inspectionparam_remote")]
    public partial class InspectionParamRemoteEntity : BaseEntity
    {
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "applytostations", StringLength = 500, DbType = "nvarchar(500)")]
        public System.String? ApplyToStations { get; set; }
        
        
        
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
        [JsonProperty, Column(Name = "person", DbType = "bigint")]
        public System.Int64? Person { get; set; }
        
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
        [JsonProperty, Column(Name = "validfrom", DbType = "smalldatetime")]
        public System.DateTime? ValidFrom { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "validto", DbType = "smalldatetime")]
        public System.DateTime? ValidTo { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "value", StringLength = 500, DbType = "nvarchar(500)")]
        public System.String? Value { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "valuedate", DbType = "smalldatetime")]
        public System.DateTime ValueDate { get; set; }
    }
}
