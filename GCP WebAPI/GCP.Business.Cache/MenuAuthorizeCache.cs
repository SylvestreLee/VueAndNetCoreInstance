using GCP.Cache.Factory;
using GCP.Entity.SystemManage;
using GCP.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GCP.Business.Cache
{
    public class MenuAuthorizeCache : BaseBusinessCache<SysMenuAuthorizeEntity>
    {
        private MenuAuthorizeService menuAuthorizeService = new MenuAuthorizeService();

        public override string CacheKey => this.GetType().Name;

        /// <summary>
        /// 获取平台的菜单权限
        /// </summary>
        /// <returns></returns>
        public override async Task<List<SysMenuAuthorizeEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<SysMenuAuthorizeEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await menuAuthorizeService.GetList(null);
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
