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
using GCP.Code;

namespace GCP.Service.SystemManage
{
    public class UserService
    {
        private RepositoryBase<PersonEntity> repositoryBase = new RepositoryBase<PersonEntity>();

        #region 获取数据

        public async Task<List<PersonEntity>> GetList(UserListParam param)
        {
            var expression = ListFilter(param);
            var list = await repositoryBase
                            .Where(expression)
                            .ToListAsync();
            return list;
        }

        public async Task<List<PersonEntity>> GetPageList(UserListParam param, Pagination pagination)
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
        public async Task<List<PersonEntity>> GetPageLoginList(string UserName, Pagination pagination, long OID)
        {
            var expression = ListFilterLogin(UserName, OID);
            var Queryable = repositoryBase
                            .Where(expression)
                            .OrderBy(pagination.Sort + " " + pagination.SortType + " ");
            pagination.TotalCount = (await Queryable.CountAsync()).ParseToInt();
            var list = await Queryable
                            .Page(pagination.PageIndex, pagination.PageSize)
                            .ToListAsync();
            return list;
        }
        public async Task<PersonEntity> GetEntity(long id)
        {
            return await repositoryBase.FindAsync(id);
        }

        public async Task<PersonEntity> GetStationUserEntity(long id)
        {
            return await repositoryBase.Where(ex => ex.OrganizationType == 3 && ex.ID == id).FirstAsync();
        }

        public async Task<PersonEntity> GetUser(string UserName, string PassWord)
        {
            var PassWordMD5 = SecurityHelper.MD5Encrypt(PassWord);
            return await repositoryBase.Where(ex => ex.UserName == UserName && ex.PassWordMD5 == PassWordMD5 && ex.Status != 2).FirstAsync();
        }

        public async Task<bool> ExistUserName(PersonEntity entity)
        {
            var expression = LinqExtensions.True<PersonEntity>();
            expression = expression.And(t => t.Status != 2);
            if (entity.ID.IsNullOrZero())
            {
                expression = expression.And(t => t.UserName == entity.UserName);
            }
            else
            {
                expression = expression.And(t => t.UserName == entity.UserName && t.ID != entity.ID);
            }
            return await repositoryBase.Where(expression).CountAsync() > 0 ? true : false;
        }

        #endregion

        #region 提交数据

        public async Task<int> Update(PersonEntity entity)
        {
            return await ChangeUser(entity);
        }

        public async Task<int> SetUserStatus(PersonEntity entity)
        {
            return await repositoryBase
                .UpdateDiy
                .SetSource(entity)
                .UpdateColumns(t => t.Status)
                .ExecuteAffrowsAsync();
        }

        public async Task SaveForm(PersonEntity entity)
        {
            //使用事务
            using (var uow = Sql.GetFsql().CreateUnitOfWork())
            {
                try
                {
                    if (entity.ID.IsNullOrZero())
                    {
                        entity.RegTime = DateTime.Now;
                        entity.Create();
                        entity.ID = await uow.Orm.Insert(entity).ExecuteIdentityAsync();
                    }
                    else
                    {
                        //删除所有用户所属
                        await uow.Orm.Delete<SysUserBelongEntity>().Where(t => t.UserId == entity.ID).ExecuteAffrowsAsync();

                        // 密码不进行更新，有单独的方法更新密码
                        entity.PassWordMD5 = null;
                        entity.Modify();
                        await uow.Orm.Update<PersonEntity>().SetSourceIgnore(entity, t => t == null).ExecuteAffrowsAsync();
                    }
                    
                    // 角色
                    if (!string.IsNullOrEmpty(entity.RoleIds))
                    {
                        List<SysUserBelongEntity> sysUserBelongs = new List<SysUserBelongEntity>();
                        foreach (long roleId in TextHelper.SplitToArray<long>(entity.RoleIds, ','))
                        {
                            SysUserBelongEntity sysUserBelong = new SysUserBelongEntity();
                            sysUserBelong.UserId = entity.ID;
                            sysUserBelong.BelongId = roleId;
                            sysUserBelong.BelongType = UserBelongTypeEnum.Role.ParseToInt();
                            sysUserBelong.Create();
                            sysUserBelongs.Add(sysUserBelong);
                        }
                        await uow.Orm.Insert(sysUserBelongs).ExecuteAffrowsAsync();
                    }
                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }
        public async Task SaveLoginForm(PersonEntity entity)
        {
            //使用事务
            using (var uow = Sql.GetFsql().CreateUnitOfWork())
            {
                try
                {
                    var userinfo =await Operator.Instance.Current();
                    entity.OrganizationID = userinfo.OrganizationID;
                    if (entity.ID.IsNullOrZero())
                    {
                        entity.RegTime = DateTime.Now;
                        entity.Create();
                        entity.ID = await uow.Orm.Insert(entity).ExecuteIdentityAsync();
                    }
                    else
                    {
                        //删除所有用户所属
                        await uow.Orm.Delete<SysUserBelongEntity>().Where(t => t.UserId == entity.ID).ExecuteAffrowsAsync();

                        // 密码不进行更新，有单独的方法更新密码
                        entity.PassWordMD5 = null;
                        entity.Modify();
                        await uow.Orm.Update<PersonEntity>().SetSourceIgnore(entity, t => t == null).ExecuteAffrowsAsync();
                    }

                    // 角色
                    if (!string.IsNullOrEmpty(entity.RoleIds))
                    {
                        List<SysUserBelongEntity> sysUserBelongs = new List<SysUserBelongEntity>();
                        foreach (long roleId in TextHelper.SplitToArray<long>(entity.RoleIds, ','))
                        {
                            SysUserBelongEntity sysUserBelong = new SysUserBelongEntity();
                            sysUserBelong.UserId = entity.ID;
                            sysUserBelong.BelongId = roleId;
                            sysUserBelong.BelongType = UserBelongTypeEnum.Role.ParseToInt();
                            sysUserBelong.Create();
                            sysUserBelongs.Add(sysUserBelong);
                        }
                        await uow.Orm.Insert(sysUserBelongs).ExecuteAffrowsAsync();
                    }
                    uow.Commit();
                }
                catch
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
                    //逻辑删除用户
                    long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                    await uow.Orm.Update<PersonEntity>().Set(e => e.Status == 2).Where(ex => idArr.Contains(ex.ID)).ExecuteAffrowsAsync();
                    //删除所属
                    await uow.Orm.Delete<SysUserBelongEntity>().Where(ex => idArr.Contains(ex.UserId)).ExecuteAffrowsAsync();
                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }

        public async Task ResetPassword(PersonEntity entity)
        {
            entity.Modify();
            await repositoryBase.UpdateAsync(entity);
        }

        public async Task<int> ChangeUser(PersonEntity entity)
        {
            entity.Modify();
            entity.PassWordMD5 = null;
            return await repositoryBase.UpdateNotNullAsync(entity);
        }

        #endregion

        #region 私有方法

        private Expression<Func<PersonEntity, bool>> ListFilter(UserListParam param)
        {
            Expression<Func<PersonEntity, bool>> expression = (ex => ex.Status != 2);
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.UserName))
                {
                    expression = expression.And(t => t.UserName.Contains(param.UserName) || t.Name.Contains(param.UserName));
                }
                if (!string.IsNullOrEmpty(param.UserIds))
                {
                    long[] userIdList = TextHelper.SplitToArray<long>(param.UserIds, ',');
                    expression = expression.And(t => userIdList.Contains(t.ID));
                }
                if (!string.IsNullOrEmpty(param.UserOrg))
                {
                    var OrgInfo = param.UserOrg.Split('_');
                    if (OrgInfo.Length == 2)
                    {
                        var OrgType = Convert.ToInt64(OrgInfo[0]);
                        var OrgID = Convert.ToInt64(OrgInfo[1]);
                        expression = expression.And(t => t.OrganizationType == OrgType && t.OrganizationID == OrgID);
                    }
                }
                if (param.OrganizationType != null)
                {
                    expression = expression.And(ex => ex.OrganizationType == param.OrganizationType);
                }
            }
            return expression;
        }
        private Expression<Func<PersonEntity, bool>> ListFilterLogin(string UserName,long OID)
        {
            
            Expression<Func<PersonEntity, bool>> expression = (ex => ex.Status != 2&&ex.OrganizationType==3&&ex.OrganizationID== OID);
            if (!string.IsNullOrEmpty(UserName))
            {
                expression = expression.And(t => t.Name.Contains(UserName));
            }
            return expression;
        }
        //Enum_Person_Post

        #endregion
    }
}
