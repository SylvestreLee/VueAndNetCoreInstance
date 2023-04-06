using FreeSql.DatabaseModel;
using GCP.Business.SystemManage;
using GCP.Code;
using GCP.CodeGenerator.Model;
using GCP.CodeGenerator.Template;
using GCP.Entity;
using GCP.Model.Result.SystemManage;
using GCP.Util;
using GCP.Util.Model;
using GCP.WebAPI.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GCP.WebAPI.Controllers.SystemManage
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DbFirstController : BaseController
    {
        private DbFirstBLL dbFirstBLL = new DbFirstBLL();
        private DatabaseTableBLL databaseTableBLL = new DatabaseTableBLL();

        /// <summary>
        /// 【代码生成器】获取表信息
        /// </summary>
        /// <param name="ManageName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("管理员")]
        public async Task<TData<string>> GetTable([FromQuery]string ManageName, [FromQuery] string TableName)
        {
            return await dbFirstBLL.GetTableStruct(ManageName, TableName);
        }

        /// <summary>
        /// 【代码生成器】获取表列表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("管理员")]
        public async Task<IActionResult> GetTableListJson([FromQuery] string tableName)
        {
            TData<List<TableInfo>> obj = await databaseTableBLL.GetTableList(tableName);
            return Json(obj);
        }

        /// <summary>
        /// 【代码生成器】获取表分页
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter("管理员")]
        public async Task<IActionResult> GetTablePageListJson([FromQuery] string tableName, [FromQuery] Pagination pagination)
        {
            TData<List<TableInfo>> obj = await databaseTableBLL.GetTablePageList(tableName, pagination);
            return Json(obj);
        }

        //[HttpGet]
        //[AuthorizeFilter("管理员")]
        private async Task<TData<BaseConfigModel>> GetBaseConfigJson([FromQuery]string tableName)
        {
            TData<BaseConfigModel> obj = new TData<BaseConfigModel>();

            string tableDescription = string.Empty;
            TData<List<DbColumnInfo>> tDataTableField = await databaseTableBLL.GetTableFieldList(tableName);
            List<string> columnList = tDataTableField.Data.Where(p => !BaseField.BaseFieldList.Contains(p.Name.ToUpper())).Select(p => p.Name).ToList();

            OperatorInfo operatorInfo = await Operator.Instance.Current();
            string serverPath = GlobalContext.HostingEnvironment.ContentRootPath;
            obj.Data = new SingleTableTemplate().GetBaseConfig(serverPath, operatorInfo.UserName, tableName, tableDescription, columnList);
            obj.Tag = 1;
            return obj;
        }

        //[HttpPost]
        //[AuthorizeFilter("管理员")]
        private async Task<TData<CodeModel>> CodePreviewJson([FromBody]BaseConfigModel baseConfig)
        {
            TData<CodeModel> obj = new TData<CodeModel>();
            if (string.IsNullOrEmpty(baseConfig.OutputConfig.OutputModule))
            {
                obj.Message = "请选择输出到的模块";
                return obj;
            }
            else if (string.IsNullOrEmpty(baseConfig.MenuName))
            {
                obj.Message = "请输入菜单名称";
                return obj;
            }
            else
            {
                SingleTableTemplate template = new SingleTableTemplate();
                TData<List<DbColumnInfo>> objTable = await databaseTableBLL.GetTableFieldList(baseConfig.TableName);
                DataTable dt = DataTableHelper.ListToDataTable(objTable.Data);  // 用DataTable类型，避免依赖
                string codeEntity = template.BuildEntity(baseConfig, dt);
                string codeEntityParam = template.BuildEntityParam(baseConfig);
                string codeService = template.BuildService(baseConfig, dt);
                string codeBusiness = template.BuildBusiness(baseConfig);
                string codeController = template.BuildController(baseConfig);
                string codeMenu = template.BuildMenu(baseConfig);

                var json = new CodeModel
                {
                    CodeEntity = HttpUtility.HtmlEncode(codeEntity),
                    CodeEntityParam = HttpUtility.HtmlEncode(codeEntityParam),
                    CodeService = HttpUtility.HtmlEncode(codeService),
                    CodeBusiness = HttpUtility.HtmlEncode(codeBusiness),
                    CodeController = HttpUtility.HtmlEncode(codeController),
                    CodeMenu = HttpUtility.HtmlEncode(codeMenu)
                };
                obj.Data = json;
                obj.Tag = 1;
            }
            return obj;
        }

        //[HttpPost]
        //[AuthorizeFilter("管理员")]
        private async Task<TData<CodeModel>> EntityPreviewJson([FromBody] BaseConfigModel baseConfig)
        {
            TData<CodeModel> obj = new TData<CodeModel>();
            if (string.IsNullOrEmpty(baseConfig.OutputConfig.OutputModule))
            {
                obj.Message = "请选择输出到的模块";
                return obj;
            }
            else
            {
                SingleTableTemplate template = new SingleTableTemplate();
                TData<List<DbColumnInfo>> objTable = await databaseTableBLL.GetTableFieldList(baseConfig.TableName);
                DataTable dt = DataTableHelper.ListToDataTable(objTable.Data);  // 用DataTable类型，避免依赖
                string codeEntity = template.BuildEntity(baseConfig, dt);

                var json = new CodeModel
                {
                    CodeEntity = HttpUtility.HtmlEncode(codeEntity),
                    CodeEntityParam = null,
                    CodeService = null,
                    CodeBusiness = null,
                    CodeController = null,
                    CodeMenu = null
                };
                obj.Data = json;
                obj.Tag = 1;
            }
            return obj;
        }

        //[HttpPost]
        //[AuthorizeFilter("管理员")]
        private async Task<IActionResult> CodeGenerateJson([FromBody] BaseConfigModel baseConfig)
        {
            TData<List<KeyValue>> obj = new TData<List<KeyValue>>();
            if (!GlobalContext.SystemConfig.Debug)
            {
                obj.Message = "请在本地开发模式时使用代码生成";
            }
            else
            {
                var ret = await CodePreviewJson(baseConfig);
                var Code = ((TData<CodeModel>)ret).Data;
                SingleTableTemplate template = new SingleTableTemplate();
                List<KeyValue> result = await template.CreateCode(baseConfig, Code);
                obj.Data = result;
                obj.Tag = 1;
            }
            return Json(obj);
        }

        //[HttpPost]
        //[AuthorizeFilter("管理员")]
        private async Task<IActionResult> EntityGenerateJson([FromBody] BaseConfigModel baseConfig)
        {
            TData<List<KeyValue>> obj = new TData<List<KeyValue>>();
            if (!GlobalContext.SystemConfig.Debug)
            {
                obj.Message = "请在本地开发模式时使用代码生成";
            }
            else
            {
                var ret = await EntityPreviewJson(baseConfig);
                var Code = ((TData<CodeModel>)ret).Data;
                SingleTableTemplate template = new SingleTableTemplate();
                List<KeyValue> result = await template.CreateEntity(baseConfig, Code);
                obj.Data = result;
                obj.Tag = 1;
            }
            return Json(obj);
        }

        /// <summary>
        /// 【代码生成器】根据表名称自动生成代码
        /// </summary>
        /// <param name="TableName">数据库表名称</param>
        /// <param name="MenuName">前台显示的菜单名称</param>
        /// <param name="OutputModule">输出到的文件夹，后缀一定要有Manage</param>
        /// <param name="BtnList">需要自动生成的按钮权限，枚举值参考GCEnum.CommonEnum.OperateButtonList</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("管理员")]
        public async Task<IActionResult> CodeAutoGenerateJson([FromQuery] string TableName, [FromQuery] string MenuName, [FromQuery] string OutputModule, [FromQuery] List<int> BtnList)
        {
            TData obj = new TData();
            try
            {
                if (!GlobalContext.SystemConfig.Debug)
                {
                    obj.Message = "请在本地开发模式时使用代码生成";
                }
                else
                {
                    var ret = await GetBaseConfigJson(TableName);
                    var baseConfig = ret.Data;
                    baseConfig.OutputConfig.OutputModule = OutputModule ?? "TestManage";
                    baseConfig.BtnList = BtnList ?? new List<int>();
                    baseConfig.MenuName = MenuName ?? "[菜单名称]";
                    var robj = await CodeGenerateJson(baseConfig);
                    return Json(robj);
                }
            }
            catch (Exception err)
            {
                obj.Message = err.Message;
            }
            return Json(obj);
        }

        /// <summary>
        /// 【代码生成器】根据类名称自动生成不操作数据库的代码
        /// </summary>
        /// <param name="ClassName">类名称</param>
        /// <param name="MenuName">前台显示的菜单名称</param>
        /// <param name="OutputModule">输出到的文件夹，后缀一定要有Manage</param>
        /// <param name="BtnList">需要自动生成的按钮权限，枚举值参考GCEnum.CommonEnum.OperateButtonList</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("管理员")]
        public async Task<IActionResult> EmptyCodeAutoGenerateJson([FromQuery] string ClassName, [FromQuery] string MenuName, [FromQuery] string OutputModule, [FromQuery] List<int> BtnList)
        {
            TData obj = new TData();
            try
            {
                var ret = await GetBaseConfigJson(ClassName);
                var baseConfig = ret.Data;
                baseConfig.OutputConfig.OutputModule = OutputModule ?? "TestManage";
                baseConfig.BtnList = BtnList ?? new List<int>();
                baseConfig.MenuName = MenuName ?? "[菜单名称]";
                var robj = await EmptyCodeGenerateJson(baseConfig);
                return Json(robj);
            }
            catch (Exception err)
            {
                obj.Message = err.Message;
            }
            return Json(obj);
        }

        private async Task<TData<CodeModel>> EmptyCodePreviewJson([FromBody] BaseConfigModel baseConfig)
        {
            TData<CodeModel> obj = new TData<CodeModel>();
            if (string.IsNullOrEmpty(baseConfig.OutputConfig.OutputModule))
            {
                obj.Message = "请选择输出到的模块";
                return obj;
            }
            else if (string.IsNullOrEmpty(baseConfig.MenuName))
            {
                obj.Message = "请输入菜单名称";
                return obj;
            }
            else
            {
                SingleTableTemplate template = new SingleTableTemplate();
                TData<List<DbColumnInfo>> objTable = await databaseTableBLL.GetTableFieldList(baseConfig.TableName);
                //DataTable dt = DataTableHelper.ListToDataTable(objTable.Data);  // 用DataTable类型，避免依赖
                string codeEntity = template.BuildEmptyEntity(baseConfig);
                string codeEntityParam = template.BuildEmptyEntityParam(baseConfig);
                string codeService = template.BuildEmptyService(baseConfig);
                string codeBusiness = template.BuildEmptyBusiness(baseConfig);
                string codeController = template.BuildEmptyController(baseConfig);
                string codeMenu = template.BuildMenu(baseConfig);

                var json = new CodeModel
                {
                    CodeEntity = HttpUtility.HtmlEncode(codeEntity),
                    CodeEntityParam = HttpUtility.HtmlEncode(codeEntityParam),
                    CodeService = HttpUtility.HtmlEncode(codeService),
                    CodeBusiness = HttpUtility.HtmlEncode(codeBusiness),
                    CodeController = HttpUtility.HtmlEncode(codeController),
                    CodeMenu = HttpUtility.HtmlEncode(codeMenu)
                };
                obj.Data = json;
                obj.Tag = 1;
            }
            return obj;
        }

        private async Task<IActionResult> EmptyCodeGenerateJson([FromBody] BaseConfigModel baseConfig)
        {
            TData<List<KeyValue>> obj = new TData<List<KeyValue>>();
            if (!GlobalContext.SystemConfig.Debug)
            {
                obj.Message = "请在本地开发模式时使用代码生成";
            }
            else
            {
                var ret = await EmptyCodePreviewJson(baseConfig); ;
                var Code = ((TData<CodeModel>)ret).Data;
                SingleTableTemplate template = new SingleTableTemplate();
                List<KeyValue> result = await template.CreateCode(baseConfig, Code);
                obj.Data = result;
                obj.Tag = 1;
            }
            return Json(obj);
        }

        /// <summary>
        /// 【代码生成器】根据表名称自动生成/更新实体类
        /// </summary>
        /// <param name="TableName">数据库表名称</param>
        /// <param name="OutputModule">输出到的文件夹，后缀一定要有Manage</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("管理员")]
        public async Task<IActionResult> EntityGenerateJson([FromQuery] string TableName, [FromQuery] string OutputModule)
        {
            TData obj = new TData();
            try
            {
                if (!GlobalContext.SystemConfig.Debug)
                {
                    obj.Message = "请在本地开发模式时使用代码生成";
                }
                else
                {
                    var ret = await GetBaseConfigJson(TableName);
                    var baseConfig = ret.Data;
                    baseConfig.OutputConfig.OutputModule = OutputModule ?? "TestManage";
                    var robj = await EntityGenerateJson(baseConfig);
                    return Json(robj);
                }
            }
            catch (Exception err)
            {
                obj.Message = err.Message;
            }
            return Json(obj);
        }
    }
}
