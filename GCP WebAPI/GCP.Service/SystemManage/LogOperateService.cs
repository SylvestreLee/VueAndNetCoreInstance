using GCP.Entity.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.Repository;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GCP.Service.SystemManage
{
    public class LogOperateService
    {
        private RepositoryBase<SysLogOperateEntity> repositoryBase = new RepositoryBase<SysLogOperateEntity>();
        private CommonService commonService = new CommonService();
        private IFreeSql sql = Sql.GetFsql();

        #region 获取数据
        //public async Task<List<SysLogOperateEntity>> GetList(LogOperateListParam param)
        //{
        //    var expression = ListFilter(param);
        //    var list = await sql.Select<SysLogOperateEntity, PersonEntity>()
        //        .LeftJoin((s, p) => s.BaseCreatorId == p.ID)
        //        .Where(expression)
        //        .ToListAsync((s, p) => new SysLogOperateEntity()
        //        {
        //            IpAddress = s.IpAddress,
        //            BaseCreatorId = s.BaseCreatorId,
        //            BusinessType = s.BusinessType,
        //            ExecuteParam = s.ExecuteParam,
        //            ExecuteResult = s.ExecuteResult,
        //            ExecuteTime = s.ExecuteTime,
        //            ExecuteUrl = s.ExecuteUrl,
        //            ID = s.ID,
        //            IpLocation = s.IpLocation,
        //            LogStatus = s.LogStatus,
        //            LogType = s.LogType,
        //            Remark = s.Remark,
        //            Status = s.Status,
        //            UpdateTime = s.UpdateTime,
        //            PersonName = p.Name
        //        });
        //    return list;
        //}

        public async Task<List<SysLogOperateEntity>> GetPageList(LogOperateListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await sql.Select<SysLogOperateEntity, PersonEntity>()
                .LeftJoin((s, p) => s.BaseCreatorId == p.ID)
                .Where(expression)
                .Page(pagination.PageIndex, pagination.PageSize)
                .OrderByDescending((s, p) => s.ID)
                .Count(out long count)
                .ToListAsync((s, p) => new SysLogOperateEntity()
                {
                    IpAddress = s.IpAddress,
                    BaseCreatorId = s.BaseCreatorId,
                    BusinessType = s.BusinessType,
                    ExecuteParam = s.ExecuteParam,
                    ExecuteResult = s.ExecuteResult,
                    ExecuteTime = s.ExecuteTime,
                    ExecuteUrl = s.ExecuteUrl,
                    ID = s.ID,
                    IpLocation = s.IpLocation,
                    LogStatus = s.LogStatus,
                    LogType = s.LogType,
                    Remark = s.Remark,
                    Status = s.Status,
                    UpdateTime = s.UpdateTime,
                    PersonName = p.Name
                });
            pagination.TotalCount = count.ParseToInt();
            return list;
        }

        public async Task<SysLogOperateEntity> GetEntity(long id)
        {
            var entity = await sql.Select<SysLogOperateEntity, PersonEntity>()
                .LeftJoin((s, p) => s.BaseCreatorId == p.ID)
                .Where((s, p) => s.ID == id)
                .FirstAsync((s, p) => new SysLogOperateEntity()
                {
                    IpAddress = s.IpAddress,
                    BaseCreatorId = s.BaseCreatorId,
                    BusinessType = s.BusinessType,
                    ExecuteParam = s.ExecuteParam,
                    ExecuteResult = s.ExecuteResult,
                    ExecuteTime = s.ExecuteTime,
                    ExecuteUrl = s.ExecuteUrl,
                    ID = s.ID,
                    IpLocation = s.IpLocation,
                    LogStatus = s.LogStatus,
                    LogType = s.LogType,
                    Remark = s.Remark,
                    Status = s.Status,
                    UpdateTime = s.UpdateTime,
                    PersonName = p.Name
                });
            return entity;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(SysLogOperateEntity entity)
        {
            if (entity.ID.IsNullOrZero())
            {
                entity.Create();
                await repositoryBase.InsertAsync(entity);
            }
            else
            {
                await repositoryBase.UpdateAsync(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await repositoryBase.DeleteLogicallyAsync(idArr.ToArray());
        }

        public async Task RemoveAllForm()
        {
            await Sql.GetFsql()
                    .Update<SysLogOperateEntity>()
                    .Set(c => c.Status == 2)
                    .ExecuteAffrowsAsync();
        }
        #endregion

        #region 私有方法

        private Expression<Func<SysLogOperateEntity, PersonEntity, bool>> ListFilter(LogOperateListParam param)
        {
            Expression<Func<SysLogOperateEntity, PersonEntity, bool>> expression = ((s, p) => s.Status == 1);
            if (param != null)
            {
                if (param.StartTime != null)
                {
                    expression = expression.And((s, p) => s.UpdateTime >= param.StartTime.Value);
                }
                if (param.EndTime != null)
                {
                    expression = expression.And((s, p) => s.UpdateTime < param.EndTime.Value.AddDays(1));
                }
                if (param.UserName != null)
                {
                    expression = expression.And((s, p) => p.UserName.Contains(param.UserName) || p.Name.Contains(param.UserName));
                }
                if (param.ExecuteUrl != null)
                {
                    expression = expression.And((s, p) => s.ExecuteUrl.Contains(param.ExecuteUrl));
                }
                if (param.ExecuteParam != null)
                {
                    expression = expression.And((s, p) => s.ExecuteParam.Contains(param.ExecuteParam));
                }
            }
            return expression;
        }

        #endregion
    }
}
