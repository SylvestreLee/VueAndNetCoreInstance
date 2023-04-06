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
    public class UserBelongService
    {
        private RepositoryBase<SysUserBelongEntity> repositoryBase = new RepositoryBase<SysUserBelongEntity>();

        #region 获取数据
        public async Task<List<SysUserBelongEntity>> GetList(SysUserBelongEntity entity)
        {
            var expression = LinqExtensions.True<SysUserBelongEntity>();
            if (entity != null)
            {
                //这里要先判断数组
                if (entity.UserIds != null)
                {
                    expression = expression.And(t => entity.UserIds.Contains(t.UserId));
                }
                else if (entity.UserId != null)
                {
                    expression = expression.And(t => t.UserId == entity.UserId);
                }

                //其他条件不写else

            }
            var list = await repositoryBase.Where(expression).ToListAsync();
            return list.ToList();
        }

        public async Task<SysUserBelongEntity> GetEntity(long id)
        {
            return await repositoryBase.FindAsync(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(SysUserBelongEntity entity)
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

        public async Task DeleteForm(long id)
        {
            await repositoryBase.DeleteLogicallyAsync(id);
        }
        #endregion
    }
}
