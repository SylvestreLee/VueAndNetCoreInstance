using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GCP.Model.Param.SystemManage
{
    public class MenuListParam
    {
        [JsonIgnore]
        public int? SystemType { get; set; }
    }
}
