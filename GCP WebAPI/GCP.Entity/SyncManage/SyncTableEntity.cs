using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.SyncManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "sync_table")]
    public partial class SyncTableEntity : BaseEntity
    {
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "childid", DbType = "int8")]
        public System.Int64? childid { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "childidcolname", StringLength = 50, DbType = "varchar(50)")]
        public System.String? childidcolname { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "descriptioncolumn", StringLength = 50, DbType = "varchar(50)")]
        public System.String? descriptioncolumn { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "forceindex", DbType = "bool")]
        public System.Boolean? forceindex { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "ifsyncdown", DbType = "bool")]
        public System.Boolean? ifsyncdown { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "ifsyncup", DbType = "bool")]
        public System.Boolean? ifsyncup { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "localidcolname", StringLength = 50, DbType = "varchar(50)")]
        public System.String? localidcolname { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "parentid", DbType = "int8")]
        public System.Int64? parentid { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "parentidcolname", StringLength = 50, DbType = "varchar(50)")]
        public System.String? parentidcolname { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "stationidcolname", StringLength = 50, DbType = "varchar(50)")]
        public System.String? stationidcolname { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "tabledescription", StringLength = -1, DbType = "text")]
        public System.String? tabledescription { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "tableitemdescription", StringLength = 50, DbType = "varchar(50)")]
        public System.String? tableitemdescription { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "tablename", StringLength = 50, IsNullable = false, DbType = "varchar(50)")]
        public System.String tablename { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "tablepkcolname", StringLength = 50, DbType = "varchar(50)")]
        public System.String? tablepkcolname { get; set; }
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "tabletype", DbType = "int2")]
        public System.Int16? tabletype { get; set; }
    }
}
