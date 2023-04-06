using GCP.Entity.SystemManage;
using GCP.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Model.Result
{
    public class MenuAuthorizeInfo
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? MenuId { get; set; }
        /// <summary>
        /// 用户Id或者角色Id
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? AuthorizeId { get; set; }
        /// <summary>
        ///  用户或者角色
        /// </summary>
        public int? AuthorizeType { get; set; }
        /// <summary>
        /// 权限标识
        /// </summary>
        public string Authorize { get; set; }

    }

    public class UserRoleInfo
    {
        public UserRoleInfo()
        {
            Permissions = new List<string>();
            Roles = new List<string>();
            Rolelist = new List<SysRoleEntity>();
        }
        public List<string> Permissions { get; set; }
        public List<string> Roles { get; set; }
        public List<SysRoleEntity> Rolelist { get; set; }
    }

    public class UserMenuInfo
    {
        public UserMenuInfo()
        {
            Meta = new MetaInfo();
            Children = new List<PageInfo>();
        }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Redirect = "noRedirect";
        public string Component = "Layout";
        public bool AlwaysShow = true;
        public MetaInfo Meta { get; set; }
        public List<PageInfo> Children { get; set; }
    }

    public class MetaInfo
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
    }

    public class PageInfo
    {
        public PageInfo()
        {
            Meta = new MetaInfo();
        }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool AlwaysShow = true;
        /// <summary>
        /// 路径
        /// </summary>
        public string Component { get; set; }
        public MetaInfo Meta { get; set; }
    }
}
