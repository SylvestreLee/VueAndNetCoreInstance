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

namespace GCP.WebAPI.SystemManage.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private UserBLL userBLL = new UserBLL();

        //[AcceptVerbs("GET", "POST")]
        //[HttpGet]
        //public IActionResult GetCaptchaImage()
        //{
        //    string sessionId = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.Session.Id;

        //    Tuple<string, int> captchaCode = CaptchaHelper.GetCaptchaCode();
        //    byte[] bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
        //    new SessionHelper().WriteSession("CaptchaCode", captchaCode.Item2);
        //    return File(bytes, @"image/jpeg");
        //}

        #region 获取数据

        ///// <summary>
        ///// 获取用户列表
        ///// </summary>
        ///// <param name="param">查询条件</param>
        ///// <returns></returns>
        //[HttpGet]
        //[AuthorizeFilter("system:user:search")]
        //public async Task<TData<List<PersonEntity>>> GetListJson([FromQuery] UserListParam param)
        //{
        //    TData<List<PersonEntity>> obj = await userBLL.GetList(param);
        //    return obj;
        //}

        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:user:search")]
        public async Task<TData<List<PersonEntity>>> GetPageListJson([FromQuery] UserListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<PersonEntity>> obj = await userBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        /// 根据id获取单条用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:user:search")]
        public async Task<TData<PersonEntity>> GetFormJson([FromQuery] long id)
        {
            TData<PersonEntity> obj = await userBLL.GetEntity(id);
            return obj;
        }

        /// <summary>
        /// 导出用户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:user:export")]
        public async Task<TData<string>> ExportUserJson([FromQuery] UserListParam param)
        {
            TData<string> obj = new TData<string>();
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

        #region 登录登出权限相关

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="loginParam">登陆参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData<OperatorInfo>> Login([FromBody] LoginParam loginParam)
        {
            return await userBLL.CheckLogin(loginParam.userName, loginParam.password);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        public async Task<TData> LoginOff()
        {
            var obj = new TData();
            OperatorInfo user = await Operator.Instance.Current();
            if (user != null)
            {
                var Token = user.Token;
                Operator.Instance.RemoveCurrent(Token);

                CacheFactory.Cache.RemoveCache("Token_" + Token);
            }
            obj.Tag = 1;
            obj.Message = "登出成功";
            return obj;
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter]
        public async Task<TData<UserAuthorizeInfo>> GetUserAuthorizeJson()
        {
            TData<UserAuthorizeInfo> obj = new TData<UserAuthorizeInfo>();
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            TData<List<MenuAuthorizeInfo>> objMenuAuthorizeInfo = await new MenuBLL().GetAuthorizeList(operatorInfo);
            obj.Data = new UserAuthorizeInfo();
            obj.Data.IsSystem = operatorInfo.IsSystem;
            if (objMenuAuthorizeInfo.Tag == 1)
            {
                obj.Data.MenuAuthorize = objMenuAuthorizeInfo.Data;
            }
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("system:user:add,system:user:edit")]
        public async Task<TData<string>> SaveFormJson([FromBody]PersonEntity entity)
        {
            TData<string> obj = await userBLL.SaveForm(entity);
            return obj;
        }

        /// <summary>
        /// 更改用户状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:user:enable,system:user:disable")]
        public async Task<TData<int>> SetUserStatus([FromQuery] long id, [FromQuery] long status)
        {
            TData<int> obj = await userBLL.SetUserStatus(id, status);
            return obj;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("system:user:delete")]
        public async Task<TData> DeleteFormJson([FromBody]string ids)
        {
            TData obj = await userBLL.DeleteForm(ids);
            return obj;
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("system:user:resetpwd")]
        public async Task<TData<long>> ResetPasswordJson([FromBody] PersonEntity entity)
        {
            TData<long> obj = await userBLL.ResetPassword(entity);
            return obj;
        }

        /// <summary>
        /// 用户自己更改密码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        public async Task<TData<long>> ChangePasswordJson([FromBody] ChangePasswordParam entity)
        {
            TData<long> obj = await userBLL.ChangePassword(entity);
            return obj;
        }

        ///// <summary>
        ///// 用户自己更改信息
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[AuthorizeFilter("system:user:edit")]
        //public async Task<TData<long>> ChangeUserJson([FromBody] PersonEntity entity)
        //{
        //    TData<long> obj = await userBLL.ChangeUser(entity);
        //    return obj;
        //}

        #endregion
    }
}
