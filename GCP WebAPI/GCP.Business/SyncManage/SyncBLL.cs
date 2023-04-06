using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using FreeSql.DatabaseModel;
using GCP.Business;
using GCP.Service.SyncManage;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using GCP.Entity.RootManage;
using GCP.Repository;

namespace GCP.Business.SyncManage
{
    public class SyncBLL
    {
        SyncService syncService = new SyncService();
        private IFreeSql sql = Sql.GetFsql();
        #region 当前Ip地址是否在授权范围内
        public async Task<TData<long>> ConnectionInfo()
        {
            TData<long> obj = new TData<long>();
            try
            {
                HttpContextAccessor context = new HttpContextAccessor();
                var ClientIP = context.HttpContext?.Connection.RemoteIpAddress.ToString();
                if (ClientIP == "::1")
                    ClientIP = "127.0.0.1";
                Expression<Func<StationEntity, bool>> expression = ((ex) => ex.Status == 1 && ex.IP == ClientIP);
                var list = await sql.Select<StationEntity>()
                    .Where(expression)
                    .FirstAsync();
                if (list == null)
                {
                    obj.Message = "当前IP：" + ClientIP + ".未经授权的IP地址.请求无效.本服务仅对特定的检测站开放,请勿随意连接使用.";
                }
                else
                {
                    obj.Value = list.ID.ToString();
                }
                obj.Tag = 1;
            }
            catch(Exception EX)
            {
                obj.Tag = 0;
                obj.Message = EX.Message;
            }
            return obj;
        }
        #endregion


        #region  获取需要生成的表信息
        //public async Task<TData<string>> GetDownTableInfo()
        //{ 




        //}

        #endregion 
    }
}
