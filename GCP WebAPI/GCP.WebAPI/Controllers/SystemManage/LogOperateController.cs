using GCP.Business.SystemManage;
using GCP.Code;
using GCP.Util;
using GCP.Util.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using GCP.WebAPI.Controllers;
using GCP.Cache.Factory;
using GCP.Model.Result.SystemManage;
using GCP.Model.Result;
using GCP.WebAPI.Filter;
using GCP.Entity.SystemManage;
using GCP.Model.Param.SystemManage;

namespace GCP.WebAPI.Controllers.SystemManage
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LogOperateController : BaseController
    {
        private LogOperateBLL logOperateBLL = new LogOperateBLL();

        #region 获取数据

        /// <summary>
        /// 获取日志分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:log:search")]
        public async Task<TData<List<SysLogOperateEntity>>> GetPageListJson([FromQuery] LogOperateListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<SysLogOperateEntity>> obj = await logOperateBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        /// 根据id获取单条日志信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:log:search")]
        public async Task<TData<SysLogOperateEntity>> GetFormJson([FromQuery] long id)
        {
            TData<SysLogOperateEntity> obj = await logOperateBLL.GetEntity(id);
            return obj;
        }

        #endregion
    }
}
