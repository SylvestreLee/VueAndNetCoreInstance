using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Model.Result.SystemManage
{
    public class TableInfo
    {
        /// <summary>
        /// 表的Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 主键名
        /// </summary>
        public string TableKeyName { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string TableKey { get; set; }
    }
}
