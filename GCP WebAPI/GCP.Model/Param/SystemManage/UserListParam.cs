using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Model.Param.SystemManage
{
    public class UserListParam
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 多个用户ID
        /// </summary>
        public string? UserIds { get; set; }

        /// <summary>
        /// 单选的用户所属单位
        /// </summary>
        public string? UserOrg { get; set; }

        /// <summary>
        /// 给检测站人员使用
        /// </summary>
        public long? OrganizationType { get; set; }
    }

    public class ChangePasswordParam
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string? NewPassword { get; set; }
    }

    public class LoginParam
    { 
        /// <summary>
        /// 用户名
        /// </summary>
        public string? userName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? password { get; set; }
    }
}
