using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "inspectioncache")]
    public partial class InspectionCacheEntity : BaseEntity
    {
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
        [JsonIgnore, Column(Name = "fueltypeid", DbType = "smallint")]
        public System.Int16? FuelTypeID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonIgnore, Column(Name = "inspectioncount", DbType = "bigint")]
        public System.Int64? InspectionCount { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "inspectionid", StringLength = 50, IsNullable = false, DbType = "nvarchar(50)")]
        public System.String InspectionID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonIgnore, Column(Name = "inspectionmethod", DbType = "bigint")]
        public System.Int64? InspectionMethod { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonIgnore, Column(Name = "logintime", DbType = "datetime")]
        public System.DateTime? LoginTime { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonIgnore, Column(IsIgnore =true)]
        public System.Int64 LoginVerifyStatus { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonIgnore, Column(IsIgnore = true)]
        public System.Int64? ParentID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonIgnore, Column(Name = "stationid", DbType = "bigint")]
        public System.Int64 StationID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "updateinsraid", DbType = "bigint")]
        public System.Int64? UpdateInsRAID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonIgnore, Column(Name = "vehicleid", DbType = "bigint")]
        public System.Int64? VehicleID { get; set; }
    }
}
