using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "vehicle")]
    public partial class VehicleEntity : BaseEntity
    {
        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "airsupplyid", DbType = "int8")]
        public System.Int64? airsupplyid { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "areacode", StringLength = 6, DbType = "varchar(6)")]
        public System.String? areacode { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "axleweight", StringLength = 20, DbType = "varchar(20)")]
        public System.String? axleweight { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "brandname", StringLength = 50, DbType = "varchar(50)")]
        public System.String? brandname { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "changeago_method", DbType = "int8")]
        public System.Int64? changeago_method { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "chassisfactory", StringLength = 20, DbType = "varchar(20)")]
        public System.String? chassisfactory { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "chassismodel", StringLength = 20, DbType = "varchar(20)")]
        public System.String? chassismodel { get; set; }





        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "chzhqmodel", StringLength = 50, DbType = "varchar(50)")]
        public System.String? chzhqmodel { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "cnzzmodel", StringLength = 50, DbType = "varchar(50)")]
        public System.String? cnzzmodel { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "confirmstatus", DbType = "int8")]
        public System.Int64? confirmstatus { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "curbweight", DbType = "int8")]
        public System.Int64? curbweight { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "cylinders", DbType = "int4")]
        public System.Int32? cylinders { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "ddjmodel", StringLength = 50, DbType = "varchar(50)")]
        public System.String? ddjmodel { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "default_methods", DbType = "int8")]
        public System.Int64? default_methods { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "deliverycapacity", DbType = "float8")]
        public System.Double? deliverycapacity { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "dipxh", StringLength = 50, DbType = "varchar(50)")]
        public System.String? dipxh { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "disconfirm", DbType = "bool")]
        public System.Boolean? disconfirm { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "dk", DbType = "bool")]
        public System.Boolean? dk { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "dpf", DbType = "bool")]
        public System.Boolean? dpf { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "dpfmodel", StringLength = 50, DbType = "varchar(50)")]
        public System.String? dpfmodel { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "drivetypeid", DbType = "int8")]
        public System.Int64? drivetypeid { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "egr", DbType = "bool")]
        public System.Boolean? egr { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "enginefactoryname", StringLength = 100, DbType = "varchar(100)")]
        public System.String? enginefactoryname { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "enginemodel", StringLength = 100, DbType = "varchar(100)")]
        public System.String? enginemodel { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "enginesn", StringLength = 50, DbType = "varchar(50)")]
        public System.String? enginesn { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "epstageid", DbType = "int8")]
        public System.Int64? epstageid { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "esp", DbType = "bool")]
        public System.Boolean? esp { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "exhaustpipesnum", DbType = "int4")]
        public System.Int32? exhaustpipesnum { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "factorycountry", StringLength = 50, DbType = "varchar(50)")]
        public System.String? factorycountry { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "factorydate", DbType = "timestamp")]
        public System.DateTime? factorydate { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "factoryname", StringLength = 100, DbType = "varchar(100)")]
        public System.String? factoryname { get; set; }





        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "fuelsupplyid", DbType = "int8")]
        public System.Int64? fuelsupplyid { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "fueltype1", DbType = "int8")]
        public System.Int64? fueltype1 { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "fueltype2", DbType = "int8")]
        public System.Int64? fueltype2 { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "fueltype3", DbType = "int8")]
        public System.Int64? fueltype3 { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "gears", DbType = "int4")]
        public System.Int32? gears { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "geartypeid", DbType = "int8")]
        public System.Int64? geartypeid { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "gkfsuccess", DbType = "bool")]
        public System.Boolean? gkfsuccess { get; set; }



        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "has3wcc", DbType = "bool")]
        public System.Boolean? has3wcc { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "hcl", DbType = "bool")]
        public System.Boolean? hcl { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "hcltype", DbType = "int4")]
        public System.Int32? hcltype { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "hgldate", DbType = "timestamp")]
        public System.DateTime? hgldate { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "hglhclfactory", StringLength = 50, DbType = "varchar(50)")]
        public System.String? hglhclfactory { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "hglhclmodel", StringLength = 50, DbType = "varchar(50)")]
        public System.String? hglhclmodel { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "ifgointocity", DbType = "bool")]
        public System.Boolean? ifgointocity { get; set; }


        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "isbusiness", DbType = "bool")]
        public System.Boolean? isbusiness { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "islightcar", DbType = "int2")]
        public System.Int16? islightcar { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "jdcxh", StringLength = 20, DbType = "varchar(20)")]
        public System.String? jdcxh { get; set; }


        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "kwh", DbType = "float8")]
        public System.Double? kwh { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "lightcartype", DbType = "int2")]
        public System.Int16? lightcartype { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "limitmax", StringLength = 20, DbType = "varchar(20)")]
        public System.String? limitmax { get; set; }



        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "limitmin", StringLength = 20, DbType = "varchar(20)")]
        public System.String? limitmin { get; set; }


        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "loginerid", DbType = "int8")]
        public System.Int64? loginerid { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "maxnetpower", DbType = "float8")]
        public System.Double? maxnetpower { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "maxweight", DbType = "int8")]
        public System.Int64? maxweight { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "mntype", DbType = "int2")]
        public System.Int16? mntype { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "model", StringLength = 50, DbType = "varchar(50)")]
        public System.String? model { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "norminalpower", DbType = "float8")]
        public System.Double? norminalpower { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "obd", DbType = "bool")]
        public System.Boolean? obd { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "odometer", DbType = "int8")]
        public System.Int64? odometer { get; set; }



        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "owner", StringLength = 100, DbType = "varchar(100)")]
        public System.String? owner { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "owneraddress", StringLength = -1, DbType = "text")]
        public System.String? owneraddress { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "ownertel", StringLength = 100, DbType = "varchar(100)")]
        public System.String? ownertel { get; set; }



        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "passengercount", DbType = "int4")]
        public System.Int32? passengercount { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "platecolor", DbType = "int8")]
        public System.Int64? platecolor { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "plateid", StringLength = 20, DbType = "varchar(20)")]
        public System.String? plateid { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "platetype", DbType = "int8")]
        public System.Int64? platetype { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "ratedspeed", DbType = "int8")]
        public System.Int64? ratedspeed { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "regdate", DbType = "date")]
        public System.DateTime? regdate { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "remarks", StringLength = -1, DbType = "text")]
        public System.String? remarks { get; set; }



        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "scr", DbType = "bool")]
        public System.Boolean? scr { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "scrmodel", StringLength = 50, DbType = "varchar(50)")]
        public System.String? scrmodel { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "specialflagid", DbType = "int8")]
        public System.Int64? specialflagid { get; set; }




        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "storestatus", DbType = "int8")]
        public System.Int64? storestatus { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "strokecycles", DbType = "int4")]
        public System.Int32? strokecycles { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "tg", DbType = "bool")]
        public System.Boolean? tg { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "usage", StringLength = -1, DbType = "text")]
        public System.String? usage { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "usageid", DbType = "int8")]
        public System.Int64? usageid { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "vehiclemodelid", DbType = "int8")]
        public System.Int64? vehiclemodelid { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "vehicletype2id", DbType = "int8")]
        public System.Int64? vehicletype2id { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "vehicletypeid", DbType = "int8")]
        public System.Int64? vehicletypeid { get; set; }

        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "vehicleversion", StringLength = 10, DbType = "varchar(10)")]
        public System.String? vehicleversion { get; set; }


        /// <summary> 
        /// 
        /// </summary> 
        [Description("")]
        [JsonProperty, Column(Name = "vin", StringLength = 20, IsNullable = false, DbType = "varchar(20)")]
        public System.String vin { get; set; }
    }
}
