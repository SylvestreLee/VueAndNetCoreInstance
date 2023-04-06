using GCP.Service.SystemManage;
using GCP.Util;
using GCP.Util.Extension;
using GCP.Util.Model;
using System;
using System.Threading.Tasks;
using GCP.Code;
using GCP.Cache.Factory;
using GCP.Entity.SystemManage;
using GCP.Enum;
using System.Collections.Generic;
using System.Linq;
using GCP.Model.Param.SystemManage;
using GCP.Business.Cache;
using System.Text;
using static GCP.Model.CommonModel;
using GCP.Model.Result;
using GCP.Service;

namespace GCP.Business.SystemManage
{
    public class DropDownBLL : CommonService
    {
        public async Task<TData<List<EnumIdNameModel>>> Get(string EnumTableName, string IDColumn, string NameColumn, string Where)
        {
            TData<List<EnumIdNameModel>> obj = new TData<List<EnumIdNameModel>>();
            if (EnumTableName.ToUpper() != "Area".ToUpper())
            {
                EnumTableName = "Enum_" + EnumTableName;
            }
            //根据缓存等查询枚举表
            obj.Data = await EnumCache.Table(EnumTableName, IDColumn, NameColumn, Where).GetList();
            obj.Tag = 1;

            return obj;
        }

        public async Task<TData<List<ElementTreeInfo>>> GetOrg()
        {
            TData<List<ElementTreeInfo>> obj = new TData<List<ElementTreeInfo>>();

            List <ElementTreeInfo> elementTreeInfos = new List<ElementTreeInfo>();

            List<ZtreeInfo> ztreeInfos = new List<ZtreeInfo>();
            var ListTableName = await EnumCache.Table("Enum_Person_OrganizationType", "ID", "TableName").GetList();
            var ListName = await EnumCache.Table("Enum_Person_OrganizationType").GetList();
            for (var i = 0; i < ListTableName.Count; i++)
            {
                ZtreeInfo ztree = new ZtreeInfo();
                ztree.id = ListName[i].ID;
                ztree.name = ListName[i].Name;
                ztree.pId = "0";
                ztreeInfos.Add(ztree);

                var ChirdList = await GetEnumList(ListTableName[i].Name);
                foreach (var c in ChirdList)
                {
                    ZtreeInfo z = new ZtreeInfo();
                    z.id = ztree.id + "_" + c.ID;
                    z.name = c.Name;
                    z.pId = ztree.id;
                    ztreeInfos.Add(z);
                }
            }

            elementTreeInfos = CommonBLL.GetElementTreeInfo(ztreeInfos);
            obj.Data = elementTreeInfos;
            obj.Tag = 1;
            return obj;
        }
    }
}
