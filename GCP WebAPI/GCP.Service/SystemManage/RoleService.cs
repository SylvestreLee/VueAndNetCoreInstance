using GCP.Entity.SystemManage;
using GCP.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GCP.Util;
using GCP.Util.Model;
using GCP.Model.Param.SystemManage;
using GCP.Util.Extension;
using System.Linq;
using System.Linq.Expressions;
using GCP.Enum.SystemManage;

namespace GCP.Service.SystemManage
{
    public class RoleService
    {
        private RepositoryBase<SysRoleEntity> repositoryBase = new RepositoryBase<SysRoleEntity>();

        #region 获取数据

        public async Task<List<SysRoleEntity>> GetList(RoleListParam param)
        {
            var expression = ListFilter(param);
            var list = await repositoryBase
                            .Where(expression)
                            .OrderBy(t => t.RoleSort)
                            .ToListAsync();
            return list;
        }



        public async Task<List<SysRoleEntity>> GetLoginList(RoleListParam param)
        {
            var expression = ListLoginFilter(param);
            var list = await repositoryBase
                            .Where(expression)
                            .OrderBy(t => t.RoleSort)
                            .ToListAsync();
            return list;
        }

        public async Task<List<SysRoleEntity>> GetPageList(RoleListParam param, Pagination pagination)
        {
            pagination.Sort = "RoleSort";
            pagination.SortType = "";
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

        public async Task<List<SysRoleEntity>> GetLoginPageList(RoleListParam param, Pagination pagination)
        {
            pagination.Sort = "RoleSort";
            pagination.SortType = "";
            var expression = ListLoginFilter(param);
            var Queryable = repositoryBase
                            .Where(expression)
                            .OrderBy(pagination.Sort + " " + pagination.SortType + " ");
            pagination.TotalCount = (await Queryable.CountAsync()).ParseToInt();
            var list = await Queryable
                            .Page(pagination.PageIndex, pagination.PageSize)
                            .ToListAsync();
            return list;
        }

        public async Task<SysRoleEntity> GetEntity(long id)
        {
            return await repositoryBase.FindAsync(id);
        }

        public async Task<long> GetMaxSort()
        {
            var max = await repositoryBase.Where(ex => ex.Status != 2).MaxAsync(m => m.ID);
            max++;
            return max;
        }

        public async Task<bool> ExistRoleName(SysRoleEntity entity)
        {
            var expression = LinqExtensions.True<SysRoleEntity>();
            expression = expression.And(t => t.Status != 2);
            if (entity.ID.IsNullOrZero())
            {
                expression = expression.And(t => t.RoleName == entity.RoleName);
            }
            else
            {
                expression = expression.And(t => t.RoleName == entity.RoleName && t.ID != entity.ID);
            }
            return await repositoryBase.Where(expression).CountAsync() > 0 ? true : false;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(SysRoleEntity entity)
        {
            //使用事务
            using (var uow = Sql.GetFsql().CreateUnitOfWork())
            {
                try
                {
                    if (entity.ID.IsNullOrZero())
                    {
                        entity.Create();
                        var ID = await uow.Orm.Insert(entity).ExecuteIdentityAsync();
                        entity.ID = ID;
                    }
                    else
                    {
                        //删除权限
                        await uow.Orm.Delete<SysMenuAuthorizeEntity>().Where(t => t.AuthorizeId == entity.ID).ExecuteAffrowsAsync();

                        entity.Modify();
                        await uow.Orm.Update<SysRoleEntity>().SetSource(entity).ExecuteAffrowsAsync();
                    }
                    // 角色对应的菜单、页面和按钮权限
                    if (!string.IsNullOrEmpty(entity.MenuIds))
                    {
                        List<SysMenuAuthorizeEntity> sysMenuAuthorizeEntities = new List<SysMenuAuthorizeEntity>();
                        foreach (long menuId in TextHelper.SplitToArray<long>(entity.MenuIds, ','))
                        {
                            SysMenuAuthorizeEntity menuAuthorizeEntity = new SysMenuAuthorizeEntity();
                            menuAuthorizeEntity.AuthorizeId = entity.ID;
                            menuAuthorizeEntity.MenuId = menuId;
                            menuAuthorizeEntity.AuthorizeType = AuthorizeTypeEnum.Role.ParseToInt();
                            menuAuthorizeEntity.Create();
                            sysMenuAuthorizeEntities.Add(menuAuthorizeEntity);
                        }
                        await uow.Orm.Insert(sysMenuAuthorizeEntities).ExecuteAffrowsAsync();
                    }
                    uow.Commit();
                }
                catch(Exception err)
                {
                    uow.Rollback();
                    throw;
                }
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
                    //逻辑删除用户
                    await uow.Orm.Update<SysRoleEntity>().Set(e => e.Status == 2).Where(ex => idArr.Contains(ex.ID)).ExecuteAffrowsAsync();
                    //删除权限
                    await uow.Orm.Delete<SysMenuAuthorizeEntity>().Where(t => idArr.Contains(t.AuthorizeId.Value)).ExecuteAffrowsAsync();
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

        private Expression<Func<SysRoleEntity, bool>> ListFilter(RoleListParam param)
        {
            //var expression = LinqExtensions.True<RoleEntity>();
            Expression<Func<SysRoleEntity, bool>> expression = (ex => ex.Status != 2&&ex.PlatformType == 1);
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.RoleName))
                {
                    expression = expression.And(t => t.RoleName.Contains(param.RoleName));
                }
                if (!string.IsNullOrEmpty(param.RoleIds))
                {
                    long[] roleIdArr = TextHelper.SplitToArray<long>(param.RoleIds, ',');
                    expression = expression.And(t => roleIdArr.Contains(t.ID));
                }
                if (!string.IsNullOrEmpty(param.RoleName))
                {
                    expression = expression.And(t => t.RoleName.Contains(param.RoleName));
                }
            }
            return expression;
        }


        private Expression<Func<SysRoleEntity, bool>> ListLoginFilter(RoleListParam param)
        {
            //var expression = LinqExtensions.True<RoleEntity>();
            Expression<Func<SysRoleEntity, bool>> expression = (ex => ex.Status != 2&&ex.PlatformType==2);
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.RoleName))
                {
                    expression = expression.And(t => t.RoleName.Contains(param.RoleName));
                }
                if (!string.IsNullOrEmpty(param.RoleIds))
                {
                    long[] roleIdArr = TextHelper.SplitToArray<long>(param.RoleIds, ',');
                    expression = expression.And(t => roleIdArr.Contains(t.ID));
                }
                if (!string.IsNullOrEmpty(param.RoleName))
                {
                    expression = expression.And(t => t.RoleName.Contains(param.RoleName));
                }
            }
            return expression;
        }

        #endregion
    }
}
