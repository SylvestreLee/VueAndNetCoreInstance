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
    public class LogOperateBLL
    {
        private LogOperateService logOperateService = new LogOperateService();

        #region 获取数据

        //public async Task<TData<List<SysLogOperateEntity>>> GetList(LogOperateListParam param)
        //{
        //    TData<List<SysLogOperateEntity>> obj = new TData<List<SysLogOperateEntity>>();
        //    obj.Data = await logOperateService.GetList(param);
        //    obj.Total = obj.Data.Count;
        //    obj.Tag = 1;
        //    return obj;
        //}

        public async Task<TData<List<SysLogOperateEntity>>> GetPageList(LogOperateListParam param, Pagination pagination)
        {
            TData<List<SysLogOperateEntity>> obj = new TData<List<SysLogOperateEntity>>();
            obj.Data = await logOperateService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<SysLogOperateEntity>> GetEntity(long id)
        {
            TData<SysLogOperateEntity> obj = new TData<SysLogOperateEntity>();
            obj.Data = await logOperateService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(SysLogOperateEntity entity)
        {
            TData<string> obj = new TData<string>();
            await logOperateService.SaveForm(entity);
            obj.Data = entity.ID.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<string>> SaveForm(string remark)
        {
            TData<string> obj = new TData<string>();
            SysLogOperateEntity entity = new SysLogOperateEntity();
            await logOperateService.SaveForm(entity);
            entity.LogStatus = OperateStatusEnum.Success.ParseToInt();
            entity.ExecuteUrl = remark;
            obj.Data = entity.ID.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await logOperateService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> RemoveAllForm()
        {
            TData obj = new TData();
            await logOperateService.RemoveAllForm();
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}
