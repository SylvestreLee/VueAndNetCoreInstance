using GCP.Business.SystemManage;
using GCP.Model.Result;
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
    [Route("[controller]")]
    [ApiController]
    public class DropDownController : BaseController
    {
        private DropDownBLL dropDownBLL = new DropDownBLL();

        [HttpGet]
        [AuthorizeFilter]
        public async Task<TData<List<EnumIdNameModel>>> Get(string EnumTableName, string IDColumn = "ID", string NameColumn = "Name", string? Where = "")
        {
            return await dropDownBLL.Get(EnumTableName, IDColumn, NameColumn, Where);
        }

        [HttpGet("[action]")]
        [AuthorizeFilter]
        public async Task<TData<List<ElementTreeInfo>>> GetOrg()
        {
            return await dropDownBLL.GetOrg();
        }
    }
}
