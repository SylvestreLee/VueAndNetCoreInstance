using GCP.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Model.Result
{
    public class ZtreeInfo
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public string id { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public string pId { get; set; }

        public string name { get; set; }
    }

    public class ElementTreeInfo
    {
        public ElementTreeInfo()
        {
            children = new List<ElementTreeInfo>();
        }
        public string id { get; set; }
        public string name { get; set; }
        public string authorize { get; set; }
        public string icon { get; set; }
        public int? sort { get; set; }
        public string target { get; set; }
        public int type { get; set; }
        public string url { get; set; }
        public List<ElementTreeInfo> children { get; set; }
    }
}
