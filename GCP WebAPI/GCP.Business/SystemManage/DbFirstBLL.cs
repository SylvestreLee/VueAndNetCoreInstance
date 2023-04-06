using FreeSql.DatabaseModel;
using GCP.Repository;
using GCP.Util.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using GCP.Util;
using GCP.Entity;

namespace GCP.Business.SystemManage
{
    public class DbFirstBLL
    {
        public Task<TData<string>> GetTableStruct(string ManageName, string TableName)
        {
            var task = Task.Run(() =>
            {

                List<string> IGNORECOLUMN = new List<string>()
                {
                    "ID".ToUpper(),
                    "STATUS".ToUpper(),
                    "UPDATETIME".ToUpper(),
                    "Level1_ID".ToUpper(),
                    "Level1_Uploaded".ToUpper(),
                    "Level1_UploadTime".ToUpper(),
                    "Level1_UploadCount".ToUpper(),
                    "UploadTime".ToUpper()
                };

                TData<string> obj = new TData<string>();
                StringBuilder SbText = new StringBuilder();

                if (GlobalContext.SystemConfig.Debug)
                {

                    var Fsql = Sql.GetFsql();
                    var Tables = (Fsql.DbFirst.GetTablesByDatabase(GlobalContext.SystemConfig.DBFirstDatabase).Where(t => t.Type == DbTableType.TABLE && t.Name.ToUpper() == TableName.ToUpper())).ToList();
                    if (Tables.Count > 0)
                    {
                        var t = Tables[0];
                        SbText.AppendLine("using System;");
                        SbText.AppendLine("using System.Collections.Generic;");
                        SbText.AppendLine("using Newtonsoft.Json;");
                        SbText.AppendLine("using FreeSql.DataAnnotations;");
                        SbText.AppendLine();
                        SbText.AppendLine("namespace GCP.Entity." + ManageName);
                        SbText.AppendLine("{");
                        SbText.AppendLine("    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = \"" + t.Name + "\")]");
                        SbText.AppendLine("    public partial class " + t.Name.Replace("_", "") + "Entity : BaseEntity");
                        SbText.AppendLine("    {");

                        var times = 0;
                        var TableStruct = Fsql.DbFirst.GetTableByName(t.Name);
                        foreach (var column in TableStruct.Columns)
                        {
                            if (!BaseField.BaseFieldList.Contains(column.Name.ToUpper()))
                            {
                                if (times > 0)
                                {
                                    SbText.AppendLine("        ");
                                }
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
                                    SbText.Append(",IsPrimary = true");
                                }
                                if (column.IsIdentity)
                                {
                                    SbText.Append(", IsIdentity = true");
                                }
                                SbText.Append(")]");
                                SbText.AppendLine();

                                SbText.Append("        public " + column.CsType.FullName);
                                if (column.IsNullable && column.CsType != typeof(string))
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
                }
                obj.Data = SbText.ToString();
                obj.Tag = 1;

                return obj;
            });

            return task;
        }
    }
}
