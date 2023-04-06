using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "line")]
    public partial class LineEntity : BaseEntity
    {
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
        [JsonProperty, Column(Name = "code", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String? Code { get; set; }
        
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
        [JsonProperty, Column(Name = "name", StringLength = 50, IsNullable = false, DbType = "nvarchar(50)")]
        public System.String Name { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "number", DbType = "bigint")]
        public System.Int64 Number { get; set; }
        
      
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "prdailyvalidto", DbType = "datetime")]
        public System.DateTime? PRDailyValidTo { get; set; }
        
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
        [JsonProperty, Column(Name = "rigistorid", DbType = "bigint")]
        public System.Int64? RegistorID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "registorip", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String? RegistorIP { get; set; }
        
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
        [JsonProperty, Column(Name = "stationid", DbType = "bigint")]
        public System.Int64 StationID { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "supportedmethods", StringLength = 50, IsNullable = false, DbType = "nvarchar(50)")]
        public System.String SupportedMethods { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "testexpiredate", DbType = "datetime")]
        public System.DateTime? TestExpireDate { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "tsno", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? TSNo { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "type", DbType = "bigint")]
        public System.Int64 Type { get; set; }
        
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "videoinfo", StringLength = 512, DbType = "varchar(512)")]
        public System.String? VideoInfo { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "videoinfos", StringLength = 4000, DbType = "nvarchar(4000)")]
        public System.String? VideoInfos { get; set; }
    }
}
