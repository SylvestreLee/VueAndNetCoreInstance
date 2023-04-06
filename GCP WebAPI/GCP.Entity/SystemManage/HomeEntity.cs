using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace GCP.Entity.SystemManage
{
    /// <summary>
    /// 首页实体类
    /// </summary>
    public partial class HomeEntity
    {
        /// <summary>
        /// 汽柴占比
        /// </summary>
        public GasDieselEntity GasDiesel { get; set; }

        /// <summary>
        /// 汽油双怠速
        /// </summary>
        public List<InsMethodEntity> DIS { get; set; }

        /// <summary>
        /// 柴油自由加速不透光
        /// </summary>
        public List<InsMethodEntity> BTG { get; set; }

        /// <summary>
        /// 自动审核统计
        /// </summary>
        public List<AutoInsEntity> AutoIns { get; set; }
    }

    /// <summary>
    /// 本年度汽柴占比
    /// </summary>
    public class GasDieselEntity
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// 汽油数量
        /// </summary>
        public int GasTotal { get; set; } = 0;

        /// <summary>
        /// 汽油占比
        /// </summary>
        public string GasRatio { get; set; } = "";

        /// <summary>
        /// 柴油数量
        /// </summary>
        public int DieselTotal { get; set; } = 0;

        /// <summary>
        /// 柴油占比
        /// </summary>
        public string DieselRatio { get; set; } = "";

        /// <summary>
        /// 其他数量
        /// </summary>
        public int OtherTotal { get; set; } = 0;

        /// <summary>
        /// 其他占比
        /// </summary>
        public string OtherRatio { get; set; } = "";
    }

    /// <summary>
    /// 本月检测方法统计
    /// </summary>
    public class InsMethodEntity
    { 
        /// <summary>
        /// 检测站名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// 检测方法数量
        /// </summary>
        public int MethodTotal { get; set; } = 0;

        /// <summary>
        /// 检测方法比例
        /// </summary>
        public double MethodRatio { get; set; } = 0;

        /// <summary>
        /// 检测方法比例
        /// </summary>
        public string StrMethodRatio { get; set; } = "";
    }

    /// <summary>
    /// 自动审核统计
    /// </summary>
    public class AutoInsEntity
    { 
        /// <summary>
        /// 检测站名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// 通过数量
        /// </summary>
        public int PassTotal { get; set; } = 0;
        
        /// <summary>
        /// 不通过数量
        /// </summary>
        public int FailTotal { get; set; } = 0;

        /// <summary>
        /// 预警数量
        /// </summary>
        public int WarningTotal { get; set; } = 0;

        /// <summary>
        /// 异常数量
        /// </summary>
        public int UnusualTotal { get; set; } = 0;
    }
}
