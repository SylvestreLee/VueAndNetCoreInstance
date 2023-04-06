using FreeSql.DatabaseModel;
using GCP.Repository;
using GCP.Util.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using GCP.Service.SystemManage;
using GCP.Entity.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.Enum;
using GCP.Util.Extension;

namespace GCP.Business.SystemManage
{
    public class LogLoginBLL
    {
        private LogLoginService logLoginService = new LogLoginService();

        #region 获取数据
        public async Task<TData<List<SysLogLoginEntity>>> GetList(LogLoginListParam param)
        {
            TData<List<SysLogLoginEntity>> obj = new TData<List<SysLogLoginEntity>>();
            obj.Data = await logLoginService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<SysLogLoginEntity>>> GetPageList(LogLoginListParam param, Pagination pagination)
        {
            TData<List<SysLogLoginEntity>> obj = new TData<List<SysLogLoginEntity>>();
            obj.Data = await logLoginService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<SysLogLoginEntity>> GetEntity(long id)
        {
            TData<SysLogLoginEntity> obj = new TData<SysLogLoginEntity>();
            obj.Data = await logLoginService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(SysLogLoginEntity entity)
        {
            TData<string> obj = new TData<string>();
            await logLoginService.SaveForm(entity);
            obj.Data = entity.ID.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await logLoginService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}
