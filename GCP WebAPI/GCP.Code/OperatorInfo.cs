using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace GCP.Code
{
    public class OperatorInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public long? UserStatus { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 登陆时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }
        /// <summary>
        /// 主平台登陆Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 是否是系统人员
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 1:管理部门，根据OrganizationID分配区县环保局权限
        /// 2.产品供应商
        /// 3.检测机构，根据OrganizationID分配检测站权限
        /// 4.监督检查机构
        /// 5.车辆M站维修机构
        /// </summary>
        public long? OrganizationType { get; set; }
        /// <summary>
        /// 根据OrganizationType的类型分配权限
        /// </summary>
        public long? OrganizationID { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        [Column(IsIgnore = true)]
        public string RoleIds { get; set; }
    }
}
