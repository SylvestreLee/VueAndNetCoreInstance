using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Model.Param.SystemManage
{
    public class LogLoginListParam : DateTimeParam
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 日志状态
        /// </summary>
        public int? LogStatus { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        public string? IpAddress { get; set; }
    }

    public class LogOperateListParam : DateTimeParam
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 执行地址
        /// </summary>
        public string? ExecuteUrl { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string? ExecuteParam { get; set; }
    }
}
