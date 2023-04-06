using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Model
{
    public class CommonModel
    {

        public class EnumIdNameModel
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }

        public class KeyValueModel
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public class CountModel
        {
            public int Count { get; set; }
        }
    }
}
