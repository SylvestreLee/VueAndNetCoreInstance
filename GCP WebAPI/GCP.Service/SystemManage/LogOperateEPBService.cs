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
using GCP.Entity.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.Entity.EnumManage;

namespace GCP.Service.SystemManage
{
    /// <summary>
    /// 创 建：sht
    /// 日 期：2021-08-02 10:58
    /// 描 述：服务类
    /// </summary>
    public class LogOperateEPBService
    {
        private IFreeSql sql = Sql.GetFsql();

        #region 获取数据

        public async Task<List<LogOperateEPBEntity>> GetPageList(LogOperateEPBListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await sql.Select<SysLogOperateEntity, PersonEntity, EnumLogOperateEPBTypeEntity>()
                .LeftJoin((s, p, elo) => s.BaseCreatorId == p.ID)
                .InnerJoin((s, p, elo) => s.ExecuteUrl == elo.ExecuteUrl)
                .Where(expression)
                .Page(pagination.PageIndex, pagination.PageSize)
                .OrderByDescending((s, p, elo) => s.ID)
                .Count(out long count)
                .ToListAsync((s, p, elo) => new LogOperateEPBEntity()
                {
                    BaseCreatorId = s.BaseCreatorId,
                    MethodID = elo.ID,
                    ID = s.ID,
                    PersonName = p.Name,
                    ExecuteParam = s.ExecuteParam,
                    ExecuteUrl = s.ExecuteUrl,
                    MethodName = elo.Name
                });
            pagination.TotalCount = count.ParseToInt();
            return list;
        }

        #endregion

        #region 私有方法

        private Expression<Func<SysLogOperateEntity, PersonEntity, EnumLogOperateEPBTypeEntity, bool>> ListFilter(LogOperateEPBListParam param)
        {
            Expression<Func<SysLogOperateEntity, PersonEntity, EnumLogOperateEPBTypeEntity, bool>> expression = ((s, p, elo) => s.Status == 1);
            if (param != null)
            {
                if (param.StartTime.IsCorrectDate())
                {
                    expression = expression.And((s, p, elo) => s.UpdateTime >= param.StartTime.Value);
                }
                if (param.EndTime.IsCorrectDate())
                {
                    expression = expression.And((s, p, elo) => s.UpdateTime < param.EndTime.Value.AddDays(1));
                }
                if (param.UserName != null)
                {
                    expression = expression.And((s, p, elo) => p.UserName.Contains(param.UserName) || p.Name.Contains(param.UserName));
                }
                if (!param.TypeID.IsNullOrZero())
                {
                    expression = expression.And((s, p, elo) => elo.ID == param.TypeID.Value);
                }
            }
            return expression;
        }

        #endregion
    }
}
