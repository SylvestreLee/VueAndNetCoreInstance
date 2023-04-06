using GCP.Business.SystemManage;
using GCP.Code;
using GCP.Entity.SystemManage;
using GCP.Model.Result;
using GCP.Util.Model;
using GCP.WebAPI.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCP.WebAPI.Controllers.SystemManage
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MenuController : BaseController
    {
        private MenuBLL menuBLL = new MenuBLL();

        #region 获取数据

        ///// <summary>
        ///// 获取菜单列表
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[AuthorizeFilter]
        //public async Task<TData<List<SysMenuEntity>>> GetMenuList()
        //{
        //    TData<List<SysMenuEntity>> obj = new TData<List<SysMenuEntity>>();
        //    obj = await menuBLL.GetMenuList();
        //    return obj;
        //}

        /// <summary>
        /// 获取用户所属角色相关信息，【初始化】
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter]
        public async Task<TData<UserRoleInfo>> GetInfo()
        {
            return await menuBLL.GetInfo();
        }

        /// <summary>
        /// 获取路由菜单信息【初始化】
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter]
        public async Task<TData<List<UserMenuInfo>>> GetRouters()
        {
            return await menuBLL.GetRouters();
        }

        /// <summary>
        /// 获取菜单ElementTree结构【菜单查询】
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:menu:search")]
        public async Task<TData<List<ElementTreeInfo>>> GetEtreeList()
        {
            return await menuBLL.GetEtreeList();
        }

        /// <summary>
        /// 获取编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("system:menu:search")]
        public async Task<TData<SysMenuEntity>> GetFormJson([FromQuery] long id)
        {
            return await menuBLL.GetEntity(id);
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 提交和更新菜单数据
        /// </summary>
        /// <param name="entity">菜单实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("system:menu:add,system:menu:edit")]
        public async Task<ActionResult> SaveFormJson([FromBody] SysMenuEntity entity)
        {
            TData<string> obj = await menuBLL.SaveForm(entity);
            return Json(obj);
        }

        /// <summary>
        /// 根据多个id删除内容
        /// </summary>
        /// <param name="ids">多个id，英文逗号隔开</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("system:menu:delete")]
        public async Task<ActionResult> DeleteFormJson([FromBody] string ids)
        {
            TData obj = await menuBLL.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}
