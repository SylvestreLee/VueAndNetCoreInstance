using GCP.CodeGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using Newtonsoft.Json.Linq;
using GCP.Entity;
using GCP.Util.Extension;
using GCP.Util.Model;
using GCP.Util;
using GCP.Entity.SystemManage;
using GCP.Enum.SystemManage;
using GCP.Enum;
using GCP.Service.SystemManage;
using GCP.Business.Cache;
using GCP.Repository;
using FreeSql.DatabaseModel;

namespace GCP.CodeGenerator.Template
{
    public class SingleTableTemplate
    {
        private MenuService menuService = new MenuService();

        #region GetBaseConfig
        public BaseConfigModel GetBaseConfig(string path, string userName, string tableName, string tableDescription, List<string> tableFieldList)
        {
            path = GetProjectRootPath(path);

            int defaultField = 2; // 默认显示2个字段

            BaseConfigModel baseConfigModel = new BaseConfigModel();
            baseConfigModel.TableName = tableName;
            baseConfigModel.TableNameUpper = tableName;

            #region FileConfigModel
            baseConfigModel.FileConfig = new FileConfigModel();
            baseConfigModel.FileConfig.ClassPrefix = TableMappingHelper.GetClassNamePrefix(tableName);
            baseConfigModel.FileConfig.ClassDescription = tableDescription;
            baseConfigModel.FileConfig.CreateName = userName;
            baseConfigModel.FileConfig.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            baseConfigModel.FileConfig.EntityName = string.Format("{0}Entity", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.EntityMapName = string.Format("{0}Map", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.EntityParamName = string.Format("{0}Param", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.BusinessName = string.Format("{0}BLL", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.ServiceName = string.Format("{0}Service", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.ControllerName = string.Format("{0}Controller", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.PageName = baseConfigModel.FileConfig.ClassPrefix;

            #endregion

            #region OutputConfigModel          
            baseConfigModel.OutputConfig = new OutputConfigModel();
            baseConfigModel.OutputConfig.OutputModule = string.Empty;
            baseConfigModel.OutputConfig.OutputEntity = Path.Combine(path, "GCP.Entity");
            baseConfigModel.OutputConfig.OutputBusiness = Path.Combine(path, "GCP.Business");
            baseConfigModel.OutputConfig.OutputWeb = Path.Combine(path, "GCP.WebAPI");
            baseConfigModel.OutputConfig.Output = path;
            string ManageRoot = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Controllers");
            if (Directory.Exists(ManageRoot))
            {
                baseConfigModel.OutputConfig.ModuleList = Directory.GetDirectories(ManageRoot).Select(p => Path.GetFileName(p)).Where(p => p != "DemoManage").ToList();
            }
            else
            {
                baseConfigModel.OutputConfig.ModuleList = new List<string> { "TestManage" };
            }
            #endregion

            #region columns

            baseConfigModel.Columns = tableFieldList;

            #endregion

            return baseConfigModel;
        }
        #endregion

        #region BuildEntity

        public string BuildEntity(BaseConfigModel baseConfigModel, DataTable dt)
        {
            TData<string> obj = new TData<string>();
            StringBuilder SbText = new StringBuilder();

            var Fsql = Sql.GetFsql();
            var Tables = (Fsql.DbFirst.GetTablesByDatabase(GlobalContext.SystemConfig.DBFirstDatabase).Where(t => t.Type == DbTableType.TABLE && t.Name.ToUpper() == baseConfigModel.TableName.ToUpper())).ToList();
            if (Tables.Count > 0)
            {
                var t = Tables[0];
                SbText.AppendLine("using System;");
                SbText.AppendLine("using System.Collections.Generic;");
                SbText.AppendLine("using Newtonsoft.Json;");
                SbText.AppendLine("using FreeSql.DataAnnotations;");
                SbText.AppendLine("using System.ComponentModel;");
                SbText.AppendLine();
                SbText.AppendLine("namespace GCP.Entity." + baseConfigModel.OutputConfig.OutputModule);
                SbText.AppendLine("{");
                SbText.AppendLine("    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = \"" + t.Name + "\")]");
                SbText.AppendLine("    public partial class " + baseConfigModel.FileConfig.EntityName + " : " + GetBaseEntity(dt));
                SbText.AppendLine("    {");

                var times = 0;
                var TableStruct = Tables[0];
                foreach (var column in TableStruct.Columns)
                {
                    if (!BaseField.BaseFieldList.Contains(column.Name.ToUpper()))
                    {
                        if (times > 0)
                        {
                            SbText.AppendLine("        ");
                        }
                        SbText.AppendLine("        /// <summary> ");
                        SbText.AppendLine("        /// ");
                        SbText.AppendLine("        /// </summary> ");
                        SbText.AppendLine("        [Description(\"\")]");
                        SbText.Append("        [JsonProperty, Column(Name = \"" + column.Name + "\"");
                        if (column.CsType == typeof(string))
                        {
                            SbText.Append(", StringLength = " + column.MaxLength + "");
                        }
                        if (column.CsType == typeof(string) && !column.IsNullable)
                        {
                            SbText.Append(", IsNullable = false");
                        }
                        SbText.Append(", DbType = \"" + column.DbTypeTextFull + "\"");
                        if (column.IsPrimary)
                        {
                            //SbText.Append(",IsPrimary = true");
                            //SbText = SbText.Replace("BaseEntity", "BaseNoPrimaryEntity");
                            //SbText = SbText.Replace("BaseIDEntity", "BaseNotPrimaryIDEntity");
                        }
                        if (column.IsIdentity)
                        {
                            SbText.Append(", IsIdentity = true");
                        }
                        SbText.Append(")]");
                        SbText.AppendLine();

                        SbText.Append("        public " + column.CsType.FullName);
                        if (column.IsNullable)
                        {
                            SbText.Append("?");
                        }
                        SbText.Append(" " + column.Name + " { get; set; }");
                        SbText.AppendLine();
                        times++;
                    }
                }

                SbText.AppendLine("    }");
                SbText.AppendLine("}");
            }
            return SbText.ToString();
        }

        public string BuildEmptyEntity(BaseConfigModel baseConfigModel)
        {
            TData<string> obj = new TData<string>();
            StringBuilder SbText = new StringBuilder();

            SbText.AppendLine("using System;");
            SbText.AppendLine("using System.Collections.Generic;");
            SbText.AppendLine("using Newtonsoft.Json;");
            SbText.AppendLine("using FreeSql.DataAnnotations;");
            SbText.AppendLine();
            SbText.AppendLine("namespace GCP.Entity." + baseConfigModel.OutputConfig.OutputModule);
            SbText.AppendLine("{");
            SbText.AppendLine("    public partial class " + baseConfigModel.FileConfig.EntityName);
            SbText.AppendLine("    {");
            SbText.AppendLine("    ");
            SbText.AppendLine("    }");
            SbText.AppendLine("}");
            return SbText.ToString();
        }

        #endregion

        #region BuildEntityParam

        public string BuildEntityParam(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using Newtonsoft.Json;");
            sb.AppendLine();

            sb.AppendLine("namespace GCP.Model.Param." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("实体查询类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam"));
            sb.AppendLine("    {");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        public string BuildEmptyEntityParam(BaseConfigModel baseConfigModel)
        {
            return BuildEntityParam(baseConfigModel);
        }

        #endregion

        #region BuildService

        public string BuildService(BaseConfigModel baseConfigModel, DataTable dt)
        {
            string baseEntity = GetBaseEntity(dt);

            StringBuilder sb = new StringBuilder();
            string method = string.Empty;
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Data.Common;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using FreeSql.DatabaseModel;");
            sb.AppendLine("using GCP.Util;");
            sb.AppendLine("using GCP.Util.Extension;");
            sb.AppendLine("using GCP.Util.Model;");
            sb.AppendLine("using GCP.Repository;");
            sb.AppendLine("using GCP.Entity." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.Model.Param." + baseConfigModel.OutputConfig.OutputModule + ";");
            //sb.AppendLine("using GCP.Model.Result." + baseConfigModel.OutputConfig.OutputModule + ";");

            sb.AppendLine();

            sb.AppendLine("namespace GCP.Service." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("服务类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.ServiceName);
            sb.AppendLine("    {");
            sb.AppendLine("        ");
            sb.AppendLine("        private RepositoryBase<" + baseConfigModel.FileConfig.EntityName + "> repositoryBase = new RepositoryBase<" + baseConfigModel.FileConfig.EntityName + ">();");
            sb.AppendLine("        ");
            sb.AppendLine("        #region 获取数据");
            sb.AppendLine("        ");
            sb.AppendLine("        public async Task<List<" + baseConfigModel.FileConfig.EntityName + ">> GetList(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param)");
            sb.AppendLine("        {");
            sb.AppendLine("            var expression = ListFilter(param);");
            sb.AppendLine("            var list = await repositoryBase");
            sb.AppendLine("                            .Where(expression)");
            sb.AppendLine("                            .ToListAsync();");
            sb.AppendLine("            return list;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<List<" + baseConfigModel.FileConfig.EntityName + ">> GetPageList(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param, Pagination pagination)");
            sb.AppendLine("        {");
            sb.AppendLine("            var expression = ListFilter(param);");
            sb.AppendLine("            var Queryable = repositoryBase");
            sb.AppendLine("                            .Where(expression)");
            sb.AppendLine("                            .OrderBy(pagination.Sort + \" \" + pagination.SortType + \" \");");
            sb.AppendLine("            pagination.TotalCount = (await Queryable.CountAsync()).ParseToInt();");
            sb.AppendLine("            var list = await Queryable");
            sb.AppendLine("                            .Page(pagination.PageIndex, pagination.PageSize)");
            sb.AppendLine("                            .ToListAsync();");
            sb.AppendLine("            return list;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<" + baseConfigModel.FileConfig.EntityName + "> GetEntity(long id)");
            sb.AppendLine("        {");
            sb.AppendLine("            return await repositoryBase.FindAsync(id);");
            sb.AppendLine("            //当主键不是ID的时候使用下面这句话");
            sb.AppendLine("            //return await repositoryBase.Where(ex => ex.ID == id).FirstAsync();");
            sb.AppendLine("        }");
            sb.AppendLine("        ");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 提交数据");
            sb.AppendLine("        ");
            sb.AppendLine("        public async Task SaveForm(" + baseConfigModel.FileConfig.EntityName + " entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (entity.ID.IsNullOrZero())");
            sb.AppendLine("            {");
            sb.AppendLine("                " + GetSaveFormCreate(baseEntity));
            sb.AppendLine("                await repositoryBase.InsertAsync(entity);");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                " + GetSaveFormModify(baseEntity));
            sb.AppendLine("                await repositoryBase.UpdateNotNullAsync(entity);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("        ");
            sb.AppendLine("        public async Task DeleteForm(string ids)");
            sb.AppendLine("        {");
            sb.AppendLine("            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');");
            sb.AppendLine("            await repositoryBase.DeleteLogicallyAsync(idArr.ToArray());");
            sb.AppendLine("        }");
            sb.AppendLine("        ");
            sb.AppendLine("        #endregion");
            sb.AppendLine("        ");
            sb.AppendLine("        #region 私有方法");
            sb.AppendLine("        ");
            sb.AppendLine("        private Expression<Func<" + baseConfigModel.FileConfig.EntityName + ", bool>> ListFilter(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param)");
            sb.AppendLine("        {");
            sb.AppendLine("            //var expression = LinqExtensions.True<" + baseConfigModel.FileConfig.EntityName + ">();");
            sb.AppendLine("            Expression<Func<" + baseConfigModel.FileConfig.EntityName + ", bool>> expression = (ex => ex.Status != 2);");
            sb.AppendLine("            if (param != null)");
            sb.AppendLine("            {");
            sb.AppendLine("            }");
            sb.AppendLine("            return expression;");
            sb.AppendLine("        }");
            sb.AppendLine("        ");
            sb.AppendLine("        #endregion");

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        public string BuildEmptyService(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            string method = string.Empty;
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Data.Common;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using FreeSql.DatabaseModel;");
            sb.AppendLine("using GCP.Util;");
            sb.AppendLine("using GCP.Util.Extension;");
            sb.AppendLine("using GCP.Util.Model;");
            sb.AppendLine("using GCP.Repository;");
            sb.AppendLine("using GCP.Entity." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.Model.Param." + baseConfigModel.OutputConfig.OutputModule + ";");
            //sb.AppendLine("using GCP.Model.Result." + baseConfigModel.OutputConfig.OutputModule + ";");

            sb.AppendLine();

            sb.AppendLine("namespace GCP.Service." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("服务类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.ServiceName);
            sb.AppendLine("    {");
            sb.AppendLine("        ");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region BuildBusiness

        public string BuildBusiness(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using GCP.Util;");
            sb.AppendLine("using GCP.Util.Extension;");
            sb.AppendLine("using GCP.Util.Model;");
            sb.AppendLine("using GCP.Entity." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.Model.Param." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.Service." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using FreeSql.DatabaseModel;");
            sb.AppendLine("using GCP.Business;");
            sb.AppendLine();

            sb.AppendLine("namespace GCP.Business." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("业务类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.BusinessName);
            sb.AppendLine("    {");

            sb.AppendLine("        private CommonBLL commonBLL = new CommonBLL(); ");
            sb.AppendLine("        private " + baseConfigModel.FileConfig.ServiceName + " " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + " = new " + baseConfigModel.FileConfig.ServiceName + "();");
            sb.AppendLine("        ");
            sb.AppendLine("        #region 获取数据");
            sb.AppendLine("        ");
            sb.AppendLine("        public async Task<TData<List<" + baseConfigModel.FileConfig.EntityName + ">>> GetList(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<List<" + baseConfigModel.FileConfig.EntityName + ">> obj = new TData<List<" + baseConfigModel.FileConfig.EntityName + ">>();");
            sb.AppendLine("            obj.Data = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + ".GetList(param);");
            sb.AppendLine("            obj.Data = await SetJoinTable(obj.Data);");
            sb.AppendLine("            obj.Total = obj.Data.Count;");
            sb.AppendLine("            obj.Tag = 1;");
            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<TData<List<" + baseConfigModel.FileConfig.EntityName + ">>> GetPageList(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param, Pagination pagination)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<List<" + baseConfigModel.FileConfig.EntityName + ">> obj = new TData<List<" + baseConfigModel.FileConfig.EntityName + ">>();");
            sb.AppendLine("            obj.Data = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + ".GetPageList(param, pagination);");
            sb.AppendLine("            obj.Data = await SetJoinTable(obj.Data);");
            sb.AppendLine("            obj.Total = pagination.TotalCount;");
            sb.AppendLine("            obj.Tag = 1;");
            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<TData<" + baseConfigModel.FileConfig.EntityName + ">> GetEntity(long id)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<" + baseConfigModel.FileConfig.EntityName + "> obj = new TData<" + baseConfigModel.FileConfig.EntityName + ">();");
            sb.AppendLine("            obj.Data = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + ".GetEntity(id);");
            sb.AppendLine("            obj.Data = await SetJoinTable(obj.Data);");
            sb.AppendLine("            if (obj.Data != null)");
            sb.AppendLine("            {");
            sb.AppendLine("                obj.Tag = 1;");
            sb.AppendLine("            }");
            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine("        ");
            sb.AppendLine("        #endregion");
            sb.AppendLine("        ");
            sb.AppendLine("        #region 提交数据");
            sb.AppendLine("        ");
            sb.AppendLine("        public async Task<TData<string>> SaveForm(" + baseConfigModel.FileConfig.EntityName + " entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<string> obj = new TData<string>();");
            sb.AppendLine("            await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + ".SaveForm(entity);");
            sb.AppendLine("            obj.Data = entity.ID.ParseToString();");
            sb.AppendLine("            obj.Tag = 1;");
            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<TData> DeleteForm(string ids)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData obj = new TData();");
            sb.AppendLine("            await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + ".DeleteForm(ids);");
            sb.AppendLine("            obj.Tag = 1;");
            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine("        ");
            sb.AppendLine("        #endregion");
            sb.AppendLine("        ");
            sb.AppendLine("        #region 私有方法");
            sb.AppendLine("        ");
            sb.AppendLine("        public async Task<List<" + baseConfigModel.FileConfig.EntityName + ">> SetJoinTable(List<" + baseConfigModel.FileConfig.EntityName + "> entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            //entity = await commonBLL.SetJoinTable(entity, t => t.枚举字段, t => t.未映射的枚举赋值字段, \"枚举表名称\");");
            sb.AppendLine("            return entity;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<" + baseConfigModel.FileConfig.EntityName + "> SetJoinTable(" + baseConfigModel.FileConfig.EntityName + " entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            //entity = await commonBLL.SetJoinTable(entity, t => t.枚举字段, t => t.未映射的枚举赋值字段, \"枚举表名称\");");
            sb.AppendLine("            return entity;");
            sb.AppendLine("        }");
            sb.AppendLine("        ");
            sb.AppendLine("        #endregion");

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        public string BuildEmptyBusiness(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using GCP.Util;");
            sb.AppendLine("using GCP.Util.Extension;");
            sb.AppendLine("using GCP.Util.Model;");
            sb.AppendLine("using GCP.Entity." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.Model.Param." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.Service." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using FreeSql.DatabaseModel;");
            sb.AppendLine("using GCP.Business;");
            sb.AppendLine();

            sb.AppendLine("namespace GCP.Business." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("业务类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.BusinessName);
            sb.AppendLine("    {");

            sb.AppendLine("        private CommonBLL commonBLL = new CommonBLL(); ");
            sb.AppendLine("        private " + baseConfigModel.FileConfig.ServiceName + " " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + " = new " + baseConfigModel.FileConfig.ServiceName + "();");
            sb.AppendLine("        ");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region BuildController

        public string BuildController(BaseConfigModel baseConfigModel)
        {
            string modulePrefix = GetModulePrefix(baseConfigModel);
            string classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Web;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using GCP.Util;");
            sb.AppendLine("using GCP.Util.Model;");
            sb.AppendLine("using GCP.Entity;");
            sb.AppendLine("using GCP.Model;");
            sb.AppendLine("using GCP.Entity." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.Business." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.Model.Param." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.WebAPI.Controllers;");
            sb.AppendLine("using GCP.WebAPI.Filter;");
            sb.AppendLine();

            sb.AppendLine("namespace GCP.WebAPI.Controllers." + baseConfigModel.OutputConfig.OutputModule + ".Controllers");
            sb.AppendLine("{");

            SetClassDescription("控制器类", baseConfigModel, sb);

            sb.AppendLine("    [Route(\"[controller]/[action]\")]");
            sb.AppendLine("    [ApiController]");
            sb.AppendLine("    public class " + baseConfigModel.FileConfig.ControllerName + " : BaseController");
            sb.AppendLine("    {");
            sb.AppendLine("        private " + baseConfigModel.FileConfig.BusinessName + " " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + " = new " + baseConfigModel.FileConfig.BusinessName + "();");
            sb.AppendLine();
            sb.AppendLine("        #region 获取数据");
            sb.AppendLine();
            sb.AppendLine("        /// <summary> ");
            sb.AppendLine("        /// 获取" + baseConfigModel.MenuName + "列表 ");
            sb.AppendLine("        /// </summary> ");
            sb.AppendLine("        /// <param name=\"param\"></param> ");
            sb.AppendLine("        /// <returns></returns> ");
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "search") + "\")]");
            sb.AppendLine("        public async Task<ActionResult> GetListJson([FromQuery] " + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<List<" + baseConfigModel.FileConfig.EntityName + ">> obj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".GetList(param);");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        /// <summary> ");
            sb.AppendLine("        /// 获取" + baseConfigModel.MenuName + "分页列表 ");
            sb.AppendLine("        /// </summary> ");
            sb.AppendLine("        /// <param name=\"param\"></param> ");
            sb.AppendLine("        /// <param name=\"pagination\"></param> ");
            sb.AppendLine("        /// <returns></returns> ");
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "search") + "\")]");
            sb.AppendLine("        public async Task<ActionResult> GetPageListJson([FromQuery] " + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param, [FromQuery] Pagination pagination)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<List<" + baseConfigModel.FileConfig.EntityName + ">> obj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".GetPageList(param, pagination);");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        /// <summary> ");
            sb.AppendLine("        /// 获取单个" + baseConfigModel.MenuName + "信息 ");
            sb.AppendLine("        /// </summary> ");
            sb.AppendLine("        /// <param name=\"id\"></param> ");
            sb.AppendLine("        /// <returns></returns> ");
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "search") + "\")]");
            sb.AppendLine("        public async Task<ActionResult> GetFormJson([FromQuery] long id)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<" + baseConfigModel.FileConfig.EntityName + "> obj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".GetEntity(id);");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        /// <summary> ");
            sb.AppendLine("        /// 导出" + baseConfigModel.MenuName + "信息 ");
            sb.AppendLine("        /// </summary> ");
            sb.AppendLine("        /// <param name=\"param\"></param> ");
            sb.AppendLine("        /// <returns></returns> ");
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "search") + "\")]");
            sb.AppendLine("        public async Task<IActionResult> ExportJson([FromBody] " + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<string> obj = new TData<string>();");
            sb.AppendLine("            TData<List<" + baseConfigModel.FileConfig.EntityName + ">> Tobj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".GetList(param);");
            sb.AppendLine("            if (Tobj.Tag == 1)");
            sb.AppendLine("            {");

            StringBuilder sbColumns = new StringBuilder("");
            if (baseConfigModel.Columns != null && baseConfigModel.Columns.Count > 0)
            {
                for (var i = 0; i < (baseConfigModel.Columns.Count < 5 ? baseConfigModel.Columns.Count : 5); i++)
                {
                    sbColumns.Append("\"" + baseConfigModel.Columns[i] + "\",");
                }
                sbColumns = sbColumns.Remove(sbColumns.Length - 1, 1);
            }

            sb.AppendLine("                string file = new ExcelHelper<" + baseConfigModel.FileConfig.EntityName + ">().ExportToExcel(\"导出数据\" + DateTime.Now.ToString(\"yyyyMMddHHmmssfff\") + \".xls\", \"导出列表\", Tobj.Data, new string[] { " + sbColumns.ToString() + " });");
            sb.AppendLine("                obj.Data = file;");
            sb.AppendLine("                obj.Tag = 1;");
            sb.AppendLine("            }");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 提交数据");
            sb.AppendLine();
            sb.AppendLine("        /// <summary> ");
            sb.AppendLine("        /// 提交" + baseConfigModel.MenuName + "信息 ");
            sb.AppendLine("        /// </summary> ");
            sb.AppendLine("        /// <param name=\"entity\"></param> ");
            sb.AppendLine("        /// <returns></returns> ");
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "add") + "," + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "edit") + "\")]");
            sb.AppendLine("        public async Task<ActionResult> SaveFormJson([FromBody] " + baseConfigModel.FileConfig.EntityName + " entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<string> obj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".SaveForm(entity);");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        /// <summary> ");
            sb.AppendLine("        /// 删除" + baseConfigModel.MenuName + "信息 ");
            sb.AppendLine("        /// </summary> ");
            sb.AppendLine("        /// <param name=\"ids\"></param> ");
            sb.AppendLine("        /// <returns></returns> ");
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "delete") + "\")]");
            sb.AppendLine("        public async Task<ActionResult> DeleteFormJson([FromBody] string ids)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData obj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".DeleteForm(ids);");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        #endregion");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        public string BuildEmptyController(BaseConfigModel baseConfigModel)
        {
            string modulePrefix = GetModulePrefix(baseConfigModel);
            string classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Web;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using GCP.Util;");
            sb.AppendLine("using GCP.Util.Model;");
            sb.AppendLine("using GCP.Entity;");
            sb.AppendLine("using GCP.Model;");
            sb.AppendLine("using GCP.Entity." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.Business." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.Model.Param." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using GCP.WebAPI.Controllers;");
            sb.AppendLine("using GCP.WebAPI.Filter;");
            sb.AppendLine();

            sb.AppendLine("namespace GCP.WebAPI.Controllers." + baseConfigModel.OutputConfig.OutputModule + ".Controllers");
            sb.AppendLine("{");

            SetClassDescription("控制器类", baseConfigModel, sb);

            sb.AppendLine("    [Route(\"[controller]/[action]\")]");
            sb.AppendLine("    [ApiController]");
            sb.AppendLine("    public class " + baseConfigModel.FileConfig.ControllerName + " :  BaseController");
            sb.AppendLine("    {");
            sb.AppendLine("        private " + baseConfigModel.FileConfig.BusinessName + " " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + " = new " + baseConfigModel.FileConfig.BusinessName + "();");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region BuildMenu
        public string BuildMenu(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("  菜单路径: " + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/" + baseConfigModel.FileConfig.PageName);
            sb.AppendLine();
            string modulePrefix = GetModulePrefix(baseConfigModel);
            string classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();
            sb.AppendLine("  页面显示权限：" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "view"));
            sb.AppendLine();
            List<KeyValue> list = GetButtonAuthorizeList();


            foreach (var btnId in baseConfigModel.BtnList)
            {
                var Key = ((OperateButtonList)btnId).ToString();
                var Value = Key.ToLower().Replace("btn", "");
                var Description = btnId.GetDescriptionByEnum<OperateButtonList>();
                KeyValue button = new KeyValue() { Key = Key, Value = Value, Description = Description };
                sb.AppendLine("  按钮" + button.Description + "权限：" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, button.Value));
            }
            
            sb.AppendLine();
            return sb.ToString();
        }
        #endregion

        #region CreateCode

        public async Task<List<KeyValue>> CreateCode(BaseConfigModel baseConfigModel, CodeModel code)
        {
            List<KeyValue> result = new List<KeyValue>();
            baseConfigModel.MenuName = baseConfigModel.MenuName ?? "[菜单名称]";

            #region 实体类
            if (!string.IsNullOrEmpty(code.CodeEntity))
            {
                string codeEntity = HttpUtility.HtmlDecode(code.CodeEntity);
                string codePath = Path.Combine(baseConfigModel.OutputConfig.Output, "GCP.Entity", baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.EntityName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeEntity);
                    result.Add(new KeyValue { Key = "实体类", Value = codePath });
                }
            }
            #endregion

            #region 实体查询类
            if (!code.CodeEntityParam.IsEmpty())
            {
                string codeListEntity = HttpUtility.HtmlDecode(code.CodeEntityParam);
                string codePath = Path.Combine(baseConfigModel.OutputConfig.Output, "GCP.Model", "Param", baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.EntityParamName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeListEntity);
                    result.Add(new KeyValue { Key = "实体查询类", Value = codePath });
                }
            }
            #endregion

            #region 服务类
            if (!code.CodeService.IsEmpty())
            {
                string codeService = HttpUtility.HtmlDecode(code.CodeService);
                string codePath = Path.Combine(baseConfigModel.OutputConfig.Output, "GCP.Service", baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.ServiceName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeService);
                    result.Add(new KeyValue { Key = "服务类", Value = codePath });
                }
            }
            #endregion

            #region 业务类
            if (!code.CodeBusiness.IsEmpty())
            {
                string codeBusiness = HttpUtility.HtmlDecode(code.CodeBusiness);
                string codePath = Path.Combine(baseConfigModel.OutputConfig.Output, "GCP.Business", baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.BusinessName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeBusiness);
                    result.Add(new KeyValue { Key = "业务类", Value = codePath });
                }
            }
            #endregion

            #region 控制器
            if (!code.CodeController.IsEmpty())
            {
                string codeController = HttpUtility.HtmlDecode(code.CodeController.ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Controllers", baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.ControllerName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeController);
                    result.Add(new KeyValue { Key = "控制器", Value = codePath });
                }
            }
            #endregion

            #region 菜单

            List<KeyValue> buttonAuthorizeList = GetButtonAuthorizeList();
            string menuUrl = "/";
            string modulePrefix = GetModulePrefix(baseConfigModel);
            string classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();
            SysMenuEntity menuEntity = new SysMenuEntity
            {
                MenuName = baseConfigModel.MenuName,
                MenuUrl = menuUrl,
                MenuType = (int)MenuTypeEnum.Menu,
                Authorize = string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "view"),
                MenuTarget = modulePrefix
            };
            TData obj = await AddMenu(menuEntity);
            if (obj.Tag == 1)
            {
                result.Add(new KeyValue { Key = "菜单(刷新页面可见)", Value = menuUrl });

                foreach (var btnId in baseConfigModel.BtnList)
                {
                    try
                    {
                        var Key = ((OperateButtonList)btnId).ToString();
                        if (Key != null)
                        {
                            var Value = Key.ToLower().Replace("btn", "");
                            var Description = btnId.GetDescriptionByEnum<OperateButtonList>();
                            KeyValue button = new KeyValue() { Key = Key, Value = Value, Description = Description };
                            SysMenuEntity buttonEntity = new SysMenuEntity
                            {
                                ParentId = menuEntity.ID,
                                MenuName = baseConfigModel.FileConfig.ClassDescription + button.Description,
                                MenuType = (int)MenuTypeEnum.Button,
                                Authorize = string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, button.Value),
                                MenuTarget = modulePrefix
                            };
                            await AddMenu(buttonEntity);
                        }
                    }
                    catch (Exception err)
                    { }
                }
                new MenuCache().Remove();
            }

            #endregion

            return result;
        }

        private async Task<TData> AddMenu(SysMenuEntity menuEntity)
        {
            TData obj = new TData();
            IEnumerable<SysMenuEntity> menuList = await menuService.GetList(null); ;
            if (!menuList.Where(p => p.MenuName == menuEntity.MenuName && p.Authorize == menuEntity.Authorize).Any())
            {
                menuEntity.MenuSort = menuList.Max(p => p.MenuSort) + 1;
                menuEntity.Create();
                await menuService.SaveForm(menuEntity);
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region CreateEntity

        public async Task<List<KeyValue>> CreateEntity(BaseConfigModel baseConfigModel, CodeModel code)
        {
            List<KeyValue> result = new List<KeyValue>();

            #region 实体类
            if (!string.IsNullOrEmpty(code.CodeEntity))
            {
                string codeEntity = HttpUtility.HtmlDecode(code.CodeEntity);
                string codePath = Path.Combine(baseConfigModel.OutputConfig.Output, "GCP.Entity", baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.EntityName + ".cs");
                //if (!File.Exists(codePath))
                //{

                //}
                FileHelper.CreateFile(codePath, codeEntity);
                result.Add(new KeyValue { Key = "实体类", Value = codePath });
            }
            #endregion

            return result;
        }

        #endregion


        #region 私有方法

        #region GetProjectRootPath
        private string GetProjectRootPath(string path)
        {
            path = path.ParseToString();
            path = path.Trim('\\');
            if (GlobalContext.SystemConfig.Debug)
            {
                // 向上找二级
                path = Directory.GetParent(path).FullName;
                //path = Directory.GetParent(path).FullName;
            }
            return path;
        }
        #endregion

        #region SetClassDescription
        private void SetClassDescription(string type, BaseConfigModel baseConfigModel, StringBuilder sb)
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 创 建：" + baseConfigModel.FileConfig.CreateName);
            sb.AppendLine("    /// 日 期：" + baseConfigModel.FileConfig.CreateDate);
            sb.AppendLine("    /// 描 述：" + baseConfigModel.FileConfig.ClassDescription + type);
            sb.AppendLine("    /// </summary>");
        }
        #endregion

        #region GetButtonAuthorizeList
        private List<KeyValue> GetButtonAuthorizeList()
        {
            var list = new List<KeyValue>();
            list.Add(new KeyValue { Key = "btnSearch", Value = "search", Description = "搜索" });
            list.Add(new KeyValue { Key = "btnAdd", Value = "add", Description = "新增" });
            list.Add(new KeyValue { Key = "btnEdit", Value = "edit", Description = "修改" });
            list.Add(new KeyValue { Key = "btnDelete", Value = "delete", Description = "删除" });
            return list;
        }
        #endregion 

        private string GetModulePrefix(BaseConfigModel baseConfigModel)
        {
            return baseConfigModel.OutputConfig.OutputModule.Replace("Manage", string.Empty).ToLower();
        }

        private string GetBaseEntity(DataTable dt)
        {
            string entity = string.Empty;
            var columnList = dt.AsEnumerable().Select(p => p["Name"].ParseToString()).ToList();

            bool id = columnList.Where(p => p == "id").Any();
            bool status = columnList.Where(p => p == "status").Any();
            bool updatetime = columnList.Where(p => p == "updatetime").Any();

            if (!id)
            {
                throw new Exception("数据库表必须有主键ID字段");
            }
            if (status && updatetime)
            {
                entity = "BaseEntity";
            }
            else
            {
                entity = "BaseIDEntity";
            }
            return entity;
        }

        private string GetSaveFormCreate(string entity)
        {
            string line = string.Empty;
            switch (entity)
            {
                case "BaseEntity":
                    line = "entity.Create();";
                    break;

                case "BaseIDEntity":
                    line = "entity.Create();";
                    break;
            }
            return line;
        }

        private string GetSaveFormModify(string entity)
        {
            string line = string.Empty;
            switch (entity)
            {
                case "BaseEntity":
                    line = "entity.Modify();";
                    break;

                case "BaseIDEntity":
                    line = string.Empty;
                    break;
            }
            return line;
        }

        #endregion
    }
}
