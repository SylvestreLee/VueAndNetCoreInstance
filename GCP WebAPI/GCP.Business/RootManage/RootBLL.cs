using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using GCP.Entity.RootManage;
using GCP.Model.Param.RootManage;
using GCP.Service.RootManage;
using FreeSql.DatabaseModel;
using GCP.Business;
using System.Data;

namespace GCP.Business.RootManage
{
    public class RootBLL
    {
        private RootService rootService = new RootService();

        #region 获取需要下载的检测记录流水号
        /// <summary>
        /// 获取需要下载的检测记录流水号
        /// </summary>
        /// <param name="StationID">检测站ID</param>
        /// <returns></returns>
        public async Task<TData<List<InspectionCacheEntity>>> DownloadInspectionID(long StationID)
        {
            TData<List<InspectionCacheEntity>> obj = new TData<List<InspectionCacheEntity>>();
            obj.Data = await rootService.DownloadInspectionID(StationID);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 删除临时表信息
        /// <summary>
        /// 删除临时表信息
        /// </summary>
        /// <param name="InspectionID">检测流水号</param> 
        /// <returns></returns>
        public async Task<TData> deleteInspectInfo(string InspectionID)
        {
            TData obj = new TData();
            int count = await rootService.deleteInspectInfo(InspectionID);
            if (count > 0)
            {
                obj.Tag = 1;
                obj.Message = "操作成功";
            }
            else
            {
                obj.Tag = 0;
                obj.Message = "操作失败";
            }

            return obj;
        }
        #endregion

        #region 获取车辆检测记录的审核信息（涉及到局端三级审核）
        /// <summary>
        /// 获取车辆检测记录的审核信息（涉及到局端三级审核）
        /// </summary>
        /// <param name="UpdateInsRAID"></param>
        /// <returns></returns>
        public async Task<TData<InsResultAuditEntity>> DownloadInspecttionInfo(long? UpdateInsRAID)
        {
            TData<InsResultAuditEntity> obj = new TData<InsResultAuditEntity>();
            obj.Data = await rootService.DownloadInspecttionInfo(UpdateInsRAID);
            obj.Total = 1;
            if (obj.Data!=null)
            {
                obj.Tag = 0;
                obj.Message = "获取成功!";
            }
            else
            {
                obj.Tag = -1;
                obj.Message = "找不到该车的审核信息";
            }
            return obj;
        }
        #endregion

        #region 获取车辆的检测记录
        /// <summary>
        /// 获取车辆的检测记录
        /// </summary>
        /// <param name="StationID">检测站ID</param>
        /// <param name="InspectionID">检测流水号</param>
        /// <returns></returns>
        public async Task<TData<InsVehicle>> DownloadInspecttionInfo(long StationID, string InspectionID)
        {
            TData<InsVehicle> obj = new TData<InsVehicle>();
            obj.Data = await rootService.DownloadInspecttionInfo(StationID, InspectionID);
            obj.Total = 1;
            if (obj.Data!=null)
            {
                obj.Tag = 0;
                obj.Message = "获取成功!";
            }
            else
            {
                obj.Tag = -1;
                obj.Message = "找不到该车的报检信息,请重新登录本车";
            }
            return obj;
        }
        #endregion

        #region 通知服务器该车辆取消检测
        /// <summary>
        /// 通知服务器该车辆取消检测
        /// </summary>
        /// <param name="InspectID">检测流水号</param>
        /// <param name="Reason">取消报检原因
        /// 1	送检人放弃检测
        /// 2	录入的车辆信息或检测参数有误
        /// 3	检测线故障
        /// 4   放弃检测-底盘号不正确（加载减速工况法专用）
        /// 5   预检失败（加载减速工况法专用，Memo中请注明失败的项目）
        /// </param>
        /// <param name="Memo">备注</param>
        /// <returns></returns>
        public async Task<TData> AbortInspect(long StationID, string InspectionID, int Reason, string Memo)
        {
            TData<string> obj = new TData<string>();
            InspectionEntity entity = await rootService.GetInsState(InspectionID);
            if (entity.Status != 3)
            {
                entity.StationID = StationID;
                entity.InspectionID = InspectionID;
                entity.AbortReason = Reason;
                entity.Status = 4;

                int count = await rootService.UpdateIns(entity);
                if (count > 0)
                {
                    obj.Tag = 0;
                    obj.Message = "流水号：" + InspectionID + "操作成功！";
                }
                else
                {
                    obj.Tag = -1;
                    obj.Message = "流水号：" + InspectionID + "操作失败！";
                }
            }
            else
            {
                obj.Tag = -1;
                obj.Message = "流水号：" + InspectionID + "操作失败,当前车辆已检测完毕！";
            }
            return obj;

        }
        #endregion

        #region 通知服务器车辆开检
        /// <summary>
        /// 通知服务器车辆开检，返回允许或禁止
        /// </summary>
        /// <param name="StationID">检测站ID</param>
        /// <param name="LineID">检测线ID</param>
        /// <param name="InspectionID">检测流水号</param>
        /// <returns></returns>
        public async Task<TData> BeginInspect(long StationID, long LineID, string InspectionID)
        {
            TData<string> obj = new TData<string>();
            InspectionEntity ins = await rootService.GetInsState(InspectionID);
            string LineSupportedMethods = "";
     
            #region 判断检测站和检测线被锁定
            #region 判定检测站状态
            int renum = 0;

            StationEntity stationEntity = await rootService.GetStaState(StationID);
            if (stationEntity.Status != 1)
            {
                obj.Tag = 1;
                EnumStationStatusEntity enumStationStatusEntity = await rootService.GetEnumStaState(stationEntity.Status);
                obj.Message = "检测站状态不正常！当前检测站已被" + enumStationStatusEntity.Name + ",请联系环保局！";
                return obj;
            }
            else
            {
                if (stationEntity.CMAValidTo.Value.AddDays(1) < System.DateTime.Now)
                {
                    obj.Tag = 1;
                    obj.Message = "当前检测站CMA资质已过期,请联系环保局！";
                    return obj;
                }
            }

            #endregion
            #region 判定检测线
            LineEntity line = await rootService.GetLine(LineID, StationID);
            if (line != null)
            {
                if (line.Status != 1)
                {
                    obj.Tag = 1;
                    EnumLineStatusEntity lineenum = await rootService.GetLineState(line.Status);
                    obj.Message = "检测线状态不正常！当前检测线已被" + lineenum.Name + ",请联系环保局！";
                    return obj;
                }
            }
            else
            {
                obj.Tag = 1;
                obj.Message = "当前检测线不存在，请检查输入参数是否正确！当前输入检测线ID为：LineID=" + LineID.ToString() + "！";
                return obj;
            }


            #endregion

            #endregion

            #region   判断设备是否已过标定有效期
            List<DevicesEntity> lists = await rootService.GetDevices(LineID);

            if (lists.Count > 0)
            {
                if (ins.RealInspectionMethod == 1)
                {
                    if (line.Type == 2)
                    {
                        obj.Tag = 1;
                        obj.Message = "当前检测线为柴油线！请检查输入线号是否正确！当前输入检测线ID为：LineID = " + LineID.ToString() + "！";
                        return obj;
                    }
                    else
                    {
                        LineSupportedMethods = "," + line.SupportedMethods + ",";
                        if (LineSupportedMethods.Contains(",1,"))
                        {
                        }
                        else
                        {
                            obj.Tag = 1;
                            obj.Message = "当前检测线不支持简易瞬态工况，请检查当前检测线平台备案信息！";
                            return obj;
                        }
                    }
                    #region 简易瞬态
                    if (lists.Where(t => t.Type == 4).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "找不到可用的设备底盘测功机！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 4 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "底盘测功机已过标定有效期，请及时标定！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 1).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "找不到可用的设备尾气分析仪！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 1 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "尾气分析仪已过标定有效期，请及时标定！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 5).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "找不到可用的设备空气流量计！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 5 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "流量计已过标定有效期，请及时标定！";
                        return obj;
                    }
                    #endregion
                }
                else if (ins.RealInspectionMethod == 2)
                {
                    if (line.Type == 2)
                    {
                        obj.Tag = 1;
                        obj.Message = "当前检测线为柴油线！请检查输入线号是否正确！当前输入检测线ID为：LineID = " + LineID.ToString() + "！";
                        return obj;
                    }
                    else
                    {
                        LineSupportedMethods = "," + line.SupportedMethods + ",";
                        if (LineSupportedMethods.Contains(",2,"))
                        {
                        }
                        else
                        {
                            obj.Tag = 1;
                            obj.Message = "当前检测线不支持双怠速工况，请检查当前检测线平台备案信息！";
                            return obj;
                        }
                    }
                    #region 双怠速
                    if (lists.Where(t => t.Type == 6).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "找不到可用的设备转速计！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 6 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "转速计已过标定有效期，请及时标定！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 1).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "找不到可用的设备尾气分析仪！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 1 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "尾气分析仪已过标定有效期，请及时标定！";
                        return obj;
                    }
                    #endregion
                }
                else if (ins.RealInspectionMethod == 3)
                {
                    if (line.Type == 1)
                    {
                        obj.Tag = 1;
                        obj.Message = "当前检测线为汽油线！请检查输入线号是否正确！当前输入检测线ID为：LineID = " + LineID.ToString() + "！";
                        return obj;
                    }
                    else
                    {
                        LineSupportedMethods = "," + line.SupportedMethods + ",";
                        if (LineSupportedMethods.Contains(",3,"))
                        {
                        }
                        else
                        {
                            obj.Tag = 1;
                            obj.Message = "当前检测线不支持自由加速滤纸烟度，请检查当前检测线平台备案信息！";
                            return obj;
                        }
                        #region 自由加速滤纸烟度法
                        if (lists.Where(t => t.Type == 3).ToList().Count == 0)
                        {
                            obj.Tag = 1;
                            obj.Message = "找不到可用的设备滤纸烟度计！";
                            return obj;
                        }
                        if (lists.Where(t => t.Type == 3 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                        {
                            obj.Tag = 1;
                            obj.Message = "滤纸烟度计已过标定有效期，请及时标定！";
                            return obj;
                        }
                        #endregion
                    }
                }
                else if (ins.RealInspectionMethod == 4)
                {
                    if (line.Type == 1)
                    {
                        obj.Tag = 1;
                        obj.Message = "当前检测线为汽油线！请检查输入线号是否正确！当前输入检测线ID为：LineID = " + LineID.ToString() + "！";
                        return obj;
                    }
                    else
                    {
                        LineSupportedMethods = "," + line.SupportedMethods + ",";
                        if (LineSupportedMethods.Contains(",4,"))
                        {
                        }
                        else
                        {
                            obj.Tag = 1;
                            obj.Message = "当前检测线不支持自由加速不透光烟度，请检查当前检测线平台备案信息！";
                            return obj;
                        }
                        #region 自由加速不透光烟度法
                        if (lists.Where(t => t.Type == 2).ToList().Count == 0)
                        {
                            obj.Tag = 1;
                            obj.Message = "找不到可用的设备不透光烟度计！";
                            return obj;
                        }
                        if (lists.Where(t => t.Type == 2 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                        {
                            obj.Tag = 1;
                            obj.Message = "不透光烟度计已过标定有效期，请及时标定！";
                            return obj;
                        }
                        if (lists.Where(t => t.Type == 4).ToList().Count == 0)
                        {
                            obj.Tag = 1;
                            obj.Message = "找不到可用的设备底盘测功机！";
                            return obj;
                        }
                        if (lists.Where(t => t.Type == 4 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                        {
                            obj.Tag = 1;
                            obj.Message = "底盘测功机已过标定有效期，请及时标定！";
                            return obj;
                        }
                        #endregion
                    }
                }
                else if (ins.RealInspectionMethod == 5)
                {
                    if (line.Type == 1)
                    {
                        obj.Tag = 1;
                        obj.Message = "当前检测线为汽油线！请检查输入线号是否正确！当前输入检测线ID为：LineID = " + LineID.ToString() + "！";
                        return obj;
                    }
                    else
                    {
                        LineSupportedMethods = "," + line.SupportedMethods + ",";
                        if (LineSupportedMethods.Contains(",5,"))
                        {
                        }
                        else
                        {
                            obj.Tag = 1;
                            obj.Message = "当前检测线不支持加载减速工况，请检查当前检测线平台备案信息！";
                            return obj;
                        }
                        #region 加载减速
                        if (lists.Where(t => t.Type == 2).ToList().Count == 0)
                        {
                            obj.Tag = 1;
                            obj.Message = "找不到可用的设备不透光烟度计！";
                            return obj;
                        }
                        if (lists.Where(t => t.Type == 2 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                        {
                            obj.Tag = 1;
                            obj.Message = "不透光烟度计已过标定有效期，请及时标定！";
                            return obj;
                        }
                        if (lists.Where(t => t.Type == 6).ToList().Count == 0)
                        {
                            obj.Tag = 1;
                            obj.Message = "找不到可用的设备转速计！";
                            return obj;
                        }
                        if (lists.Where(t => t.Type == 6 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                        {
                            obj.Tag = 1;
                            obj.Message = "转速计已过标定有效期，请及时标定！";
                            return obj;
                        }
                        bool bCheck_NOX = Convert.ToBoolean(rootService.GetCheckDailyRemote(StationID, "Check_NOX"));
                        if (bCheck_NOX)
                        {
                            if (lists.Where(t => t.Type == 10).ToList().Count == 0)
                            {
                                obj.Tag = 1;
                                obj.Message = "找不到可用的设备氮氧化物分析仪！";
                                return obj;
                            }
                            if (lists.Where(t => t.Type == 10 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                            {
                                obj.Tag = 1;
                                obj.Message = "氮氧化物分析仪已过标定有效期，请及时标定！";
                                return obj;
                            }
                        }
                        #endregion
                    }
                }
                else if (ins.InspectionMethod == 6)
                {
                    if (line.Type == 2)
                    {
                        obj.Tag = 1;
                        obj.Message = "当前检测线为柴油线！请检查输入线号是否正确！当前输入检测线ID为：LineID = " + LineID.ToString() + "！";
                        return obj;
                    }
                    else
                    {
                        LineSupportedMethods = "," + line.SupportedMethods + ",";
                        if (LineSupportedMethods.Contains(",6,"))
                        {
                        }
                        else
                        {
                            obj.Tag = 1;
                            obj.Message = "当前检测线不支持简易稳态工况，请检查当前检测线平台备案信息！";
                            return obj;
                        }
                    }
                    #region 简易稳态
                    if (lists.Where(t => t.Type == 1).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "找不到可用的设备尾气分析仪！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 1 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "尾气分析仪已过标定有效期，请及时标定！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 4).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "找不到可用的设备底盘测功机！";
                        return obj;
                    }
                    if (lists.Where(t => t.Type == 4 && t.CalibrateValidTo.Value.AddDays(1) >= DateTime.Now).ToList().Count == 0)
                    {
                        obj.Tag = 1;
                        obj.Message = "底盘测功机已过标定有效期，请及时标定！";
                        return obj;
                    }
                    #endregion
                }
            }
            else
            {
                obj.Tag = 1;
                obj.Message = "当前检测线下无相关设备！";
                return obj;
            }

            #endregion

            #region 开始检测
            InspectionEntity inspectionEntity = await rootService.Getins(InspectionID, LineID);//非本次
            EnumInspectionStatusEntity enumInspectionStatusEntity = await rootService.GetEnumInsStatus(ins.Status);
            if (inspectionEntity==null) //此时检测线上没有车辆
            {
                if (ins.Status == 1 || ins.Status == 2 || ins.Status == 5 || ins.Status == 6)
                {
                    ins.LineID = LineID;
                    ins.Status = 2;
                    obj.Tag = 0;
                    int count = await rootService.UpdateIns(ins);
                    if (count > 0)
                    {
                        obj.Message = "允许开始检测"; ;
                    }
                    else
                    {
                        obj.Message = "开始检测失败，修改检测信息失败";
                    }
                }
                else
                {
                    obj.Tag = -1;
                    obj.Message = "流水号：" + InspectionID + "的车辆检测状态不正确,当前检测状态为" + ins.Status + "(" + enumInspectionStatusEntity.Name + ")";
                    return obj;
                }
            }
            else
            {
                //终止所有本条检测线上非本次检测车辆的检测记录
                inspectionEntity.Status = 1;
                int flag = await rootService.UpdateIns(inspectionEntity);
                if (flag > 0)
                {
                    if (ins.Status == 1 || ins.Status == 2 || ins.Status == 5 || ins.Status == 6)
                    {
                        ins.LineID = LineID;
                        ins.Status = 2;
                        obj.Tag = 0;
                        int count = await rootService.UpdateIns(ins);
                        if (count > 0)
                        {
                            obj.Message = "开始检测成功"; ;
                        }
                        else
                        {
                            obj.Message = "开始检测失败，修改检测信息失败";
                        }
                    }
                    else
                    {
                        obj.Tag = -1;
                        obj.Message = "流水号：" + InspectionID + "的车辆检测状态不正确,当前检测状态为" + ins.Status + "(" + enumInspectionStatusEntity.Name + ")";
                        return obj;
                    }
                }
                else
                {
                    obj.Tag = -1;
                    obj.Message = "流水号：" + InspectionID + "的车辆报检失败！线上有其他车辆正在检测！检测流水号为：" + inspectionEntity.InspectionID;
                    return obj;
                }
            }
            #endregion

            return obj;
        }
        #endregion

        #region 通知服务器车辆已中止（暂停）检测。
        /// <summary>
        /// 通知服务器车辆已中止（暂停）检测。
        /// </summary>
        /// <param name="StationID">检测站ID</param>
        /// <param name="InspectID">检测记录流水号</param>
        /// <param name="Memo">备注</param>
        public async Task<TData> StopInspect(long StationID, string InspectionID, string Memo)
        {
            TData<string> obj = new TData<string>();
            InspectionEntity entity = await rootService.GetInsState(InspectionID, StationID);
            if (entity.Status == 1 || entity.Status == 2 || entity.Status == 5 || entity.Status == 6)
            {
                entity.Status = 1;
                entity.UpdateTime = DateTime.Now;
                int count = await rootService.UpdateIns(entity);
                if (count > 0)
                {
                    obj.Message = "流水号：" + InspectionID + "操作成功";
                    obj.Tag = 0;
                }
                else
                {
                    obj.Tag = -1;
                    obj.Message = "流水号：" + InspectionID + "操作失败";
                }
            }
            else
            {
                obj.Message = "流水号：" + InspectionID + "操作失败,车辆检测状态不正确,当前状态为" + entity.Status; ;
                obj.Tag = -1;
            }
            return obj;
        }
        #endregion

        #region 通知服务器结束检测
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TData> EndInspect(EndInspectParam param)
        {
            TData<string> obj = new TData<string>();
            InspectionEntity ins = await rootService.GetInsState(param.InspectionID, param.StationID);
            if (ins != null)
            {
                ins.InspectionItem = param.InspectionItem;
                ins.JYRQ = param.JYRQ;
                ins.RealInspectionMethod = param.RealInspectionMethod;
                ins.ReportNumber = param.ReportNumber;
                ins.Result = param.Result;
                ins.Status = 3;
                ins.UpdateTime = DateTime.Now;

                int count = await rootService.UpdateIns(ins);

                if (count > 0)
                {
                    obj.Tag = 0;
                    obj.Message = "检测记录更新成功";
                }
                else
                {
                    obj.Tag = 1;
                    obj.Message = "检测记录更新失败";
                }
            }
            else
            {
                obj.Tag = 1;
                obj.Message = "未找到当前检测记录，检测流水号为：" + param.InspectionID;
                return obj;
            }
            return obj;
        }
        #endregion

        #region 获取系统的参数限值
        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="StationID">检测站ID</param>
        /// <param name="Memo">备注</param>
        /// <returns></returns>
        public async Task<TData<InspectParams>> GetInspectParams(long StationID, string Memo)
        {
            TData<InspectParams> obj = new TData<InspectParams>();
            obj.Tag = 0;
            obj.Message = "获取参数成功";
            InspectParams inspectParams = new InspectParams();
            inspectParams.DPCGJYXQX = await rootService.GetCheckDailyRemote(StationID, "DPCGJYXQX");
            inspectParams.SBZDJCYXQX = await rootService.GetCheckDailyRemote(StationID, "SBZDJCYXQX");
            inspectParams.SRMMJXCS = await rootService.GetCheckDailyRemote(StationID, "SRMMJXCS");
            inspectParams.HXJCZGCS = await rootService.GetCheckDailyRemote(StationID, "HXJCZGCS");

            inspectParams.TestTime = await rootService.GetCheckDailyRemote(StationID, "TestTime");
            inspectParams.PassWord = await rootService.GetCheckDailyRemote(StationID, "PassWord");
            inspectParams.JCZQ = await rootService.GetCheckDailyRemote(StationID, "JCZQ");

            EnumInspectionMethodEntity methodStatus= await rootService.GetEnumInsMethod(1);
            if (methodStatus.Status == 2)
            {
                inspectParams.COXLimit = await rootService.GetCheckDailyRemote(StationID, "COXLimit");
            }
            else
            {
                inspectParams.LLJYXQX = await rootService.GetCheckDailyRemote(StationID, "LLJYXQX");
                inspectParams.FXYXYSJ = await rootService.GetCheckDailyRemote(StationID, "FXYXYSJ");
            }
            obj.Data = inspectParams;
            return obj;

        }
        #endregion

        #region 解锁（所有机构）检测线和相应的设备
        /// <summary>
        /// 解锁（所有机构）检测线和相应的设备
        /// </summary>
        /// <returns></returns>
        public async Task<TData> UnlockLines()
        {
            TData obj = new TData();
            obj.Tag = 0;
            obj.Message = "解锁成功";
            List<LineEntity> list = await rootService.GetLine();
            foreach (LineEntity item in list)
            {
                #region 解锁检测线
                item.Status = 1;
                item.Remarks = "";
                item.UpdateTime = DateTime.Now;
                await rootService.SaveLine(item);
                #endregion 
                #region 解锁设备
                List<DevicesEntity> devicesEntity = await rootService.GetDevices(item.ID);
                if (devicesEntity.Count > 0)
                {
                    foreach (var item2 in devicesEntity)
                    {
                        item2.Status = 1;
                        item2.Remarks = "";
                        item2.UpdateTime = DateTime.Now;
                        await rootService.SaveDevices(item2);
                    }
                }
                #endregion
            }
            return obj;
        }
        #endregion


        #region 每日检查合格率
        /// <summary>
        /// 每日检查合格率
        /// </summary>
        public async Task<TData> SetDaily()
        {
            TData obj = new TData();
            List<StationEntity> list = await rootService.GetStationEntities();
            foreach (var item in list)
            {
                await GetPassRates(item.ID);
            }
            obj.Tag = 0;
            obj.Message = "每日检查合格率成功";
            return obj;
        }

        /// <summary>
        /// 获取各检测线初检合格率
        /// </summary>
        /// <returns></returns>
        public async Task GetPassRates(long StationID)
        {
            bool bCheckDailyLimit = Convert.ToBoolean(await rootService.GetCheckDailyRemote(StationID, "PRLimit_Enabled"));//是否判定 
                                                                                                                                     //执行惩罚制度的昨日初检最小总检量
            int PRLimit_MinTotalDaily = Convert.ToInt32(await rootService.GetCheckDailyRemote(StationID, "PRLimit_MinTotalDaily"));
            string strPRLimit_PRDaily = await rootService.GetCheckDailyRemote(StationID, "PRLimit_PRDaily");
            string strValueDate = await rootService.GetCheckDailyRemote(StationID, "PRLimit_PRDaily");
            string[] Params = strPRLimit_PRDaily.Split(',');
            //D需要向前统计的天数
            int D = Convert.ToInt32(Params[0]);
            //E检测线锁定天数
            int E = Convert.ToInt32(Params[1]);
            //L合格率控制限值
            int L = Convert.ToInt32(Params[2]);
            DataTable dt = await rootService.GetPassRateBySID(StationID, D);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FirstInspectionPassRateEntity entity = new FirstInspectionPassRateEntity();
                    entity.InspectionCount = 1;
                    entity.StationID = dt.Rows[i]["StationID"].ParseToInt();
                    entity.LineID = dt.Rows[i]["LineID"].ParseToInt();
                    entity.C = dt.Rows[i]["C"].ToString().ParseToDateTime();
                    entity.D = D;
                    entity.E = E;
                    entity.L = L;
                    entity.PRLimit_Enabled = bCheckDailyLimit;
                    entity.PRLimit_MinTotalDaily = PRLimit_MinTotalDaily;
                    entity.InspectionParamValueDate = strValueDate.ParseToDateTime();
                    int cjcsl = Convert.ToInt32(dt.Rows[i]["初检车数量"].ToString().Trim());
                    entity.VehicleCount = dt.Rows[i]["初检车数量"].ParseToInt();
                    entity.FirstInsCount = dt.Rows[i]["初检车次"].ParseToInt();
                    entity.FirstPassCount = dt.Rows[i]["初检合格车次"].ParseToInt();
                    entity.Status = 1;
                    entity.UpdateTime = DateTime.Now;
                    long cjcount = Convert.ToInt64(dt.Rows[i]["初检车次"].ToString());
                    long cjhgcount = Convert.ToInt64(dt.Rows[i]["初检合格车次"].ToString());
                    float doublecj = 0;
                    if (doublecj == 0)
                    {
                        entity.FirstPassRate = 0;

                    }
                    else
                    {
                        entity.FirstPassRate = doublecj.ParseToDouble();
                    }

                    if (bCheckDailyLimit == true && cjcsl >= PRLimit_MinTotalDaily && doublecj >= L)
                    {
                        LineEntity line = await rootService.GetLine(entity.LineID);
                        if (line != null)
                        {
                            line.Status = 3;
                            line.Remarks = "当前检测线合格率异常偏高，暂停检测。该检测线自" + DateTime.Now.AddDays(-D).ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("yyyy-MM-dd") + "初检车数量为：" + cjcsl + ">=" + PRLimit_MinTotalDaily;
                            line.PRDailyValidTo = DateTime.Now.AddDays(E);
                            line.UpdateTime = DateTime.Now;
                            await rootService.SaveLine(line);
                            entity.LockValidTo = DateTime.Now.ToString("yyyy-MM-dd").ParseToDateTime();
                            entity.Remarks = "已启用合格率管控措施。当前检测线合格率异常偏高，暂停检测。该检测线自" + DateTime.Now.AddDays(-D).ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("yyyy-MM-dd") + "初检车数量为：" + cjcsl + ">=" + PRLimit_MinTotalDaily;
                            List<DevicesEntity> devlist = await rootService.GetDevices(entity.LineID);
                            if (devlist.Count > 0)
                            {
                                foreach (var item in devlist)
                                {
                                    item.Status = 3;
                                    item.Remarks = "合格率异常偏高，暂停检测。";
                                    item.UpdateTime = DateTime.Now;
                                    await rootService.SaveDevices(item);
                                }
                            }
                        }
                    }
                    await rootService.SaveFirstInspectionPassRate(entity);
                }
            }
        }
        #endregion

    }

}
