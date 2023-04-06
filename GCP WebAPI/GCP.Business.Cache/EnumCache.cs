using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GCP.Cache.Factory;
using GCP.Service;
using static GCP.Model.CommonModel;
using System.Linq;
using GCP.Util;

namespace GCP.Business.Cache
{
    public class EnumCache : CommonService
    {
        /// <summary>
        /// 枚举表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// ID字段名称，默认ID
        /// </summary>
        public string IdColumnName { get; set; }

        /// <summary>
        /// Name字段名称，默认Name
        /// </summary>
        public string NameColumnName { get; set; }

        /// <summary>
        /// Where条件
        /// </summary>
        public string Where { get; set; }

        public EnumCache()
        {
            IdColumnName = "ID";
            NameColumnName = "Name";
        }

        /// <summary>
        /// 实例化枚举缓存
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static EnumCache Table(string tableName, string id = "ID", string name = "Name", string where = "")
        {
            if (tableName.ToUpper() == "Area".ToUpper())
            {
                id = "Code";
            }
            return new EnumCache { TableName = tableName, IdColumnName = id, NameColumnName = name, Where = where };
        }

        /// <summary>
        /// 获取枚举缓存
        /// </summary>
        /// <returns></returns>
        public async Task<List<EnumIdNameModel>> GetList()
        {
            if (this.TableName != null && this.TableName != string.Empty)
            {
                var CacheName = TableName + "_" + IdColumnName + "_" + NameColumnName;
                if (Where != "")
                {
                    CacheName = CacheName + "_" + Where;
                }
                CacheName = CacheName.ToUpper();
                var cacheList = CacheFactory.Cache.GetCache<List<EnumIdNameModel>>(CacheName);
                if (cacheList == null)
                {
                    if (CanGetCache(this.TableName))
                    {
                        var list = await GetEnumList(TableName, Where, IdColumnName, NameColumnName);
                        if (TableName.ToUpper() == "Area".ToUpper())
                        {
                            //用于筛选一个地市的数据
                            var SettingAreaCode = GlobalContext.SystemConfig.AreaCode;
                            list = list.Where(a => a.ID.Substring(0, SettingAreaCode.Length) == SettingAreaCode).ToList();
                        }
                        CacheFactory.Cache.SetCache(CacheName, list);
                        return list;
                    }
                    else
                    {
                        return new List<EnumIdNameModel>();
                    }
                }
                else
                {
                    return cacheList;
                }
            }
            else
            {
                return new List<EnumIdNameModel>();
            }
        }

        /// <summary>
        /// 不是所有表都能缓存，使用此方法判断该表是否能被缓存，若能被缓存则可以使用GetList方法获取缓存的枚举表内容
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static bool CanGetCache(string TableName)
        {
            if (
                TableName.ToUpper().Contains("Enum".ToUpper()) ||
                TableName.ToUpper() == "Area".ToUpper()
                )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        public void RemoveCache()
        {
            var CacheName = TableName + "_" + IdColumnName + "_" + NameColumnName;
            CacheName = CacheName.ToUpper();
            CacheFactory.Cache.RemoveCache(CacheName);
        }
    }
}
