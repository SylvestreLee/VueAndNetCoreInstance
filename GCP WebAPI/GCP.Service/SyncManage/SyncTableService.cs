using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeSql.DatabaseModel;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using GCP.Repository;
using GCP.Entity.SyncManage;
using GCP.Model.Param.SyncManage;

namespace GCP.Service.SyncManage
{
    /// <summary>
    /// 创 建：manage
    /// 日 期：2022-03-02 15:48
    /// 描 述：服务类
    /// </summary>
    public class SyncTableService
    {

        private RepositoryBase<SyncTableEntity> repositoryBase = new RepositoryBase<SyncTableEntity>();
        private IFreeSql sql = Sql.GetFsql();
        #region 获取数据

        public async Task<List<SyncTableNameEntity>> GetList()
        {
            Expression<Func<SyncTableEntity, bool>> expression = ((ex) => ex.Status == 1 && ((ex.ifsyncdown == true || ex.ifsyncup == true || ex.tabletype != null)));


            var list = await sql.Select<SyncTableEntity>()
                            .Where(expression)
                             .OrderBy("tablename desc")
                .ToListAsync(a => new SyncTableNameEntity()
                {
                    name = a.tablename,
                    type = (short)a.tabletype,
                });
            return list;
        }
        #endregion 

    }
}
