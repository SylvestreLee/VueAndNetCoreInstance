using FreeSql.DatabaseModel;
using GCP.Repository;
using GCP.Util.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using GCP.Model.Result.SystemManage;
using GCP.Service.SystemManage;

namespace GCP.Business.SystemManage
{
    public class DatabaseTableBLL
    {
        private DatabaseTableService databaseTableService = new DatabaseTableService();

        #region 获取数据

        public async Task<TData<List<TableInfo>>> GetTableList(string tableName)
        {
            TData<List<TableInfo>> obj = new TData<List<TableInfo>>();
            List<TableInfo> list = await databaseTableService.GetTableList(tableName);
            obj.Data = list;
            obj.Total = list.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<TableInfo>>> GetTablePageList(string tableName, Pagination pagination)
        {
            TData<List<TableInfo>> obj = new TData<List<TableInfo>>();
            List<TableInfo> list = await databaseTableService.GetTablePageList(tableName, pagination);
            obj.Data = list;
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<TData<List<DbColumnInfo>>> GetTableFieldList(string tableName)
        {
            TData<List<DbColumnInfo>> obj = new TData<List<DbColumnInfo>>();
            List<DbColumnInfo> list = await databaseTableService.GetTableFieldList(tableName);
            obj.Data = list;
            obj.Total = list.Count;
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
