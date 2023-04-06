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
using GCP.Entity.SystemManage;
using GCP.Business.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.WebAPI.Controllers;
using GCP.WebAPI.Filter;

namespace GCP.WebAPI.Controllers.SystemManage.Controllers
{
    /// <summary>
    /// 创 建：sht
    /// 日 期：2021-08-02 10:58
    /// 描 述：控制器类
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class LogOperateEPBController :  BaseController
    {
        private LogOperateEPBBLL logOperateEPBBLL = new LogOperateEPBBLL();

        /// <summary>
        /// 获取日志分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:logoperateepb:search")]
        public async Task<TData<List<LogOperateEPBEntity>>> GetPageListJson([FromQuery] LogOperateEPBListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<LogOperateEPBEntity>> obj = await logOperateEPBBLL.GetPageList(param, pagination);
            return obj;
        }
    }
}
