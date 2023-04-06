using System;
using System.Collections.Generic;
using System.Text;

namespace GCP.Entity.SyncManage
{
    public  class SyncTableNameEntity
    {
        /// <summary>
        /// tableName
        /// 需要同步的表名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// tableType
        /// 需要同步的表类型
        /// </summary>
        public short type { get; set; }
    }
}
