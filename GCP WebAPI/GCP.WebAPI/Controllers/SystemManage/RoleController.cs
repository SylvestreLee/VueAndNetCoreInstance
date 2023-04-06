using GCP.Business.SystemManage;
using GCP.Entity.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.Util.Model;
using GCP.WebAPI.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GCP.Model.CommonModel;

namespace GCP.WebAPI.Controllers.SystemManage
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private RoleBLL roleBLL = new RoleBLL();

        #region 获取数据

        /// <summary>
        /// 获取监管平台角色下拉枚举
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter]
        public async Task<TData<List<EnumIdNameModel>>> GetEnum()
        {
            return await roleBLL.GetEnum();
        }


        /// <summary>
        /// 获取登录系统角色下拉枚举
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter]
        public async Task<TData<List<EnumIdNameModel>>> GetLoginEnum()
        {
            return await roleBLL.GetLoginEnum();
        }

        /// <summary>
        /// 获取监管平台角色列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:role:search,system:user:search")]
        public async Task<IActionResult> GetListJson([FromQuery] RoleListParam param)
        {
            TData<List<SysRoleEntity>> obj = await roleBLL.GetList(param);
            return Json(obj);
        }


        /// <summary>
        /// 获取登录系统角色列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:role:search,system:user:search")]
        public async Task<IActionResult> GetLoginListJson([FromQuery] RoleListParam param)
        {
            TData<List<SysRoleEntity>> obj = await roleBLL.GetLoginList(param);
            return Json(obj);
        }

        /// <summary>
        /// 获取监管平台角色分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:role:search,system:user:search")]
        public async Task<IActionResult> GetPageListJson([FromQuery] RoleListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<SysRoleEntity>> obj = await roleBLL.GetPageList(param, pagination);
            return Json(obj);
        }


        /// <summary>
        /// 获取登录系统角色分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:role:search,system:user:search")]
        public async Task<IActionResult> GetLoginPageListJson([FromQuery] RoleListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<SysRoleEntity>> obj = await roleBLL.GetLoginPageList(param, pagination);
            return Json(obj);
        }


        /// <summary>
        /// 根据ID获取单个角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:role:search,system:user:search")]
        public async Task<IActionResult> GetFormJson([FromQuery] long id)
        {
            TData<SysRoleEntity> obj = await roleBLL.GetEntity(id);
            return Json(obj);
        }

        /// <summary>
        /// 根据多个ID，返回角色名称
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:role:search")]
        public async Task<IActionResult> GetRoleName([FromQuery] RoleListParam param)
        {
            TData<string> obj = new TData<string>();
            var list = await roleBLL.GetList(param);
            if (list.Tag == 1)
            {
                obj.Data = string.Join(",", list.Data.Select(p => p.RoleName));
                obj.Tag = 1;
            }
            return Json(obj);
        }

        /// <summary>
        /// 获取最大的角色排序，用来默认显示在新增页面上
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:role:search")]
        public async Task<IActionResult> GetMaxSortJson()
        {
            TData<long> obj = await roleBLL.GetMaxSort();
            return Json(obj);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 提交监管平台角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("system:role:add,system:role:edit")]
        public async Task<IActionResult> SaveFormJson([FromBody] SysRoleEntity entity)
        {
            TData<string> obj = await roleBLL.SaveForm(entity);
            return Json(obj);
        }


        /// <summary>
        /// 提交登录系统角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("system:role:add,system:role:edit")]
        public async Task<IActionResult> SaveLoginFormJson([FromBody] SysRoleEntity entity)
        {
            TData<string> obj = await roleBLL.SaveLoginForm(entity);
            return Json(obj);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("system:role:delete")]
        public async Task<IActionResult> DeleteFormJson([FromBody] string ids)
        {
            TData obj = await roleBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
