using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DatabaseModel;
using GCP.Entity.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.Model.Result.SystemManage;
using GCP.Repository;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;

namespace GCP.Service.SystemManage
{
    public class DatabaseTableService
    {
        private IFreeSql sql = Sql.GetFsql();

        public Task<List<TableInfo>> GetTableList(string tableName)
        {
            var task = Task.Run(() =>
            {
                List<TableInfo> tableInfos = new List<TableInfo>();
                var Tables = (sql.DbFirst.GetTablesByDatabase(GlobalContext.SystemConfig.DBFirstDatabase).Where(t => t.Type == DbTableType.TABLE)).ToList();
                if (tableName != null)
                {
                    Tables = Tables.Where(t => t.Name.ToUpper().Contains(tableName.ToUpper())).ToList();
                }
                foreach (DbTableInfo table in Tables)
                {
                    TableInfo t = new TableInfo();
                    t.Id = table.Id.ParseToLong();
                    t.TableName = table.Name;
                    t.TableKeyName = table.Comment;
                    t.TableKey = table.Primarys.Count > 0 ? table.Primarys[0].Name : "";
                    tableInfos.Add(t);
                }
                return tableInfos;
            });

            return task;
        }

        public Task<List<TableInfo>> GetTablePageList(string tableName, Pagination pagination)
        {
            var task = Task.Run(() =>
            {
                List<TableInfo> tableInfos = new List<TableInfo>();
                var Tables = (sql.DbFirst.GetTablesByDatabase(GlobalContext.SystemConfig.DBFirstDatabase).Where(t => t.Type == DbTableType.TABLE)).ToList();
                if (tableName != null)
                {
                    Tables = Tables.Where(t => t.Name.ToUpper().Contains(tableName.ToUpper())).ToList();
                }
                pagination.TotalCount = Tables.Count;
                Tables = Tables.Skip((pagination.PageIndex - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
                foreach (DbTableInfo table in Tables)
                {
                    TableInfo t = new TableInfo();
                    t.Id = table.Id.ParseToLong();
                    t.TableName = table.Name;
                    t.TableKeyName = table.IndexesDict.Count > 0 ? table.IndexesDict.First().Key : "";
                    t.TableKey = table.Primarys.Count > 0 ? table.Primarys[0].Name : "";
                    tableInfos.Add(t);
                }
                return tableInfos;
            });
            return task;
        }

        public Task<List<DbColumnInfo>> GetTableFieldList(string TableName)
        {
            var task = Task.Run(() => {

                List<DbColumnInfo> dbColumnInfos = new List<DbColumnInfo>();
                StringBuilder SbText = new StringBuilder();

                var Fsql = Sql.GetFsql();
                var Tables = (Fsql.DbFirst.GetTablesByDatabase(GlobalContext.SystemConfig.DBFirstDatabase).Where(t => t.Type == DbTableType.TABLE && t.Name.ToUpper() == TableName.ToUpper())).ToList();
                if (Tables.Count > 0)
                {
                    //var TableStruct = Fsql.DbFirst.GetTableByName(TableName);
                    //dbColumnInfos = TableStruct.Columns;
                    dbColumnInfos = Tables[0].Columns;
                }

                return dbColumnInfos;
            });

            return task;
        }
    }
}
