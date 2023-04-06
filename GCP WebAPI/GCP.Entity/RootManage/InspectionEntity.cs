using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace GCP.Entity.RootManage
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = "inspection")]
    public partial class InspectionEntity : BaseNoPrimaryEntity
    {
        ///登录的时候局端特有的字段值（或是操作的时候局端特有的值），这样的字段必须同步到站上，避免后期同步上传的时候覆盖掉
        /// 
        /// <summary>
        /// 检验流水号
        /// </summary>
        [Description("检验流水号")]
        [JsonProperty, Column(IsPrimary = true, StringLength = 50,Name ="inspectionid")]
        public System.String InspectionID { get; set; }


        /// <summary> 
        ///  取消报检原因ID
        /// </summary> 
        [Description("取消报检原因ID")]
        [Column(Name = "abortreason", DbType = "bigint")]
        public System.Int64? AbortReason { get; set; }


      

        /// <summary> 
        ///  公共流水号
        /// </summary> 
        [Description("公共流水号")]
        [JsonProperty, Column(Name = "cominspectionid", StringLength = 50, DbType = "varchar(50)")]
        public System.String ComInspectionID { get; set; }

        /// <summary> 
        ///  是否允许跨站检测
        /// </summary> 
        [Description("是否允许跨站检测")]
        [JsonProperty, Column(Name = "crosssiteinspection", DbType = "bit")]
        public System.Boolean? CrossSiteInspection { get; set; }

        /// <summary> 
        ///  检测记录序号
        /// </summary> 
        [Description("安徽地区专用")]
        [JsonProperty, Column(Name = "inspectionidah", StringLength = 50, DbType = "varchar(50)")]
        public System.String? InspectionIDAH { get; set; }

        

        /// <summary> 
        ///  检测燃料类型ID
        /// </summary> 
        [Description("检测燃料类型ID")]
        [JsonProperty, Column(Name = "fueltypeid", DbType = "smallint")]
        public System.Int16? FuelTypeID { get; set; }

        /// <summary> 
        ///  处理跨站检测的人员ID
        /// </summary> 
        [Description("处理跨站检测的人员ID")]
        [JsonProperty, Column(Name = "handlecsperson", DbType = "bigint")]
        public System.Int64? HandleCSPerson { get; set; }


        /// <summary> 
        ///  检测次数
        /// </summary> 
        [Description("检测次数")]
        [JsonProperty, Column(Name = "inspectioncount", DbType = "bigint")]
        public System.Int64 InspectionCount { get; set; }

        /// <summary> 
        ///  IM制度维修次数
        /// </summary> 
        [Description("IM制度维修次数")]
        [JsonProperty, Column(Name = "imwxcs", DbType = "int")]
        public System.Int32 IMWXCS { get; set; }

        /// <summary> 
        ///  检测已完成项目Value
        /// </summary> 
        [Description("检测已完成项目")]
        [JsonProperty, Column(Name = "inspectionitem", DbType = "bigint")]
        public System.Int64? InspectionItem { get; set; }



        /// <summary> 
        ///  报检检测方法ID
        /// </summary> 
        [Description("报检检测方法ID")]
        [JsonProperty, Column(Name = "inspectionmethod", DbType = "bigint")]
        public System.Int64? InspectionMethod { get; set; }



        /// <summary> 
        ///  检测类型ID
        /// </summary> 
        [Description("检测类型ID")]
        [JsonProperty, Column(Name = "inspectiontype", DbType = "bigint")]
        public System.Int64 InspectionType { get; set; }

       

        /// <summary> 
        ///  本次检测所属周期的有效期至
        /// </summary> 
        [Description("本次检测所属周期的有效期至")]
        [JsonProperty, Column(Name = "inspectionvalidto", DbType = "datetime")]
        public System.DateTime? InspectionValidTo { get; set; }

        /// <summary> 
        /// 检测记录版本，是否老平台数据  
        /// </summary> 
        [JsonProperty, Column(Name = "inspectionversion", StringLength = 10, DbType = "nvarchar(10)")]
        public System.String InspectionVersion { get; set; }



        /// <summary> 
        ///  上次检测方法枚举值
        /// </summary> 
        [Description("上次检测方法枚举值")]
        [JsonProperty, Column(Name = "lastinspectionmethod", DbType = "bigint")]
        public System.Int64? LastInspectionMethod { get; set; }


        /// <summary> 
        ///  登录员ID
        /// </summary> 
        [Description("登录员ID")]
        [JsonProperty, Column(Name = "loginerid", DbType = "bigint")]
        public System.Int64? LoginerID { get; set; }


        /// <summary> 
        ///  报检时间
        /// </summary> 
        [Description("报检时间")]
        [JsonProperty, Column(Name = "logintime", DbType = "datetime")]
        public System.DateTime LoginTime { get; set; }


        /// <summary> 
        ///  是否免检
        /// </summary> 
        [Description("是否免检")]
        [JsonProperty, Column(Name = "noinspection", DbType = "bit")]
        public System.Boolean? NOInspection { get; set; }
        /// <summary> 
        ///  上次是否维修
        /// </summary> 
        [Description("上次是否维修")]
        [JsonProperty, Column(Name = "lastinsifrepair", DbType = "bit")]
        public System.Boolean? LastInsIFRepair { get; set; }
        /// <summary> 
        ///  是否脱审
        /// </summary> 
        [Description("是否脱审")]
        [JsonProperty, Column(Name = "iftuoshen", DbType = "bit")]
        public System.Boolean? IFTuoShen { get; set; }
        /// <summary> 
        ///  下次检测开始时间
        /// </summary> 
        [Description("下次检测开始时间")]
        [JsonProperty, Column(Name = "nextinsfrom", DbType = "bit")]
        public System.DateTime? NextInsFrom { get; set; }

        /// <summary> 
        ///  下次检测周期有效期
        /// </summary> 
        [Description("下次检测周期有效期")]
        [JsonProperty, Column(Name = "nextinsto", DbType = "bit")]
        public System.DateTime? NextInsTo { get; set; }



        /// <summary> 
        ///  实际使用检测方法枚举值
        /// </summary> 
        [Description("实际使用检测方法枚举值")]
        [JsonProperty, Column(Name = "realinspectionmethod", DbType = "int")]
        public System.Int32? RealInspectionMethod { get; set; }

        /// <summary> 
        ///  推荐（按规则自动计算得出）的有效期至
        /// </summary> 
        [Description("推荐（按规则自动计算得出）的有效期至")]
        [JsonProperty, Column(Name = "recomendedvalidto", DbType = "datetime")]
        public System.DateTime RecomendedValidTo { get; set; }



        /// <summary> 
        ///  
        /// </summary> 
        [JsonProperty, Column(Name = "remarks", StringLength = -1, DbType = "text")]
        public System.String Remarks { get; set; }


        /// <summary> 
        ///  
        /// </summary> 
        [Column(Name = "specialflag", DbType = "bigint")]
        public System.Int64? SpecialFlag { get; set; }

        /// <summary> 
        ///  检测站的ID
        /// </summary> 
        [Description("检测站的ID")]
        [JsonProperty, Column(Name = "stationid", DbType = "bigint")]
        public System.Int64? StationID { get; set; }


        /// <summary> 
        ///  检测线ID
        /// </summary> 
        [Description("检测线ID")]
        [JsonProperty, Column(Name = "lineid", DbType = "bigint")]
        public System.Int64? LineID { get; set; }

        /// <summary> 
        ///  车辆表ID
        /// </summary> 
        [JsonProperty, Column(Name = "vehicleid", DbType = "bigint")]
        public System.Int64 VehicleID { get; set; }


        /// <summary> 
        ///  车辆大架子号
        /// </summary> 
        [Description("车辆大架子号")]
        [JsonProperty, Column(Name = "vin", StringLength = 20, IsNullable = false, DbType = "nvarchar(20)")]
        public System.String VIN { get; set; }



        /// <summary> 
        ///  取消报检人员ID
        /// </summary> 
        [Description("取消报检人员ID")]
        [JsonProperty, Column(Name = "handleabortperson", DbType = "bigint")]
        public System.Int64? HandleAbortPerson { get; set; }


        /// <summary> 
        ///  终止检测人员ID
        /// </summary> 
        [Description("终止检测人员ID")]
        [JsonProperty, Column(Name = "handlestopperson", DbType = "bigint")]
        public System.Int64? HandleStopPerson { get; set; }


        /// <summary> 
        ///  处理跨过OBD检查人员ID
        /// </summary> 
        [Description("处理跨过OBD检查人员ID")]
        [JsonProperty, Column(Name = "handleskipobdperson", DbType = "bigint")]
        public System.Int64? HandleSkipOBDPerson { get; set; }

        /// <summary> 
        ///  检验日期
        /// </summary> 
        [Description("检验日期")]
        [JsonProperty, Column(Name = "jyrq", DbType = "datetime")]
        public System.DateTime? JYRQ { get; set; }

        /// <summary> 
        ///  检测报告单编号
        /// </summary> 
        [Description("检测报告单编号")]
        [JsonProperty, Column(Name = "reportnumber", StringLength = 50, DbType = "nvarchar(50)")]
        public System.String? ReportNumber { get; set; }

        /// <summary> 
        ///  检测结果枚举值
        /// </summary> 
        [Description("检测结果枚举值")]
        [JsonProperty, Column(Name = "result", DbType = "bigint")]
        public System.Int64? Result { get; set; }

    }
}
