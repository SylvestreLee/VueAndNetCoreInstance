using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using GCP.Entity.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.Service.SystemManage;
using FreeSql.DatabaseModel;
using GCP.Business;
using System.Web;
using GCP.Business.Cache;

namespace GCP.Business.SystemManage
{
    /// <summary>
    /// 创 建：sht
    /// 日 期：2021-08-02 10:58
    /// 描 述：业务类
    /// </summary>
    public class LogOperateEPBBLL
    {
        private CommonBLL commonBLL = new CommonBLL(); 
        private LogOperateEPBService logOperateEPBService = new LogOperateEPBService();

        public async Task<TData<List<LogOperateEPBEntity>>> GetPageList(LogOperateEPBListParam param, Pagination pagination)
        {
            TData<List<LogOperateEPBEntity>> obj = new TData<List<LogOperateEPBEntity>>();
            obj.Data = await logOperateEPBService.GetPageList(param, pagination);
            obj.Data = await GetParamsList(obj.Data);
            obj.Data = await SetJoinTable(obj.Data);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        #region 私有方法

        public async Task<LogOperateEPBEntity> SetJoinTable(LogOperateEPBEntity entity)
        {
            entity = await commonBLL.SetJoinTable(entity, t => t.StationID, t => t.StationName, "Station");
            entity = await commonBLL.SetJoinTable(entity, t => t.LineID, t => t.LineName, "Line");
            entity = await commonBLL.SetJoinTable(entity, t => t.VehicleID, t => t.VehiclePlateID, "Vehicle", "", "ID", "PlateID");
            entity = await commonBLL.SetJoinTable(entity, t => t.VehicleID, t => t.VehiclePlateColor, "Vehicle", "", "ID", "PlateColor");
            entity = await commonBLL.SetJoinTable(entity, t => t.VehiclePlateColor, t => t.VehiclePlateColorName, "Enum_Vehicle_PlateColor");
            entity.VehicleName = entity.VehiclePlateColorName + entity.VehiclePlateID;
            return entity;
        }

        public async Task<List<LogOperateEPBEntity>> SetJoinTable(List<LogOperateEPBEntity> entities)
        {
            entities = await commonBLL.SetJoinTable(entities, t => t.StationID, t => t.StationName, "Station");
            entities = await commonBLL.SetJoinTable(entities, t => t.LineID, t => t.LineName, "Line");
            entities = await commonBLL.SetJoinTable(entities, t => t.VehicleID, t => t.VehiclePlateID, "Vehicle", "", "ID", "PlateID");
            entities = await commonBLL.SetJoinTable(entities, t => t.VehicleID, t => t.VehiclePlateColor, "Vehicle", "", "ID", "PlateColor");
            entities = await commonBLL.SetJoinTable(entities, t => t.VehiclePlateColor, t => t.VehiclePlateColorName, "Enum_Vehicle_PlateColor");
            foreach (var e in entities)
            {
                e.VehicleName = e.VehiclePlateColorName + e.VehiclePlateID;
            }
            return entities;
        }

        /// <summary>
        /// 遍历给实体赋值
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private async Task<List<LogOperateEPBEntity>> GetParamsList(List<LogOperateEPBEntity> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = await SetParams(list[i]);
            }
            return list;
        }

        /// <summary>
        /// 解析参数赋值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task<LogOperateEPBEntity> SetParams(LogOperateEPBEntity entity)
        {
            var param = ParseQueryString(entity.ExecuteParam);
            foreach (var p in param)
            {
                //锁定/解锁检测站
                if (commonBLL.StringUpperEqual(entity.ExecuteUrl, "/Station/SetStationStatus"))
                {
                    //检测站
                    if (commonBLL.StringUpperEqual(p.Key, "id"))
                    {
                        var strv = p.Value;
                        if (Int64.TryParse(strv, out long lv))
                        {
                            entity.StationID = lv;
                        }
                    }
                    //锁定解锁判定
                    if (commonBLL.StringUpperEqual(p.Key, "status"))
                    {
                        var strv = p.Value;
                        if (Int64.TryParse(strv, out long lv))
                        {
                            switch (lv)
                            {
                                case 1:
                                    entity.MethodName = "解锁检测站";
                                    break;
                                case 3:
                                    entity.MethodName = "锁定检测站";
                                    break;
                                default:
                                    entity.MethodName = "未知操作";
                                    break;
                            }
                        }
                    }
                }
                //锁定/解锁检测线
                else if (commonBLL.StringUpperEqual(entity.ExecuteUrl, "/Line/SetLineStatus"))
                {
                    //检测线
                    if (commonBLL.StringUpperEqual(p.Key, "id"))
                    {
                        var strv = p.Value;
                        if (Int64.TryParse(strv, out long lv))
                        {
                            entity.LineID = lv;
                        }
                    }
                    //锁定解锁判定
                    if (commonBLL.StringUpperEqual(p.Key, "status"))
                    {
                        var strv = p.Value;
                        if (Int64.TryParse(strv, out long lv))
                        {
                            switch (lv)
                            {
                                case 1:
                                    entity.MethodName = "解锁检测线";
                                    break;
                                case 3:
                                    entity.MethodName = "锁定检测线";
                                    break;
                                default:
                                    entity.MethodName = "未知操作";
                                    break;
                            }
                        }
                    }
                }
                //车辆
                else if (entity.ExecuteUrl.IndexOf("/Vehicle/") > -1)
                {
                    //车辆
                    if (commonBLL.StringUpperEqual(p.Key, "id"))
                    {
                        var strv = p.Value;
                        if (Int64.TryParse(strv, out long lv))
                        {
                            entity.VehicleID = lv;
                        }
                    }
                    //参数
                    if (commonBLL.StringUpperEqual(p.Key, "FlagValue"))
                    {
                        //车辆特殊标记
                        entity.MethodName = "设置车辆特殊标记";
                        var list = await EnumCache.Table("Enum_Vehicle_SpecialFlag", "Value").GetList();
                        entity.MethodName = entity.MethodName + ":" + list.Where(ex => ex.ID == p.Value).Select(t => t.Name).SingleOrDefault();
                    }
                    else if (commonBLL.StringUpperEqual(p.Key, "EPSValue")) 
                    {
                        //设置车辆排放标准
                        entity.MethodName = "设置车辆排放标准";
                        var list = await EnumCache.Table("Enum_Vehicle_EPStage").GetList();
                        entity.MethodName = entity.MethodName + ":" + list.Where(ex => ex.ID == p.Value).Select(t => t.Name).SingleOrDefault();
                    }
                    else if (commonBLL.StringUpperEqual(p.Key, "OdoMeter"))
                    {
                        //设置里程表读数
                        entity.MethodName = "设置里程表读数:" + p.Value;
                    }
                    else if (commonBLL.StringUpperEqual(entity.ExecuteUrl, "/Vehicle/SetLMD"))
                    {
                        //【this method has to reprogram. 'cuz method has 2 paramters. and I just write 1.】
                        //设置点燃式汽车过量空气系数限值
                        entity.MethodName = "设置点燃式汽车过量空气系数限值:" + p.Value;
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// 拆分param，获取值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseQueryString(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                return new Dictionary<string, string>();
            }
            if (param.Substring(0, 1) == "?")
            {
                param = param.Substring(1);
            }
            //1.去除第一个前导?字符
            var dic = param
                    //2.通过&划分各个参数
                    .Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                    //3.通过=划分参数key和value,且保证只分割第一个=字符
                    .Select(param => param.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries))
                    //4.通过相同的参数key进行分组
                    .GroupBy(part => part[0], part => part.Length > 1 ? part[1] : string.Empty)
                    //5.将相同key的value以,拼接
                    .ToDictionary(group => group.Key, group => string.Join(",", group));

            return dic;
        }

        #endregion
    }
}
