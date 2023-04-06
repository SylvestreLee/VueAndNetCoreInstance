using System;
using System.Collections.Generic;
using System.Text;
using GCP.Util.Filter;

namespace GCP.CodeGenerator.Model
{
    public class BaseConfigModel
    {
        [输入输出字段]
        public string TableName { get; set; }
        [输入字段]
        public string MenuName { get; set; }
        [输入字段]
        public List<int> BtnList { get; set; }
        public List<string> Columns { get; set; }
        public string TableNameUpper { get; set; }
        public FileConfigModel FileConfig { get; set; }
        public OutputConfigModel OutputConfig { get; set; }
    }

    public class FileConfigModel
    {
        public string ClassPrefix { get; set; }
        public string ClassDescription { get; set; }
        public string CreateName { get; set; }
        public string CreateDate { get; set; }

        public string EntityName { get; set; }
        public string EntityMapName { get; set; }
        public string EntityParamName { get; set; }

        public string BusinessName { get; set; }
        public string ServiceName { get; set; }

        public string ControllerName { get; set; }
        public string PageName { get; set; }
    }

    public class OutputConfigModel
    {
        public List<string> ModuleList { get; set; }
        public string OutputModule { get; set; }
        public string OutputEntity { get; set; }
        public string OutputBusiness { get; set; }
        public string OutputWeb { get; set; }
        public string Output { get; set; }
    }

    public class CodeModel
    { 
        public string CodeEntity { get; set; }
        public string CodeEntityParam { get; set; }
        public string CodeService { get; set; }
        public string CodeBusiness { get; set; }
        public string CodeController { get; set; }
        public string CodeMenu { get; set; }
    }
}
