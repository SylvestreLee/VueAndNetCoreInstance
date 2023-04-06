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
using GCP.Enum.SystemManage;
using static GCP.Model.CommonModel;
using GCP.Service;

namespace GCP.Business.SystemManage
{
    public class RoleBLL
    {
        private RoleService roleService = new RoleService();
        private MenuAuthorizeService menuAuthorizeService = new MenuAuthorizeService();
        private MenuService menuService = new MenuService();
        private CommonService commonService = new CommonService();

        private MenuAuthorizeCache menuAuthorizeCache = new MenuAuthorizeCache();

        #region 获取数据
        public async Task<TData<List<EnumIdNameModel>>> GetEnum()
        {
            TData<List<EnumIdNameModel>> obj = new TData<List<EnumIdNameModel>>();

            obj.Data = await commonService.GetEnumList("SysRole", "status=1 ", "ID", "RoleName");
            obj.Tag = 1;
            obj.Total = 1;

            return obj;
        }

        public async Task<TData<List<EnumIdNameModel>>> GetLoginEnum()
        {
            TData<List<EnumIdNameModel>> obj = new TData<List<EnumIdNameModel>>();

            obj.Data = await commonService.GetEnumList("SysRole", "status=1 and platformtype=2", "ID", "RoleName");
            obj.Tag = 1;
            obj.Total = 1;

            return obj;
        }

        public async Task<TData<List<SysRoleEntity>>> GetList(RoleListParam param)
        {
            TData<List<SysRoleEntity>> obj = new TData<List<SysRoleEntity>>();
            obj.Data = await roleService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }


        public async Task<TData<List<SysRoleEntity>>> GetLoginList(RoleListParam param)
        {
            TData<List<SysRoleEntity>> obj = new TData<List<SysRoleEntity>>();
            obj.Data = await roleService.GetLoginList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<SysRoleEntity>>> GetPageList(RoleListParam param, Pagination pagination)
        {
            TData<List<SysRoleEntity>> obj = new TData<List<SysRoleEntity>>();
            obj.Data = await roleService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<SysRoleEntity>>> GetLoginPageList(RoleListParam param, Pagination pagination)
        {
            TData<List<SysRoleEntity>> obj = new TData<List<SysRoleEntity>>();
            obj.Data = await roleService.GetLoginPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<SysRoleEntity>> GetEntity(long id)
        {
            TData<SysRoleEntity> obj = new TData<SysRoleEntity>();
            SysRoleEntity roleEntity = await roleService.GetEntity(id);
            List<SysMenuAuthorizeEntity> menuAuthorizeList = await menuAuthorizeService.GetList(new SysMenuAuthorizeEntity
            {
                AuthorizeId = id,
                AuthorizeType = AuthorizeTypeEnum.Role.ParseToInt()
            });
            //只返回按钮
            var menuList = await menuService.GetList(null);
            var menuIdList = menuList.Where(ex => ex.MenuType == 3).Select(t => t.ID).Distinct().ToList();
            menuAuthorizeList = menuAuthorizeList.Where(ex => menuIdList.Contains(ex.MenuId.Value)).ToList();
            // 获取角色对应的权限
            roleEntity.MenuIds = string.Join(",", menuAuthorizeList.Select(p => p.MenuId));

            obj.Data = roleEntity;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<long>> GetMaxSort()
        {
            TData<long> obj = new TData<long>();
            obj.Data = await roleService.GetMaxSort();
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(SysRoleEntity entity)
        {
            TData<string> obj = new TData<string>();
            entity.PlatformType = 1;
            var ExistRoleName = await roleService.ExistRoleName(entity);
            if (ExistRoleName)
            {
                obj.Message = "角色名称已经存在！";
                return obj;
            }

            await roleService.SaveForm(entity);

            // 清除缓存里面的权限数据
            menuAuthorizeCache.Remove();

            obj.Data = entity.ID.ParseToString();
            obj.Tag = 1;

            return obj;
        }
        public async Task<TData<string>> SaveLoginForm(SysRoleEntity entity)
        {
            TData<string> obj = new TData<string>();
            entity.PlatformType = 2;
            var ExistRoleName = await roleService.ExistRoleName(entity);
            if (ExistRoleName)
            {
                obj.Message = "角色名称已经存在！";
                return obj;
            }

            await roleService.SaveForm(entity);

            // 清除缓存里面的权限数据
            menuAuthorizeCache.Remove();

            obj.Data = entity.ID.ParseToString();
            obj.Tag = 1;

            return obj;
        }


        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();

            await roleService.DeleteForm(ids);

            // 清除缓存里面的权限数据
            menuAuthorizeCache.Remove();

            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}
