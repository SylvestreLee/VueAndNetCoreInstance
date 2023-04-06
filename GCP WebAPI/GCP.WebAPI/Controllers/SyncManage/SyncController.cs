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
using GCP.Entity.RootManage;
using GCP.Business.RootManage;
using GCP.Model.Param.RootManage;
using GCP.WebAPI.Controllers;
using GCP.WebAPI.Filter;
using GCP.Business.SyncManage;
using Microsoft.AspNetCore.Http;

namespace GCP.WebAPI.Controllers.SyncManage
{
    /// <summary>
    /// 创 建：ycz
    /// 日 期：2021-08-23 13:41
    /// 描 述：控制器类
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class SyncController:BaseController
    {
        SyncBLL syncBLL = new SyncBLL();

        #region 当前Ip地址是否在授权范围内
        /// <summary>
        /// 当前Ip地址是否在授权范围内
        /// </summary>
        /// <returns>StationID</returns>
        [HttpGet]
        public async Task<ActionResult> ConnectionInfo()
        {           
            TData<long> obj = await syncBLL.ConnectionInfo();
            return Json(obj);
        }        
        #endregion





    }
}
