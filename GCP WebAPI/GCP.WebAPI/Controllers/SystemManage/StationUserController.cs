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
    public class StationUserController : ControllerBase
    {
        private UserBLL userBLL = new UserBLL();

        #region 获取数据

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="param">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("station:stationuser:search")]
        public async Task<TData<List<PersonEntity>>> GetListJson([FromQuery] UserListParam param)
        {
            param.OrganizationType = 3;
            TData<List<PersonEntity>> obj = await userBLL.GetList(param);
            return obj;
        }

        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("station:stationuser:search")]
        public async Task<TData<List<PersonEntity>>> GetPageListJson([FromQuery] UserListParam param, [FromQuery] Pagination pagination)
        {
            param.OrganizationType = 3;
            TData<List<PersonEntity>> obj = await userBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("station:stationuser:search")]
        public async Task<TData<List<PersonEntity>>> GetPageLoginListJson([FromQuery] string? UserName, [FromQuery] Pagination pagination)
        {
            var userinfo = await Operator.Instance.Current();
            TData<List<PersonEntity>> obj = await userBLL.GetPageLoginList(UserName, pagination,long.Parse(userinfo.OrganizationID.ToString()));
            return obj;
        }


        /// <summary>
        /// 根据id获取单条用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("station:stationuser:search")]
        public async Task<TData<PersonEntity>> GetFormJson([FromQuery] long id)
        {
            TData<PersonEntity> obj = await userBLL.GetStationUserEntity(id);
            return obj;
        }

        /// <summary>
        /// 导出用户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("station:stationuser:export")]
        public async Task<TData<string>> ExportUserJson([FromQuery] UserListParam param)
        {
            TData<string> obj = new TData<string>();
            param.OrganizationType = 3;
            TData<List<PersonEntity>> userObj = await userBLL.GetList(param);
            if (userObj.Tag == 1)
            {
                string file = new ExcelHelper<PersonEntity>().ExportToExcel("用户列表.xls",
                                                                          "用户列表",
                                                                          userObj.Data,
                                                                          new string[] { "UserName", "Name", "Sex", "Tel" });
                obj.Data = file;
                obj.Tag = 1;
            }
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 提交（监管平台）数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("station:stationuser:add,station:stationuser:edit")]
        public async Task<TData<string>> SaveFormJson([FromBody] PersonEntity entity)
        {
            TData<string> obj = await userBLL.SaveStationUserForm(entity);
            return obj;
        }

        /// <summary>
        /// 提交(登录系统)数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("station:stationuser:add,station:stationuser:edit")]
        public async Task<TData<string>> SaveLoginFormJson([FromBody] PersonEntity entity)
        {
            var userinfo = await Operator.Instance.Current();
            entity.OrganizationID = userinfo.OrganizationID;
            TData<string> obj = await userBLL.SaveStationUserForm(entity);
            return obj;
        }

        /// <summary>
        /// 更改用户状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("station:stationuser:enable,station:stationuser:disable")]
        public async Task<TData<int>> SetUserStatus([FromQuery] long id, [FromQuery] long status)
        {
            TData<int> obj = await userBLL.SetStationUserStatus(id, status);
            return obj;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("station:stationuser:delete")]
        public async Task<TData> DeleteFormJson([FromBody] string ids)
        {
            TData obj = await userBLL.DeleteStationUserForm(ids);
            return obj;
        }

        #endregion
    }
}
