using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Model.Result.SystemManage
{
    public class UserAuthorizeInfo
    {
        public bool IsSystem { get; set; }
        public List<MenuAuthorizeInfo> MenuAuthorize { get; set; }
    }
}
