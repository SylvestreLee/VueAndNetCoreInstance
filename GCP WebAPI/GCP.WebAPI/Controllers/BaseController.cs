﻿using GCP.Business.SystemManage;
using GCP.Code;
using GCP.Entity.SystemManage;
using GCP.Enum;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCP.WebAPI.Controllers
{
    public class BaseController : Controller
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string action = context.RouteData.Values["Action"].ParseToString();
            OperatorInfo user = await Operator.Instance.Current();

            if (GlobalContext.SystemConfig.Demo)
            {
                if (context.HttpContext.Request.Method.ToUpper() == "POST")
                {
                    string[] allowAction = new string[] { "LoginJson", "ExportUserJson", "CodePreviewJson" };
                    if (!allowAction.Select(p => p.ToUpper()).Contains(action.ToUpper()))
                    {
                        TData obj = new TData();
                        obj.Message = "演示模式，不允许操作";
                        context.Result = new JsonResult(obj);
                        return;
                    }
                }
            }

            var resultContext = await next();

            sw.Stop();
            string ip = NetHelper.Ip;
            SysLogOperateEntity operateEntity = new SysLogOperateEntity();
            var areaName = context.RouteData.DataTokens["area"] + "/";
            var controllerName = context.RouteData.Values["controller"] + "/";
            string currentUrl = "/" + areaName + controllerName + action;

            string[] notLogAction = new string[] { "GetServerJson", "Error" };
            if (!notLogAction.Select(p => p.ToUpper()).Contains(action.ToUpper()))
            {
                #region 获取请求参数
                switch (context.HttpContext.Request.Method.ToUpper())
                {
                    case "GET":
                        operateEntity.ExecuteParam = context.HttpContext.Request.QueryString.Value.ParseToString();
                        break;

                    case "POST":
                        if (context.ActionArguments?.Count > 0)
                        {
                            operateEntity.ExecuteUrl += context.HttpContext.Request.QueryString.Value.ParseToString();
                            operateEntity.ExecuteParam = TextHelper.GetSubString(JsonConvert.SerializeObject(context.ActionArguments), 4000);
                        }
                        else
                        {
                            operateEntity.ExecuteParam = context.HttpContext.Request.QueryString.Value.ParseToString();
                        }
                        break;
                }
                #endregion

                #region 异常获取
                StringBuilder sbException = new StringBuilder();
                if (resultContext.Exception != null)
                {
                    Exception exception = resultContext.Exception;
                    sbException.AppendLine(exception.Message);
                    while (exception.InnerException != null)
                    {
                        sbException.AppendLine(exception.InnerException.Message);
                        exception = exception.InnerException;
                    }
                    sbException.AppendLine(resultContext.Exception.StackTrace);
                    operateEntity.LogStatus = OperateStatusEnum.Fail.ParseToInt();
                }
                else
                {
                    operateEntity.LogStatus = OperateStatusEnum.Success.ParseToInt();
                }
                #endregion

                #region 日志实体                  
                if (user != null)
                {
                    operateEntity.BaseCreatorId = user.UserId;
                }

                operateEntity.ExecuteTime = sw.ElapsedMilliseconds.ParseToInt();
                operateEntity.IpAddress = ip;
                operateEntity.ExecuteUrl = currentUrl.Replace("//", "/");
                operateEntity.ExecuteResult = TextHelper.GetSubString(sbException.ToString(), 4000);
                #endregion

                Action taskAction = async () =>
                {
                    // 让底层不用获取HttpContext
                    operateEntity.BaseCreatorId = operateEntity.BaseCreatorId ?? 0;

                    // 耗时的任务异步完成
                    // operateEntity.IpLocation = IpLocationHelper.GetIpLocation(ip);
                    try
                    {
                        await new LogOperateBLL().SaveForm(operateEntity);
                    }
                    catch (Exception err)
                    { }
                };
                AsyncTaskHelper.StartTask(taskAction);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
