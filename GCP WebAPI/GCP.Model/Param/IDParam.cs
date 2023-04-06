using GCP.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Model.Param
{
    public class IDParam
    {
        /// <summary>
        /// 所有表的主键
        /// long返回到前端js的时候，会丢失精度，所以转成字符串
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ID { get; set; }
    }
}
