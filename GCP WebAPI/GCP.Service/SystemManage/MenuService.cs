using GCP.Entity.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.Repository;
using GCP.Util;
using GCP.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GCP.Service.SystemManage
{
    public class MenuService
    {
        private RepositoryBase<SysMenuEntity> repositoryBase = new RepositoryBase<SysMenuEntity>();

        #region 获取数据

        /// <summary>
        /// 获取平台的菜单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<SysMenuEntity>> GetList(MenuListParam param)
        {
            var expression = ListFilter(param);
            var list = await repositoryBase.Where(expression).ToListAsync();
            return list.OrderBy(p => p.MenuSort).ToList();
        }
        /// <summary>
        /// 获取登录系统菜单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<SysMenuEntity>> GetGCLoginList(MenuListParam param)
        {
            var expression = GCLoginListFilter(param);
            var list = await repositoryBase.Where(expression).ToListAsync();
            return list.OrderBy(p => p.MenuSort).ToList();
        }


        public async Task<SysMenuEntity> GetEntity(long id)
        {
            return await repositoryBase.FindAsync(id);
        }

        public async Task<long> GetMaxSort(long parentId)
        {
            var max = await repositoryBase.Where(ex => ex.Status != 2).MaxAsync(m => m.ID);
            max++;
            return max;
        }

        public async Task<bool> ExistMenuName(SysMenuEntity entity)
        {
            var expression = LinqExtensions.True<SysMenuEntity>();
            expression = expression.And(t => t.Status != 2);
            if (entity.ID.IsNullOrZero())
            {
                expression = expression.And(t => t.MenuName == entity.MenuName && t.MenuType == entity.MenuType);
            }
            else
            {
                expression = expression.And(t => t.MenuName == entity.MenuName && t.MenuType == entity.MenuType && t.ID != entity.ID);
            }
            return await repositoryBase.Where(expression).CountAsync() > 0 ? true : false;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(SysMenuEntity entity)
        {
            if (entity.ID.IsNullOrZero())
            {
                entity.Create();
                await repositoryBase.InsertAsync(entity);
            }
            else
            {
                entity.Modify();
                await repositoryBase.UpdateAsync(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            //使用事务
            using (var uow = Sql.GetFsql().CreateUnitOfWork())
            {
                try
                {
                    long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                    //逻辑删除菜单
                    await uow.Orm.Update<SysMenuEntity>().Set(e => e.Status == 2).Where(ex => idArr.Contains(ex.ID)).ExecuteAffrowsAsync();
                    //删除菜单权限
                    await uow.Orm.Delete<SysMenuAuthorizeEntity>().Where(ex => idArr.Contains(ex.MenuId.Value)).ExecuteAffrowsAsync();
                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region 私有方法
        private Expression<Func<SysMenuEntity, bool>> ListFilter(MenuListParam param)
        {
            Expression<Func<SysMenuEntity, bool>> expression = (ex => ex.Status != 2 && ex.PlatformType == 1);
            if (param != null)
            {
                //if (!string.IsNullOrEmpty(param.MenuName))
                //{
                //    expression = expression.And(t => t.MenuName.Contains(param.MenuName));
                //}
                //if (param.MenuStatus > -1)
                //{
                //    expression = expression.And(t => t.MenuStatus == param.MenuStatus);
                //}
                if (!param.SystemType.IsNullOrZero())
                {
                    expression = expression.And(ex => ex.SystemType == param.SystemType);
                }
            }
            return expression;
        }

        private Expression<Func<SysMenuEntity, bool>> GCLoginListFilter(MenuListParam param)
        {
            Expression<Func<SysMenuEntity, bool>> expression = (ex => ex.Status != 2&&ex.PlatformType==2);
            if (param != null)
            {
                if (!param.SystemType.IsNullOrZero())
                {
                    expression = expression.And(ex => ex.SystemType == param.SystemType);
                }
            }
            return expression;
        }

        #endregion
    }
}
