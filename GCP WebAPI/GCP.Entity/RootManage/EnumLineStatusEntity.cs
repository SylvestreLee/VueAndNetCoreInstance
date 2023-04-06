using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "enum_line_status")]
    public partial class EnumLineStatusEntity : BaseIDEntity
    {
        
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "name", StringLength = 10, IsNullable = false, DbType = "nvarchar(10)")]
        public System.String Name { get; set; }
        
       
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "remarks", StringLength = -1, DbType = "text")]
        public System.String? Remarks { get; set; }
    }
}
