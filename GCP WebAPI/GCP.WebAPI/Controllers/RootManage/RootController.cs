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

namespace GCP.WebAPI.Controllers.RootManage.Controllers
{
    /// <summary>
    /// 创 建：ycz
    /// 日 期：2021-08-23 13:41
    /// 描 述：控制器类
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class RootController:BaseController
    {
        private RootBLL rootBLL = new RootBLL();

        #region 获取需要下载的检测记录流水号
        /// <summary>
        /// 获取需要下载的检测记录流水号
        /// </summary>
        /// <param name="StationID">检测站ID</param>
        /// <returns></returns> 
        [HttpGet]
        public async Task<ActionResult> DownloadInspectionID([FromQuery] long StationID)
        {
            TData<List<InspectionCacheEntity>> obj = await rootBLL.DownloadInspectionID(StationID);
            return Json(obj);
        }

        #endregion

        #region 删除临时表信息

        /// <summary> 
        /// 删除临时表信息
        /// </summary> 
        /// <param name="InspectionID">检测流水号</param> 
        /// <returns></returns> 
        [HttpGet]
        public async Task<ActionResult> deleteInspectInfo([FromQuery] string InspectionID)
        {
            TData obj = await rootBLL.deleteInspectInfo(InspectionID);
            return Json(obj);
        }
        #endregion

        #region 下载检测记录详细信息(单条)
        /// <summary>
        /// 下载检测记录详细信息(单条)
        /// </summary>
        /// <param name="StationID">检测站ID</param>
        /// <param name="InspectionID">检测流水号ID</param>
        /// <param name="UpdateInsRAID">车辆审核ID</param>
        /// <returns></returns> 
        [HttpGet]
        public async Task<ActionResult> DownloadInspecttionInfo([FromQuery] long StationID, [FromQuery] string InspectionID, [FromQuery] long? UpdateInsRAID)
        {
            if (UpdateInsRAID != null)
            {
                TData<InsResultAuditEntity> obj = await rootBLL.DownloadInspecttionInfo(UpdateInsRAID);
                return Json(obj);
            }
            else
            {
                TData<InsVehicle> obj = await rootBLL.DownloadInspecttionInfo(StationID, InspectionID);
                return Json(obj);
            }
        }
        #endregion

        #region 通知服务器该车辆取消检测
        /// <summary>
        /// 通知服务器该车辆取消检测
        /// </summary>
        /// <param name="InspectionID">检测流水号</param>
        /// <param name="Reason">取消报检原因
        /// 1	送检人放弃检测
        /// 2	录入的车辆信息或检测参数有误
        /// 3	检测线故障
        /// 4   放弃检测-底盘号不正确（加载减速工况法专用）
        /// 5   预检失败（加载减速工况法专用，Memo中请注明失败的项目）
        /// </param>
        /// <param name="StationID">检测站ID</param>
        [HttpGet]
        public async Task<ActionResult> AbortInspect([FromQuery] long StationID, [FromQuery] string InspectionID, [FromQuery] int Reason)
        {
            TData obj = await rootBLL.AbortInspect(StationID, InspectionID, Reason, "");
            return Json(obj);
        }
        #endregion

        #region 通知服务器车辆开检
        /// <summary>
        ///  通知服务器车辆开检，返回允许或禁止
        /// </summary>
        /// <param name="StationID">检测站ID</param>
        /// <param name="LineID">检测线ID</param>
        /// <param name="InspectionID">流水号</param>
        [HttpGet]
        public async Task<ActionResult> BeginInspect([FromQuery] long StationID, [FromQuery] long LineID, [FromQuery] string InspectionID)
        {
            TData obj = await rootBLL.BeginInspect(StationID, LineID, InspectionID);
            return Json(obj);
        }
        #endregion

        #region 通知服务器车辆已中止（暂停）检测
        /// <summary>
        /// 通知服务器车辆已中止（暂停）检测。
        /// </summary>
        /// <param name="StationID">检测站ID</param>
        /// <param name="InspectID">检测记录流水号</param>
        /// <param name="Memo">备注</param>
        [HttpGet]
        public async Task<ActionResult> StopInspect([FromQuery] long StationID, [FromQuery] string InspectID, [FromQuery] string Memo)
        {
            TData obj = await rootBLL.StopInspect(StationID, InspectID, Memo);
            return Json(obj);
        }
        #endregion

        #region 通知服务器结束检测
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EndInspect([FromBody] EndInspectParam param)
        {
            TData obj = await rootBLL.EndInspect(param);
            return Json(obj);
        }
        #endregion

        #region 获取局端服务器的时间
        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SyncTime()
        {
            TData obj = new TData();
            obj.Tag = 0;
            obj.Message = "获取系统时间成功";
            obj.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return Json(obj);
        }
        #endregion

        #region 获取系统的参数限值
        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="StationID">检测站ID</param>>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetsysParams([FromQuery] long StationID)
        {
            TData<InspectParams> obj = await rootBLL.GetInspectParams(StationID, "");
            return Json(obj);
        }
        #endregion

        #region 解锁（所有机构）检测线和相应的设备
        /// <summary>
        /// 解锁（所有机构）检测线和相应的设备
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> UnlockLines()
        {
            TData obj = await rootBLL.UnlockLines();
            return Json(obj);
        }
        #endregion

        #region 检查每天的合格率
        /// <summary>
        /// 每日检查合格率
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> SetDaily()
        {
            TData obj = await rootBLL.SetDaily();
            return Json(obj);
        }
        #endregion

    }
}
