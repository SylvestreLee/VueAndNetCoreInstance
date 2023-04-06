using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using GCP.Util;
using GCP.Util.Model;
using GCP.Entity;
using GCP.Model;
using GCP.Entity.SyncManage;
using GCP.Business.SyncManage;
using GCP.Model.Param.SyncManage;
using GCP.WebAPI.Controllers;
using GCP.WebAPI.Filter;

namespace GCP.WebAPI.Controllers.SyncManage.Controllers
{
    /// <summary>
    /// 创 建：manage
    /// 日 期：2022-03-02 15:48
    /// 描 述：控制器类
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class SyncTableController : BaseController
    {
        private SyncTableBLL syncTableBLL = new SyncTableBLL();

        #region 获取数据

        /// <summary> 
        /// 获取同步表列表 
        /// </summary>        
        /// <returns></returns> 
        [HttpGet]
        [AuthorizeFilter("sync:synctable:search")]
        public async Task<ActionResult> GetDownTableInfo()
        {
            TData<List<SyncTableNameEntity>> obj = await syncTableBLL.GetList();
            return Json(obj);
        }


        //[HttpGet]
        //[AuthorizeFilter("sync:synctable:search")]
        //public async Task<ActionResult> GetDownTableStru([FromQuery]string TableName)
        //{
        //    TData<List<SyncTableNameEntity>> obj = await syncTableBLL.GetList();
        //    return Json(obj);
        //}





        #endregion


    }
}
