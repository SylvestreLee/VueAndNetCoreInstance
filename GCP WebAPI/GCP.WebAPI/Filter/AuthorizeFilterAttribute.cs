using GCP.Business.SystemManage;
using GCP.Code;
using GCP.Model.Result;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCP.WebAPI.Filter
{
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        public AuthorizeFilterAttribute() { }

        public AuthorizeFilterAttribute(string authorize)
        {
            this.Authorize = authorize;
        }

        /// <summary>
        /// 权限字符串，例如 organization:user:view
        /// </summary>
        public string Authorize { get; set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool hasPermission = false;

            OperatorInfo user = await Operator.Instance.Current();
            if (user == null)
            {
                // 防止用户选择记住我，页面一直在首页刷新
                if (new CookieHelper().GetCookie("RememberMe").ParseToInt() == 1)
                {
                    Operator.Instance.RemoveCurrent();
                }

                TData obj = new TData();
                obj.Message = "抱歉，没有登录或登录已超时";
                obj.Tag = 1000;
                context.Result = new JsonResult(obj);
                return;
            }
            else
            {
                // 系统用户拥有所有权限
                if (user.IsSystem)
                {
                    hasPermission = true;
                }
                else
                {
                    // 权限判断
                    if (!string.IsNullOrEmpty(Authorize))
                    {
                        string[] authorizeList = Authorize.Split(',');
                        TData<List<MenuAuthorizeInfo>> objMenuAuthorize = await new MenuBLL().GetAuthorizeList(user);
                        List<MenuAuthorizeInfo> authorizeInfoList = objMenuAuthorize.Data.Where(p => authorizeList.Contains(p.Authorize)).ToList();
                        if (authorizeInfoList.Any())
                        {
                            hasPermission = true;

                            #region 新增和修改判断
                            if (context.RouteData.Values["Action"].ToString() == "SaveFormJson")
                            {
                                var id = context.HttpContext.Request.Form["Id"];
                                if (id.ParseToLong() > 0)
                                {
                                    if (!authorizeInfoList.Where(p => p.Authorize.Contains("edit")).Any())
                                    {
                                        hasPermission = false;
                                    }
                                }
                                else
                                {
                                    if (!authorizeInfoList.Where(p => p.Authorize.Contains("add")).Any())
                                    {
                                        hasPermission = false;
                                    }
                                }
                            }
                            #endregion
                        }
                        if (!hasPermission)
                        {
                            if (context.HttpContext.Request.IsAjaxRequest())
                            {
                                TData obj = new TData();
                                obj.Message = "抱歉，没有权限";
                                obj.Tag = 999;
                                context.Result = new JsonResult(obj);
                            }
                            else
                            {
                                //context.Result = new RedirectResult("~/Home/NoPermission");
                                TData obj = new TData();
                                obj.Tag = 999;
                                obj.Message = "抱歉，没有权限";
                                context.Result = new JsonResult(obj);
                            }
                        }
                    }
                    else
                    {
                        hasPermission = true;
                    }
                }
                if (hasPermission)
                {
                    var resultContext = await next();
                }
            }
        }
    }
}
