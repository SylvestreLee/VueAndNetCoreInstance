using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Model.Param.SystemManage
{
    public class RoleListParam
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// 多个角色Id
        /// </summary>
        public string? RoleIds { get; set; }
    }
}
