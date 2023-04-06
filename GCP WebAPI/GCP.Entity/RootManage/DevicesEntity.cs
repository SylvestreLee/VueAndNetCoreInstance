using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "devices")]
    public partial class DevicesEntity : BaseEntity
    {
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "calibrateno", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? CalibrateNo { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "calibratevalidto", DbType = "datetime")]
        public System.DateTime? CalibrateValidTo { get; set; }
        
       
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "dailyselfcheckvalidto", DbType = "datetime")]
        public System.DateTime? DailySelfCheckValidTo { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "factorydate", DbType = "datetime")]
        public System.DateTime? FactoryDate { get; set; }
        
        
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "lastcalibrateno", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? LastCalibrateNo { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "lastcalibratetime", DbType = "datetime")]
        public System.DateTime? LastCalibrateTime { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "lastdailyselfchecktime", DbType = "datetime")]
        public System.DateTime? LastDailySelfCheckTime { get; set; }
        
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
        [JsonProperty, Column(Name = "model", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String? Model { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "name", StringLength = 20, IsNullable = false, DbType = "nvarchar(20)")]
        public System.String Name { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "obdorder", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? OBDOrder { get; set; }
        
       
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "proposer", DbType = "bigint")]
        public System.Int64? Proposer { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "provider", DbType = "bigint")]
        public System.Int64 Provider { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "regtime", DbType = "datetime")]
        public System.DateTime? RegTime { get; set; }
        
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
        [JsonProperty, Column(Name = "serialnumber", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? SerialNumber { get; set; }
        
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
        [JsonProperty, Column(Name = "type", DbType = "bigint")]
        public System.Int64 Type { get; set; }
        
       
    }
}
