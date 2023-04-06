using GCP.Service.SystemManage;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using System;
using System.Threading.Tasks;
using GCP.Code;
using GCP.Cache.Factory;
using GCP.Entity.SystemManage;
using GCP.Enum;
using System.Collections.Generic;
using System.Linq;
using GCP.Model.Param.SystemManage;
using GCP.Business.Cache;
using System.Text;

namespace GCP.Business.SystemManage
{
    public class UserBLL
    {
        /*
         * 
         * 逻辑BUG提示
         * 
         * 改密码：
         * 需要判断用户传的ID是否是自己的ID
         * 更改密码的用户需要强制下线
         * 
         * 改个人信息
         * 需要判断用户传的ID是否是自己的ID
         * 
         */

        private readonly UserService userService = new UserService();
        private readonly UserBelongService userBelongService = new UserBelongService();
        private readonly CommonBLL commonBLL = new CommonBLL();

        #region 获取数据

        public async Task<TData<List<PersonEntity>>> GetList(UserListParam param)
        {
            TData<List<PersonEntity>> obj = new TData<List<PersonEntity>>();
            obj.Data = await userService.GetList(param);
            await GetUserBelong(obj.Data);
            obj.Data = await SetJoinTable(obj.Data);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<PersonEntity>>> GetPageList(UserListParam param, Pagination pagination)
        {
            TData<List<PersonEntity>> obj = new TData<List<PersonEntity>>();
            obj.Data = await userService.GetPageList(param, pagination);
            await GetUserBelong(obj.Data);
            obj.Data = await SetJoinTable(obj.Data);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }
        public async Task<TData<List<PersonEntity>>> GetPageLoginList(string UserName,Pagination pagination,long OID)
        {
            TData<List<PersonEntity>> obj = new TData<List<PersonEntity>>();
            obj.Data = await userService.GetPageLoginList(UserName, pagination, OID);
            await GetUserBelong(obj.Data);
            obj.Data = await SetJoinTable(obj.Data);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }
        public async Task<TData<PersonEntity>> GetEntity(long id)
        {
            TData<PersonEntity> obj = new TData<PersonEntity>();
            obj.Data = await userService.GetEntity(id);
            await GetUserBelong(obj.Data);
            obj.Data = await SetJoinTable(obj.Data);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<PersonEntity>> GetStationUserEntity(long id)
        {
            TData<PersonEntity> obj = new TData<PersonEntity>();
            obj.Data = await userService.GetStationUserEntity(id);
            if (obj.Data != null)
            {
                await GetUserBelong(obj.Data);
                obj.Data = await SetJoinTable(obj.Data);
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<OperatorInfo>> CheckLogin(string UserName, string Password)
        {
            TData<OperatorInfo> obj = new TData<OperatorInfo>();

            if (UserName.IsEmpty() || Password.IsEmpty())
            {
                obj.Message = "用户名或密码不能为空";
            }
            else
            {
                var Person = await userService.GetUser(UserName, Password);
                if (Person != null)
                {
                    if (Person.Status != 1)
                    {
                        obj.Message = "用户已经被禁用！";
                    }
                    else
                    {
                        //Login Success
                        var LastToken = Person.Token;
                        if (!LastToken.IsEmpty())
                        {
                            CacheFactory.Cache.RemoveCache("Token_" + LastToken);
                        }
                        Person.Token = SecurityHelper.GetGuid();
                        Person.LastLoginTime = DateTime.Now;
                        await userService.Update(Person);

                        OperatorInfo operatorInfo = new OperatorInfo();
                        operatorInfo.UserId = Person.ID;
                        operatorInfo.UserStatus = Person.Status;
                        operatorInfo.RealName = Person.Name;
                        operatorInfo.UserName = Person.UserName;
                        operatorInfo.Token = Person.Token;
                        operatorInfo.IsSystem = Person.IsSystem ?? false;
                        operatorInfo.OrganizationType = Person.OrganizationType;
                        operatorInfo.OrganizationID = Person.OrganizationID;
                        operatorInfo.LastLoginTime = Person.LastLoginTime.Value;

                        await GetUserBelong(Person);
                        operatorInfo.RoleIds = Person.RoleIds;

                        CacheFactory.Cache.SetCache("Token_" + Person.Token, operatorInfo, DateTime.Now.AddDays(GlobalContext.SystemConfig.UserOnlineLimit));
                        obj.Data = operatorInfo;
                        obj.Message = "登录成功";
                        obj.Tag = 1;

                        Operator.Instance.AddCurrent(Person.Token);
                    }
                    Action taskAction = async () =>
                    {
                        string ip = NetHelper.Ip;
                        string browser = NetHelper.Browser;
                        string os = NetHelper.GetOSVersion();
                        string userAgent = NetHelper.UserAgent;
                        SysLogLoginEntity logLoginEntity = new SysLogLoginEntity
                        {
                            LogStatus = Person.Status == 1 ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt(),
                            Remark = "",
                            IpAddress = ip,
                            //IpLocation = IpLocationHelper.GetIpLocation(ip),
                            IpLocation = "",
                            Browser = browser,
                            OS = os,
                            ExtraRemark = userAgent,
                            BaseCreatorId = Person.ID
                        };

                        // 让底层不用获取HttpContext
                        logLoginEntity.BaseCreatorId = logLoginEntity.BaseCreatorId ?? 0;

                        await new LogLoginBLL().SaveForm(logLoginEntity);
                    };
                    AsyncTaskHelper.StartTask(taskAction);
                }
                else
                {
                    obj.Message = "用户名或密码不正确！";
                }
            }
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(PersonEntity entity)
        {
            TData<string> obj = new TData<string>();
            var ExistUserName = await userService.ExistUserName(entity);
            if (ExistUserName)
            {
                obj.Message = "用户名已经存在！";
                return obj;
            }
            if (entity.ID.IsNullOrZero())
            {
                entity.PassWordMD5 = SecurityHelper.MD5Encrypt(entity.PassWord);
            }
            //if (entity.PostList != null && entity.PostList.Count > 0)
            //{
            //    long value = 0;
            //    foreach (var p in entity.PostList)
            //    {
            //        value += p;
            //    }
            //    entity.Post = value;
            //}
            if (!string.IsNullOrEmpty(entity.PostInfo))
            {
                var post = TextHelper.SplitToArray<long>(entity.PostInfo, ',');
                long value = 0;
                foreach (var p in post)
                {
                    value += p;
                }
                entity.Post = value;
            }
            if (entity.Sex != "男" && entity.Sex != "女")
            {
                entity.Sex = "";
            }
            var org = entity.OrgInfo.Split('_');
            if (org.Length == 2)
            {
                entity.OrganizationType = org[0].ParseToLong();
                entity.OrganizationID = org[1].ParseToLong();
            }
            else
            {
                entity.OrganizationType = null;
                entity.OrganizationID = null;
            }
            await userService.SaveForm(entity);

            await RemoveCacheById(entity.ID);

            obj.Data = entity.ID.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<string>> SaveStationUserForm(PersonEntity entity)
        {
            TData<string> obj = new TData<string>();
            var ExistUserName = await userService.ExistUserName(entity);
            if (ExistUserName)
            {
                obj.Message = "用户名已经存在！";
                return obj;
            }
            if (entity.ID.IsNullOrZero())
            {
                entity.PassWordMD5 = SecurityHelper.MD5Encrypt(entity.PassWord);
            }
            if (!string.IsNullOrEmpty(entity.PostInfo))
            {
                var post = TextHelper.SplitToArray<long>(entity.PostInfo, ',');
                long value = 0;
                foreach (var p in post)
                {
                    value += p;
                }
                entity.Post = value;
            }
            if (entity.Sex != "男" && entity.Sex != "女")
            {
                entity.Sex = "";
            }
            entity.OrganizationType = 3;
            await userService.SaveForm(entity);

            await RemoveCacheById(entity.ID);

            obj.Data = entity.ID.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> SetUserStatus(long id, long status)
        {
            TData<int> obj = new TData<int>();

            var user = await userService.GetEntity(id);
            if (user != null)
            {
                //防止恶意用户传入其他数值
                var statuses = await EnumCache.Table("Enum_Status").GetList();
                if (statuses.Select(t => t.ID).ToList().Contains(status.ToString()))
                {
                    //如果枚举表存在这个数值，就赋值
                    user.Status = status;
                }
                obj.Data = await userService.SetUserStatus(user);
                obj.Tag = 1;
            }
            else 
            {
                obj.Message = "用户不存在";
            }

            return obj;
        }

        public async Task<TData<int>> SetStationUserStatus(long id, long status)
        {
            TData<int> obj = new TData<int>();

            var user = await userService.GetStationUserEntity(id);
            if (user != null)
            {
                //防止恶意用户传入其他数值
                var statuses = await EnumCache.Table("Enum_Status").GetList();
                if (statuses.Select(t => t.ID).ToList().Contains(status.ToString()))
                {
                    //如果枚举表存在这个数值，就赋值
                    user.Status = status;
                }
                obj.Data = await userService.SetUserStatus(user);
                obj.Tag = 1;
            }
            else
            {
                obj.Message = "用户不存在";
            }

            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            if (string.IsNullOrEmpty(ids))
            {
                obj.Message = "参数不能为空";
                return obj;
            }
            await userService.DeleteForm(ids);

            await RemoveCacheById(ids);

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteStationUserForm(string ids)
        {
            TData obj = new TData();
            if (string.IsNullOrEmpty(ids))
            {
                obj.Message = "参数不能为空";
                return obj;
            }
            var list = await userService.GetList(new UserListParam { UserIds = ids, OrganizationType = 3 });
            var realIds = string.Join(',', list.Select(x => x.ID).ToList());
            await userService.DeleteForm(realIds);

            await RemoveCacheById(realIds);

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<long>> ResetPassword(PersonEntity entity)
        {
            TData<long> obj = new TData<long>();
            if (entity.ID > 0)
            {
                PersonEntity dbUserEntity = await userService.GetEntity(entity.ID);
                var PassWordMD5 = SecurityHelper.MD5Encrypt(entity.PassWord);
                if (dbUserEntity.PassWordMD5 == PassWordMD5)
                {
                    obj.Message = "密码未更改";
                    return obj;
                }
                dbUserEntity.PassWordMD5 = PassWordMD5;
                await userService.ResetPassword(dbUserEntity);

                await RemoveCacheById(entity.ID);

                obj.Data = entity.ID;
                obj.Tag = 1;
                
                //服务器端强制下线
                CacheFactory.Cache.RemoveCache("Token_" + dbUserEntity.Token);
            }
            return obj;
        }

        public async Task<TData<long>> ChangePassword(ChangePasswordParam param)
        {
            TData<long> obj = new TData<long>();
            if (param.Id > 0)
            {
                //获取用户
                OperatorInfo operatorInfo = await Operator.Instance.Current();
                if (operatorInfo.UserId != param.Id)
                {
                    obj.Message = "只能修改自己的密码";
                    return obj;
                }
                if (string.IsNullOrEmpty(param.Password) || string.IsNullOrEmpty(param.NewPassword))
                {
                    obj.Message = "新密码不能为空";
                    return obj;
                }
                PersonEntity dbUserEntity = await userService.GetEntity(param.Id.Value);
                if (dbUserEntity.PassWordMD5 != SecurityHelper.MD5Encrypt(param.Password))
                {
                    obj.Message = "旧密码不正确";
                    return obj;
                }
                dbUserEntity.PassWordMD5 = SecurityHelper.MD5Encrypt(param.NewPassword);
                await userService.ResetPassword(dbUserEntity);

                await RemoveCacheById(param.Id.Value);

                //服务器端强制下线
                CacheFactory.Cache.RemoveCache("Token_" + operatorInfo.Token);

                obj.Data = dbUserEntity.ID;
                obj.Tag = 1;
            }
            return obj;
        }

        /// <summary>
        /// 用户自己修改自己的信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TData<long>> ChangeUser(PersonEntity entity)
        {
            TData<long> obj = new TData<long>();
            //获取用户
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            if (operatorInfo.UserId != entity.ID)
            {
                obj.Message = "只能修改自己的信息";
                return obj;
            }
            if (entity.ID > 0)
            {
                await userService.ChangeUser(entity);

                await RemoveCacheById(entity.ID);

                obj.Data = entity.ID;
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData> UpdateUser(PersonEntity entity)
        {
            TData obj = new TData();
            await userService.Update(entity);

            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 私有方法

        public async Task<PersonEntity> SetJoinTable(PersonEntity entity)
        {
            entity = await commonBLL.GetBinaryValue(entity, "Enum_Person_Post", t => t.Post, t => t.PostName, t => t.PostList, "HexValue", "Name");
            entity = await commonBLL.SetJoinTable(entity, t => t.OrganizationType, t => t.OrganizationTable, "Enum_Person_OrganizationType", "" ,"ID", "TableName");
            entity = await commonBLL.SetJoinTable(entity, t => t.OrganizationID, t => t.OrganizationName, entity.OrganizationTable);
            entity = await commonBLL.SetJoinTable(entity, t => t.OrganizationType, t => t.OrganizationTypeName, "Enum_Person_OrganizationType");
            entity = await commonBLL.SetJoinTable(entity, t => t.Status, t => t.StatusName, "Enum_Status");
            return entity;
        }

        public async Task<List<PersonEntity>> SetJoinTable(List<PersonEntity> entities)
        {
            entities = await commonBLL.GetBinaryValue(entities, "Enum_Person_Post", t => t.Post, t => t.PostName, t => t.PostList, "HexValue", "Name");
            entities = await commonBLL.SetJoinTable(entities, t => t.OrganizationType, t => t.OrganizationTable, "Enum_Person_OrganizationType", "", "ID", "TableName");
            entities = await commonBLL.SetJoinTable(entities, t => t.OrganizationID, t => t.OrganizationName, t => t.OrganizationTable);
            entities = await commonBLL.SetJoinTable(entities, t => t.OrganizationType, t => t.OrganizationTypeName, "Enum_Person_OrganizationType");
            entities = await commonBLL.SetJoinTable(entities, t => t.Status, t => t.StatusName, "Enum_Status");
            return entities;
        }

        /// <summary>
        /// 移除缓存里面的token
        /// </summary>
        /// <param name="id"></param>
        private async Task RemoveCacheById(string ids)
        {
            foreach (long id in ids.Split(',').Select(p => long.Parse(p)))
            {
                await RemoveCacheById(id);
            }
        }

        private async Task RemoveCacheById(long id)
        {
            var dbEntity = await userService.GetEntity(id);
            if (dbEntity != null)
            {
                try
                {
                    CacheFactory.Cache.RemoveCache(dbEntity.Token);
                }
                catch (Exception err)
                { }
            }
        }

        private async Task GetUserBelong(PersonEntity user)
        {
            List<SysUserBelongEntity> userBelongList = await userBelongService.GetList(new SysUserBelongEntity { UserId = user.ID });

            List<SysUserBelongEntity> roleBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Role.ParseToInt()).ToList();
            if (roleBelongList.Count > 0)
            {
                user.RoleIds = string.Join(",", roleBelongList.Select(p => p.BelongId).ToList());
            }
        }
        private async Task GetUserBelong(List<PersonEntity> users)
        {
            List<SysUserBelongEntity> userBelongList = await userBelongService.GetList(new SysUserBelongEntity { UserIds = users.Select(t => t.ID).ToList() });

            List<SysUserBelongEntity> roleBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Role.ParseToInt()).ToList();
            if (roleBelongList.Count > 0)
            {
                foreach (var u in users)
                { 
                    u.RoleIds = string.Join(",", roleBelongList.Where(p => p.UserId == u.ID).Select(p => p.BelongId).ToList());
                }
            }
        }


        //private async Task<List<PersonEntity>> GetPost(List<PersonEntity> entities)
        //{
        //    var list = await EnumCache.Table("Enum_Person_Post").GetList();
        //    foreach (var entity in entities)
        //    {
        //        try
        //        {
        //            var post = Convert.ToInt64(entity.Post);
        //            List<string> names = new List<string>();
        //            List<long> ids = new List<long>();
        //            foreach (var ev in list)
        //            {
        //                var v = Convert.ToInt64(ev.ID);
        //                if ((post & v) == v)
        //                {
        //                    ids.Add(v);
        //                    names.Add(ev.Name);
        //                }
        //            }
        //            entity.PostName = string.Join(",", names);
        //            entity.PostList = ids;
        //        }
        //        catch (Exception err)
        //        { }
        //    }
        //    return entities;
        //}

        //private async Task<PersonEntity> GetPost(PersonEntity entity)
        //{
        //    var ret = await GetPost(new List<PersonEntity>() { entity });
        //    return ret[0];
        //}

        #endregion
    }
}
