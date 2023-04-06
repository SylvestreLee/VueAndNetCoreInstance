using GCP.Entity.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.Repository;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GCP.Service.SystemManage
{
    public class LogLoginService
    {
        private RepositoryBase<SysLogLoginEntity> repositoryBase = new RepositoryBase<SysLogLoginEntity>();

        #region 获取数据
        public async Task<List<SysLogLoginEntity>> GetList(LogLoginListParam param)
        {
            var expression = ListFilter(param);
            var list = await repositoryBase
                            .Where(expression)
                            .ToListAsync();
            return list;
        }

        public async Task<List<SysLogLoginEntity>> GetPageList(LogLoginListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var Queryable = repositoryBase
                            .Where(expression)
                            .OrderBy(pagination.Sort + " " + pagination.SortType + " ");
            pagination.TotalCount = (await Queryable.CountAsync()).ParseToInt();
            var list = await Queryable
                            .Page(pagination.PageIndex, pagination.PageSize)
                            .ToListAsync();
            return list;
        }

        public async Task<SysLogLoginEntity> GetEntity(long id)
        {
            return await repositoryBase.FindAsync(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(SysLogLoginEntity entity)
        {
            if (entity.ID.IsNullOrZero())
            {
                entity.Create();
                await repositoryBase.InsertAsync(entity);
            }
            else
            {
                await repositoryBase.UpdateAsync(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await repositoryBase.DeleteLogicallyAsync(idArr.ToArray());
        }

        public async Task RemoveAllForm()
        {
            await Sql.GetFsql()
                    .Update<SysLogOperateEntity>()
                    .Set(c => c.Status == 2)
                    .ExecuteAffrowsAsync();
        }
        #endregion

        #region 私有方法

        private Expression<Func<SysLogLoginEntity, bool>> ListFilter(LogLoginListParam param)
        {
            Expression<Func<SysLogLoginEntity, bool>> expression = (ex => ex.Status != 2);
            if (param != null)
            {

            }
            return expression;
        }

        #endregion
    }
}
