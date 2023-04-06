using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using GCP.Entity.SyncManage;
using GCP.Model.Param.SyncManage;
using GCP.Service.SyncManage;
using FreeSql.DatabaseModel;
using GCP.Business;

namespace GCP.Business.SyncManage
{
    /// <summary>
    /// 创 建：manage
    /// 日 期：2022-03-02 15:48
    /// 描 述：业务类
    /// </summary>
    public class SyncTableBLL
    {
        private CommonBLL commonBLL = new CommonBLL(); 
        private SyncTableService syncTableService = new SyncTableService();
        
        #region 获取数据
        
        public async Task<TData<List<SyncTableNameEntity>>> GetList()
        {
            TData<List<SyncTableNameEntity>> obj = new TData<List<SyncTableNameEntity>>();
            obj.Data = await syncTableService.GetList();           
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }     

      
        
        #endregion
      
        
      
    }
}
