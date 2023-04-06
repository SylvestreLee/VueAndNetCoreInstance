using GCP.Business.Cache;
using GCP.Code;
using GCP.Entity.SystemManage;
using GCP.Enum;
using GCP.Enum.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.Model.Result;
using GCP.Service.SystemManage;
using GCP.Util.Extension;
using GCP.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCP.Business.SystemManage
{
    public class MenuBLL
    {
        private MenuService menuService = new MenuService();

        private MenuCache menuCache = new MenuCache();
        private MenuAuthorizeCache menuAuthorizeCache = new MenuAuthorizeCache();

        private RoleService roleService = new RoleService();

        #region 获取数据

        public async Task<TData<List<SysMenuEntity>>> GetMenuList()
        {
            TData<List<SysMenuEntity>> obj = new TData<List<SysMenuEntity>>();

            //获取用户
            OperatorInfo operatorInfo = await Operator.Instance.Current();

            //获取菜单
            List<SysMenuEntity> list = await menuCache.GetList();
            List<SysMenuEntity> menuList = list;
            menuList = menuList.Where(p => p.Status == Status.正常.ParseToInt()).ToList();

            //判断权限
            if (!operatorInfo.IsSystem)
            {
                TData<List<MenuAuthorizeInfo>> objMenuAuthorize = await GetAuthorizeList(operatorInfo);
                List<long?> authorizeMenuIdList = objMenuAuthorize.Data.Select(p => p.MenuId).ToList();
                menuList = menuList.Where(p => authorizeMenuIdList.Contains(p.ID)).ToList();
            }

            obj.Tag = 1;
            obj.Data = menuList;
            obj.Total = obj.Data.Count;

            return obj;
        }

        public async Task<TData<List<MenuAuthorizeInfo>>> GetAuthorizeList(OperatorInfo user)
        {
            TData<List<MenuAuthorizeInfo>> obj = new TData<List<MenuAuthorizeInfo>>();
            obj.Data = new List<MenuAuthorizeInfo>();

            List<SysMenuAuthorizeEntity> authorizeList = new List<SysMenuAuthorizeEntity>();
            List<SysMenuAuthorizeEntity> userAuthorizeList = null;
            List<SysMenuAuthorizeEntity> roleAuthorizeList = null;

            var menuAuthorizeCacheList = await menuAuthorizeCache.GetList();
            var menuList = await menuCache.GetList();
            var enableMenuIdList = menuList.Where(p => p.Status == Status.正常.ParseToInt()).Select(p => p.ID).ToList();

            menuAuthorizeCacheList = menuAuthorizeCacheList.Where(p => enableMenuIdList.Contains(p.MenuId.ParseToLong())).ToList();

            // 用户
            userAuthorizeList = menuAuthorizeCacheList.Where(p => p.AuthorizeId == user.UserId && p.AuthorizeType == AuthorizeTypeEnum.User.ParseToInt()).ToList();

            // 角色
            if (!string.IsNullOrEmpty(user.RoleIds))
            {
                List<long> roleIdList = user.RoleIds.Split(',').Select(p => long.Parse(p)).ToList();
                roleAuthorizeList = menuAuthorizeCacheList.Where(p => roleIdList.Contains(p.AuthorizeId.Value) && p.AuthorizeType == AuthorizeTypeEnum.Role.ParseToInt()).ToList();
            }

            // 排除重复的记录
            if (userAuthorizeList.Count > 0)
            {
                authorizeList.AddRange(userAuthorizeList);
                roleAuthorizeList = roleAuthorizeList.Where(p => !userAuthorizeList.Select(u => u.AuthorizeId).Contains(p.AuthorizeId)).ToList();
            }
            if (roleAuthorizeList != null && roleAuthorizeList.Count > 0)
            {
                authorizeList.AddRange(roleAuthorizeList);
            }

            foreach (SysMenuAuthorizeEntity authorize in authorizeList)
            {
                obj.Data.Add(new MenuAuthorizeInfo
                {
                    MenuId = authorize.MenuId,
                    AuthorizeId = authorize.AuthorizeId,
                    AuthorizeType = authorize.AuthorizeType,
                    Authorize = menuList.Where(t => t.ID == authorize.MenuId).Select(t => t.Authorize).FirstOrDefault()
                });
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<UserRoleInfo>> GetInfo()
        {
            TData<UserRoleInfo> obj = new TData<UserRoleInfo>();
            obj.Data = new UserRoleInfo();

            //定义变量
            List<SysMenuAuthorizeEntity> authorizeList = new List<SysMenuAuthorizeEntity>();
            List<SysMenuAuthorizeEntity> userAuthorizeList = null;
            List<SysMenuAuthorizeEntity> roleAuthorizeList = null;

            //获取用户
            OperatorInfo operatorInfo = await Operator.Instance.Current();

            //角色相关
            var Roles = await roleService.GetList(new Model.Param.SystemManage.RoleListParam { RoleIds = operatorInfo.RoleIds ?? "-1" });
            var RoleNames = Roles.Select(r => r.RoleName).ToList();

            //权限相关
            var menuAuthorizeCacheList = await menuAuthorizeCache.GetList();
            var menuList = await menuCache.GetList();
            var enableMenuIdList = menuList.Where(p => p.Status == Status.正常.ParseToInt()).Select(p => p.ID).ToList();

            menuAuthorizeCacheList = menuAuthorizeCacheList.Where(p => enableMenuIdList.Contains(p.MenuId.ParseToLong())).Where(p => p.Status == 1).ToList();

            // 用户
            userAuthorizeList = menuAuthorizeCacheList.Where(p => p.AuthorizeId == operatorInfo.UserId && p.AuthorizeType == AuthorizeTypeEnum.User.ParseToInt()).ToList();

            // 角色
            if (!string.IsNullOrEmpty(operatorInfo.RoleIds))
            {
                List<long> roleIdList = operatorInfo.RoleIds.Split(',').Select(p => long.Parse(p)).ToList();
                roleAuthorizeList = menuAuthorizeCacheList.Where(p => roleIdList.Contains(p.AuthorizeId.Value) && p.AuthorizeType == AuthorizeTypeEnum.Role.ParseToInt()).ToList();
            }

            // 排除重复的记录
            if (userAuthorizeList.Count > 0)
            {
                authorizeList.AddRange(userAuthorizeList);
                roleAuthorizeList = roleAuthorizeList.Where(p => !userAuthorizeList.Select(u => u.AuthorizeId).Contains(p.AuthorizeId)).ToList();
            }
            if (roleAuthorizeList != null && roleAuthorizeList.Count > 0)
            {
                authorizeList.AddRange(roleAuthorizeList);
            }

            if (operatorInfo.IsSystem)
            {
                obj.Data.Permissions.Add("*:*:*");
            }
            else
            {
                foreach (SysMenuAuthorizeEntity authorize in authorizeList)
                {
                    var p = menuList.Where(t => t.ID == authorize.MenuId && t.MenuType == (int)MenuTypeEnum.Button).Select(t => t.Authorize).FirstOrDefault();
                    if (p != null)
                    {
                        obj.Data.Permissions.Add(p);
                    }
                }
                //去重
                obj.Data.Permissions = obj.Data.Permissions.Distinct().ToList();
            }

            obj.Data.Roles = RoleNames;
            obj.Data.Rolelist = Roles;
            obj.Tag = 1;

            return obj;
        }

        public async Task<TData<List<UserMenuInfo>>> GetRouters()
        {
            TData<List<UserMenuInfo>> obj = new TData<List<UserMenuInfo>>();
            obj.Data = new List<UserMenuInfo>();

            //定义变量
            List<SysMenuAuthorizeEntity> authorizeList = new List<SysMenuAuthorizeEntity>();
            List<SysMenuAuthorizeEntity> userAuthorizeList = null;
            List<SysMenuAuthorizeEntity> roleAuthorizeList = null;

            //获取用户
            OperatorInfo operatorInfo = await Operator.Instance.Current();

            var menuAuthorizeCacheList = await menuAuthorizeCache.GetList();
            var menuList = await menuCache.GetList();
            var enableMenuIdList = menuList.Where(p => p.Status == Status.正常.ParseToInt()).Select(p => p.ID).ToList();

            menuAuthorizeCacheList = menuAuthorizeCacheList.Where(p => enableMenuIdList.Contains(p.MenuId.ParseToLong())).ToList();

            // 用户
            userAuthorizeList = menuAuthorizeCacheList.Where(p => p.AuthorizeId == operatorInfo.UserId && p.AuthorizeType == AuthorizeTypeEnum.User.ParseToInt()).ToList();

            // 角色
            if (!string.IsNullOrEmpty(operatorInfo.RoleIds))
            {
                List<long> roleIdList = operatorInfo.RoleIds.Split(',').Select(p => long.Parse(p)).ToList();
                roleAuthorizeList = menuAuthorizeCacheList.Where(p => roleIdList.Contains(p.AuthorizeId.Value) && p.AuthorizeType == AuthorizeTypeEnum.Role.ParseToInt()).ToList();
            }

            // 排除重复的记录
            if (userAuthorizeList.Count > 0)
            {
                authorizeList.AddRange(userAuthorizeList);
                roleAuthorizeList = roleAuthorizeList.Where(p => !userAuthorizeList.Select(u => u.AuthorizeId).Contains(p.AuthorizeId)).ToList();
            }
            if (roleAuthorizeList != null && roleAuthorizeList.Count > 0)
            {
                authorizeList.AddRange(roleAuthorizeList);
            }

            //目录
            Dictionary<SysMenuEntity, List<SysMenuEntity>> menu = new Dictionary<SysMenuEntity, List<SysMenuEntity>>();
            List<SysMenuEntity> DirectoryMenuList = new List<SysMenuEntity>();
            //组合格式
            if (operatorInfo.IsSystem)
            {
                DirectoryMenuList = menuList.Where(t => t.MenuType == (int)MenuTypeEnum.Directory).ToList();
            }
            else
            {
                DirectoryMenuList = menuList.Where(t => t.MenuType == (int)MenuTypeEnum.Directory && authorizeList.Select(a => a.MenuId).ToList().Contains(t.ID)).ToList();
            }
            foreach (var dm in DirectoryMenuList)
            {
                if (!menu.ContainsKey(dm))
                {
                    menu.Add(dm, new List<SysMenuEntity>());
                }
            }
            //根据目录找菜单
            List<SysMenuEntity> MenuMenuList = new List<SysMenuEntity>();
            if (operatorInfo.IsSystem)
            {
                MenuMenuList = menuList.Where(t => t.MenuType == (int)MenuTypeEnum.Menu).ToList();
            }
            else
            {
                MenuMenuList = menuList.Where(t => t.MenuType == (int)MenuTypeEnum.Menu && authorizeList.Select(a => a.MenuId).ToList().Contains(t.ID)).ToList();
            }
            foreach (var mm in MenuMenuList)
            {
                var ParentMenu = menuList.Where(t => t.ID == mm.ParentId).FirstOrDefault();
                if (ParentMenu != null && menu.ContainsKey(ParentMenu))
                {
                    menu[ParentMenu].Add(mm);
                }
            }
            //组合
            foreach (var pm in menu)
            {
                if (pm.Value.Count > 0)
                {
                    var ParentMenu = pm.Key;
                    UserMenuInfo userMenuInfo = new UserMenuInfo();
                    userMenuInfo.Name = ParentMenu.MenuTarget.FirstUpper() ?? "";
                    userMenuInfo.Path = "/" + (ParentMenu.MenuTarget ?? "").ToLower();
                    userMenuInfo.Meta.Title = ParentMenu.MenuName ?? "";
                    userMenuInfo.Meta.Icon = ParentMenu.MenuIcon ?? "";
                    foreach (var m in pm.Value)
                    {
                        PageInfo pageInfo = new PageInfo();
                        pageInfo.Name = m.MenuTarget.FirstUpper() ?? "";
                        pageInfo.Path = "/" + (m.MenuTarget ?? "").ToLower();
                        pageInfo.Component = m.MenuUrl ?? "";
                        pageInfo.Meta.Title = m.MenuName ?? "";
                        pageInfo.Meta.Icon = m.MenuIcon ?? "";
                        userMenuInfo.Children.Add(pageInfo);
                    }
                    obj.Data.Add(userMenuInfo);
                }
            }

            obj.Tag = 1;

            return obj;
        }

        public async Task<TData<List<ElementTreeInfo>>> GetEtreeList()
        {
            var obj = new TData<List<ElementTreeInfo>>();
            List<ZtreeInfo> ztreeInfos  = new List<ZtreeInfo>();

            List<SysMenuEntity> list = await menuCache.GetList();
            //list = ListFilter(param, list);

            foreach (SysMenuEntity menu in list)
            {
                ztreeInfos.Add(new ZtreeInfo
                {
                    id = menu.ID.ToString(),
                    pId = menu.ParentId.ToString(),
                    name = menu.MenuName
                });
            }
            obj.Data = CommonBLL.GetMenuElementTreeInfo(ztreeInfos, list);

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<SysMenuEntity>> GetEntity(long id)
        {
            TData<SysMenuEntity> obj = new TData<SysMenuEntity>();
            obj.Data = await menuService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(SysMenuEntity entity)
        {
            TData<string> obj = new TData<string>();
            entity.PlatformType = 1;
            await menuService.SaveForm(entity);
            menuCache.Remove();
            obj.Data = entity.ID.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await menuService.DeleteForm(ids);
            menuCache.Remove();
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 私有方法
        private List<SysMenuEntity> ListFilter(MenuListParam param, List<SysMenuEntity> list)
        {
            if (param != null)
            {
                
            }
            return list;
        }
        #endregion
    }
}
