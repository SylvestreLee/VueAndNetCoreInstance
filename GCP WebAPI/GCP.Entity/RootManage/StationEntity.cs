using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "station")]
    public partial class StationEntity : BaseEntity
    {
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "tsno", StringLength = 50, IsNullable = false, DbType = "nvarchar(50)")]
        public System.String TSNo { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "address", StringLength = 512, DbType = "nvarchar(512)")]
        public System.String? Address { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "areacode", StringLength = 6, DbType = "nvarchar(6)")]
        public System.String? AreaCode { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "areanum", StringLength = 10, DbType = "nvarchar(10)")]
        public System.String? AreaNum { get; set; }
        
        
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "cmanumber", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String? CMANumber { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "cmavalidto", DbType = "datetime")]
        public System.DateTime? CMAValidTo { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "code", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? Code { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "establishdate", DbType = "datetime")]
        public System.DateTime? EstablishDate { get; set; }
        
       
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "iano", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String? IANo { get; set; }
        
        
       
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "iisport", DbType = "bigint")]
        public System.Int64? IISPort { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "ip", StringLength = 30, DbType = "nvarchar(30)")]
        public System.String? IP { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "laniisport", DbType = "bigint")]
        public System.Int64? LANIISPort { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "lanip", StringLength = 30, DbType = "nvarchar(30)")]
        public System.String? LANIP { get; set; }
        
        
        
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "limitremarks", StringLength = 512, DbType = "nvarchar(512)")]
        public System.String? LimitRemarks { get; set; }
        
       
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "name", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? Name { get; set; }
        
        
        
        
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "remarks", StringLength = 512, DbType = "nvarchar(512)")]
        public System.String? Remarks { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "sdperson", DbType = "bigint")]
        public System.Int64? SDPerson { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "sdreason", StringLength = 500, DbType = "nvarchar(500)")]
        public System.String? SDReason { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "servicedeadline", DbType = "datetime")]
        public System.DateTime? ServiceDeadLine { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "shortname", StringLength = 20, DbType = "nvarchar(20)")]
        public System.String? ShortName { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "stationtype", DbType = "bigint")]
        public System.Int64? StationType { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "statusreason", StringLength = 512, DbType = "nvarchar(512)")]
        public System.String? StatusReason { get; set; }
        
    }
}
