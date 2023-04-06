using GCP.Cache.Factory;
using GCP.Entity.SystemManage;
using GCP.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GCP.Business.Cache
{
    //public class RoleCache : BaseBusinessCache<SysRoleEntity>
    //{
    //    private RoleService roleService = new RoleService();

    //    public override string CacheKey => this.GetType().Name;

    //    public override async Task<List<SysRoleEntity>> GetList()
    //    {
    //        var cacheList = CacheFactory.Cache.GetCache<List<SysRoleEntity>>(CacheKey);
    //        if (cacheList == null)
    //        {
    //            var list = await roleService.GetList(null);
    //            CacheFactory.Cache.SetCache(CacheKey, list);
    //            return list;
    //        }
    //        else
    //        {
    //            return cacheList;
    //        }
    //    }
    //}
}
