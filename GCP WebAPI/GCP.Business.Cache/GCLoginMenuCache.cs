using GCP.Cache.Factory;
using GCP.Entity.SystemManage;
using GCP.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GCP.Business.Cache
{
    public class GCLoginMenuCache : BaseBusinessCache<SysMenuEntity>
    {
        private MenuService menuService = new MenuService();

        public override string CacheKey => this.GetType().Name;
        /// <summary>
        /// 获取登录系统的菜单列表
        /// </summary>
        /// <returns></returns>
        public override async Task<List<SysMenuEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<SysMenuEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await menuService.GetGCLoginList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }
    }
}
