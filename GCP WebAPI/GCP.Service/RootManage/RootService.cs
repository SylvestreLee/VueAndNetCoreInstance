using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeSql.DatabaseModel;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using GCP.Repository;
using GCP.Entity.RootManage;
using GCP.Model.Param.RootManage;
using System.Data;

namespace GCP.Service.RootManage
{
    public class RootService
    {
        private RepositoryBase<InspectionCacheEntity> repositoryBase = new RepositoryBase<InspectionCacheEntity>();
        private RepositoryBase<InsResultAuditEntity> repositoryBase_InsRA = new RepositoryBase<InsResultAuditEntity>();
        private RepositoryBase<InspectionEntity> ins = new RepositoryBase<InspectionEntity>();
        private RepositoryBase<StationEntity> repositoryBaseinsStation = new RepositoryBase<StationEntity>();
        private RepositoryBase<EnumStationStatusEntity> RepositoryBase_ess = new RepositoryBase<EnumStationStatusEntity>();
        private RepositoryBase<LineEntity> repositoryLine = new RepositoryBase<LineEntity>();
        private RepositoryBase<EnumLineStatusEntity> repositoryBaseenumline = new RepositoryBase<EnumLineStatusEntity>();
        private RepositoryBase<DevicesEntity> devBase = new RepositoryBase<DevicesEntity>();
        private RepositoryBase<EnumInspectionStatusEntity> enumInsState = new RepositoryBase<EnumInspectionStatusEntity>();
        private RepositoryBase<EnumInspectionMethodEntity> enumInsMethod = new RepositoryBase<EnumInspectionMethodEntity>();
        private RepositoryBase<InspectionParamRemoteEntity> insParamRemote = new RepositoryBase<InspectionParamRemoteEntity>();
        private RepositoryBase<InspectionParamListEntity> insParamList = new RepositoryBase<InspectionParamListEntity>();

        private RepositoryBase<FirstInspectionPassRateEntity> firstBase = new RepositoryBase<FirstInspectionPassRateEntity>();
        private IFreeSql sql = Sql.GetFsql();

        #region 获取需要下载的检测记录流水号
        /// <summary>
        /// 获取需要下载的检测记录流水号
        /// </summary>
        /// <param name="StationID">检测站ID</param>
        /// <returns></returns>
        public async Task<List<InspectionCacheEntity>> DownloadInspectionID(long StationID)
        {
            Expression<Func<InspectionCacheEntity, bool>> expression = (ex => ex.Status != 2);
            expression = expression.And((i) => i.StationID == StationID
                     );
            // var expression = ListFilter(param);
            var list = await repositoryBase
                            .Where(expression)
                            .ToListAsync((i) => new InspectionCacheEntity()
                            {
                                InspectionID = i.InspectionID,
                                UpdateInsRAID = i.UpdateInsRAID
                            });
            return list;
        }
        #endregion

        #region 删除临时表信息
        /// <summary>
        /// 删除临时表信息
        /// </summary>
        /// <param name="InspectionID">检测流水号</param> 
        /// <returns></returns>
        public async Task<int> deleteInspectInfo(string InspectionID)
        {
            Expression<Func<InspectionCacheEntity, bool>> expression = (ex => ex.Status != 2);
            expression = expression.And((i) => i.InspectionID == InspectionID
                     );
            int count = await repositoryBase.DeleteAsync(expression);
            return count;
        }
        #endregion

        #region 获取车辆检测记录的审核信息（涉及到局端三级审核）
        /// <summary>
        /// 获取车辆检测记录的审核信息（涉及到局端三级审核）
        /// </summary>
        /// <param name="UpdateInsRAID"></param>
        /// <returns></returns>
        public async Task<InsResultAuditEntity> DownloadInspecttionInfo(long? UpdateInsRAID)
        {
            Expression<Func<InsResultAuditEntity, bool>> expression = (ex => ex.Status != 2);
            expression = expression.And(ex => ex.ID == UpdateInsRAID);
            var list = await repositoryBase_InsRA
                            .Where(expression)
                            .FirstAsync();
            return list;
        }
        #endregion

        #region 获取车辆详细检测信息
        /// <summary>
        /// 获取车辆详细检测信息
        /// </summary>
        /// <param name="StationID"></param>
        /// <param name="InspectionID"></param>
        /// <returns></returns>
        public async Task<InsVehicle> DownloadInspecttionInfo(long StationID, string InspectionID)
        {
            Expression<Func<InspectionEntity, VehicleEntity, bool>> expression = ((i, r) => i.Status != 2);
            expression = expression.And((i, r) => i.StationID == StationID && i.InspectionID == InspectionID);
            var list = await sql.Select<InspectionEntity>().From<VehicleEntity>((i, r) => i
              .LeftJoin(a => a.VehicleID == r.ID))
              .Where(expression)
              .FirstAsync((i, r) => new InsVehicle
              {
                  inspectionEntity=i,
                  vehicleEntity=r
              });

            return list;
        }
        #endregion

        #region 根据流水号获取检测实体类
        /// <summary>
        /// 根据流水号获取检测实体类
        /// </summary>
        /// <param name="InspectionID">检测流水号</param>
        /// <returns></returns>
        public async Task<InspectionEntity> GetInsState(string InspectionID)
        {
            return await ins.Where(ex => ex.InspectionID == InspectionID).FirstAsync();
        }
        #endregion

        #region 根据流水号和检测站ID获取检测信息
        /// <summary>
        /// 根据流水号和检测站ID获取检测信息
        /// </summary>
        /// <param name="InspectionID">检测流水号</param>
        /// <param name="StationID">检测站ID</param>
        /// <returns></returns>
        public async Task<InspectionEntity> GetInsState(string InspectionID, long StationID)
        {
            return await ins.Where(ex => ex.InspectionID == InspectionID && ex.StationID == StationID).FirstAsync();
        }
        #endregion

        #region 根据检测状态ID获取检测状态枚举表
        /// <summary>
        /// 根据检测状态ID获取检测状态枚举表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EnumInspectionStatusEntity> GetEnumInsStatus(long id)
        {
            return await enumInsState.FindAsync(id);
        }
        #endregion

        #region 根据检测方法ID获取检测方法枚举表
        /// <summary>
        /// 根据检测方法ID获取检测方法枚举表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EnumInspectionMethodEntity> GetEnumInsMethod(long id)
        {
            return await enumInsMethod.FindAsync(id);
        }
        #endregion

        #region 更新检测记录实体
        /// <summary>
        /// 更新检测记录实体
        /// </summary>
        /// <param name="entity">检测信息实体类</param>
        /// <returns></returns>
        public async Task<int> UpdateIns(InspectionEntity entity)
        {
            entity.Modify();
            return await ins.UpdateNotNullAsync(entity);
        }
        #endregion

        #region 获取检测站列表
        /// <summary>
        /// 获取检测站列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<StationEntity>> GetStationEntities()
        {
            Expression<Func<StationEntity, bool>> expression = (ex => ex.Status != 2);
            var list = await repositoryBaseinsStation
                 .Where(expression)
                 .ToListAsync();
            return list;
        }
        #endregion

        #region 获取检测站的信息
        /// <summary>
        /// 获取检测站的信息
        /// </summary>
        /// <param name="StationID">检测站的主键</param>
        /// <returns></returns>
        public async Task<StationEntity> GetStaState(long StationID)
        {
            return await repositoryBaseinsStation.FindAsync(StationID);
        }
        #endregion

        #region 获取检测站枚举表的信息
        /// <summary>
        /// 获取检测站枚举表的信息
        /// </summary>
        /// <param name="StationID">枚举表的主键</param>
        /// <returns></returns>
        public async Task<EnumStationStatusEntity> GetEnumStaState(long ID)
        {
            return await RepositoryBase_ess.FindAsync(ID);
        }
        #endregion

        #region 根据检测线的ID和检测站的id获取检测线信息
        /// <summary>
        /// 根据检测线的ID和检测站的id
        /// </summary>
        /// <param name="LineID">检测线主键</param>
        /// <param name="StationID">检测站id</param>
        /// <returns></returns>
        public async Task<LineEntity> GetLine(long LineID, long StationID)
        {
            return await repositoryLine.Where(ex => ex.ID == LineID && ex.StationID == StationID).FirstAsync();
        }
        #endregion

        #region 获取所有因惩罚锁定的检测线
        /// <summary>
        /// 获取所有因惩罚锁定的检测线
        /// </summary>
        /// <returns></returns>
        public async Task<List<LineEntity>> GetLine()
        {
            Expression<Func<LineEntity, bool>> expression = (ex => ex.Status == 3 && ex.PRDailyValidTo <= DateTime.Now);
            var list = await repositoryLine
                .Where(expression)
                .ToListAsync();
            return list;
        }
        #endregion

        #region 根据检测线ID获取检测线信息
        /// <summary>
        /// 根据检测线ID获取检测线信息
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public async Task<LineEntity> GetLine(long LineID)
        {
            return await repositoryLine.FindAsync(LineID);
        }
        #endregion

        #region 保存检测线信息
        /// <summary>
        /// 保存修改检测线信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task SaveLine(LineEntity entity)
        {
            if (entity.ID.IsNullOrZero())
            {
                entity.Create();
                await repositoryLine.InsertAsync(entity);
            }
            else
            {
                entity.Modify();
                await repositoryLine.UpdateNotNullAsync(entity);
            }
        }
        #endregion

        #region 获取检测线状态枚举表
        /// <summary>
        /// 获取检测线状态枚举表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EnumLineStatusEntity> GetLineState(long id)
        {
            return await repositoryBaseenumline.FindAsync(id);
        }

        #endregion

        #region 根据检测线Id获取设备信息
        /// <summary>
        /// 根据检测线Id获取设备信息
        /// </summary>
        /// <param name="LindID"></param>
        /// <returns></returns>
        public async Task<List<DevicesEntity>> GetDevices(long LindID)
        {
            Expression<Func<DevicesEntity, bool>> expression = (ex => ex.LineID == LindID);
            var list = await devBase
                .Where(expression)
                .ToListAsync();
            return list;

        }
        #endregion

        #region 保存修改设备信息
        /// <summary>
        /// 保存修改设备信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task SaveDevices(DevicesEntity entity)
        {
            if (entity.ID.IsNullOrZero())
            {
                entity.Create();
                await devBase.InsertAsync(entity);
            }
            else
            {
                entity.Modify();
                await devBase.UpdateNotNullAsync(entity);
            }
        }
        #endregion

        #region 获取检测线上非本次检测的检测记录
        /// <summary>
        /// 获取检测线上非本次检测的检测记录
        /// </summary>
        /// <param name="InspectionID">检测流水号</param>
        /// <param name="LineID">检测线ID</param>
        /// <returns></returns>
        public async Task<InspectionEntity> Getins(string InspectionID, long LineID)
        {
            return await ins.Where(t => t.InspectionID != InspectionID && t.LineID == LineID && t.Status == 2).FirstAsync();
        }
        #endregion

        #region 获取限制值inspectionparam_remote
        /// <summary>
        /// 根据检测站ID和限值名称获取限制值
        /// </summary>
        /// <param name="SationID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public async Task<string> GetCheckDailyRemote(long StationID, string Name)
        {
            string Value = "";
            string StationID2 = "," + StationID + ",";
            InspectionParamRemoteEntity entity = await insParamRemote.Where(t => t.Name == Name && t.Status == 1 && (t.ApplyToStations.Contains(StationID2) == true || string.IsNullOrEmpty(t.ApplyToStations)) && (t.ValidFrom.IsNullOrZero()|| t.ValidFrom <= DateTime.Now) && (t.ValidTo.IsNullOrZero() || t.ValidTo >= DateTime.Now)).OrderBy(t=>t.ValueDate).FirstAsync();
            if (entity != null)
            {
                Value = entity.Value;
            }
            else
            {
                InspectionParamListEntity inslist = await insParamList.Where(t => t.Name == Name).FirstAsync();
                Value = inslist.DefaultValue;
            }
            return Value;
        }
        #endregion

        #region 获取检测站X天内的合格率
        /// <summary>
        /// 获取检测站X天内的合格率
        /// </summary>
        /// <param name="StationID"></param>
        /// <param name="Day"></param>
        /// <returns></returns>
        public async Task<DataTable> GetPassRateBySID(long StationID, int Day)
        {
            DateTime dtNow = DateTime.Now;
            StringBuilder sbPassRate = new StringBuilder();
            sbPassRate.Append(" select ");
            sbPassRate.Append(" Convert(varchar,L.StationID) StationID, ");
            sbPassRate.Append(" Convert(varchar,L.ID) LineID, ");
            sbPassRate.Append(" CONVERT (nvarchar(10),GETDATE(),120) AS C,'' D,'' E,'' L, ");
            sbPassRate.Append(" count(distinct ins.vin) '初检车数量', 	 ");
            sbPassRate.Append(" sum(case when  ins.LineID=L.ID then 1 else 0 end) as 初检车次, ");
            sbPassRate.Append(" sum(case when  Ins.LineID=L.ID and Ins.Result=1 then 1 else 0 end) as 初检合格车次,'' 初检合格率 ");
            sbPassRate.Append(" from Line L ");
            sbPassRate.Append(" left join Inspection Ins on Ins.LineID=L.ID ");
            sbPassRate.Append(" left join Station s on s.id=l.StationID ");
            sbPassRate.Append(" left join Vehicle v on v.id=ins.VehicleID ");
            sbPassRate.Append(" where L.status=1 and  s.status<>2 and  Ins.endtime>='" + dtNow.AddDays(-Day).ToString("yyyy-MM-dd") + "' and Ins.endtime<='" + dtNow.ToString("yyyy-MM-dd") + "' and Ins.inspectioncount =1  and ins.InspectionType<>3 and v.PlateID not like '%测%'  and ins.StationID= " + StationID);
            sbPassRate.Append(" group by l.id,L.StationID  ");
            DataTable dt = await sql.Ado.ExecuteDataTableAsync(sbPassRate.ToString());
            return dt;

        }
        #endregion

        #region 保存初检合格率
        public async Task SaveFirstInspectionPassRate(FirstInspectionPassRateEntity entity)
        {
            if (entity.ID.IsNullOrZero())
            {
                entity.Create();
                await firstBase.InsertAsync(entity);
            }
            else
            {
                entity.Modify();
                await firstBase.UpdateNotNullAsync(entity);
            }
        }
        #endregion

    }
}
