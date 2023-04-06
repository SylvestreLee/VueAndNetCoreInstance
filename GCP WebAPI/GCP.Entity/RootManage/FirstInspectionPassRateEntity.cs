using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "firstinspectionpassrate")]
    public partial class FirstInspectionPassRateEntity : BaseEntity
    {
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "c", DbType = "datetime")]
        public System.DateTime C { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "childid", DbType = "bigint")]
        public System.Int64? ChildID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "d", DbType = "int")]
        public System.Int32? D { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "e", DbType = "int")]
        public System.Int32? E { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "firstinscount", DbType = "int")]
        public System.Int32? FirstInsCount { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "firstpasscount", DbType = "int")]
        public System.Int32? FirstPassCount { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "firstpassrate", DbType = "float")]
        public System.Double? FirstPassRate { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "inspectioncount", DbType = "int")]
        public System.Int32? InspectionCount { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "inspectionparamvaluedate", DbType = "datetime")]
        public System.DateTime? InspectionParamValueDate { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "l", DbType = "int")]
        public System.Int32? L { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "lineid", DbType = "bigint")]
        public System.Int64 LineID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "lockvalidto", DbType = "datetime")]
        public System.DateTime? LockValidTo { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "parentid", DbType = "int")]
        public System.Int32? ParentID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "prlimit_enabled", DbType = "bit")]
        public System.Boolean PRLimit_Enabled { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "prlimit_mintotaldaily", DbType = "int")]
        public System.Int32? PRLimit_MinTotalDaily { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "remarks", StringLength = -1, DbType = "text")]
        public System.String? Remarks { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "stationid", DbType = "bigint")]
        public System.Int64 StationID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "vehiclecount", DbType = "int")]
        public System.Int32? VehicleCount { get; set; }
    }
}
