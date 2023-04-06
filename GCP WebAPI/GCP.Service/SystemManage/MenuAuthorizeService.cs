using GCP.Entity.SystemManage;
using GCP.Model.Param.SystemManage;
using GCP.Repository;
using GCP.Util;
using GCP.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GCP.Service.SystemManage
{
    public class MenuAuthorizeService
    {
        private RepositoryBase<SysMenuAuthorizeEntity> _repositoryBase = new RepositoryBase<SysMenuAuthorizeEntity>();

        #region 获取数据

        /// <summary>
        /// 获取监管平台/登录系统的菜单权限列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<SysMenuAuthorizeEntity>> GetList(SysMenuAuthorizeEntity param)
        {
            var expression = ListFilter(param);
            var list = await _repositoryBase.Where(expression).ToListAsync();
            return list;
        }

        #endregion

        #region 私有方法
        private Expression<Func<SysMenuAuthorizeEntity, bool>> ListFilter(SysMenuAuthorizeEntity param)
        {
            Expression<Func<SysMenuAuthorizeEntity, bool>> expression = (ex => ex.Status != 2);
            if (param != null)
            {
                if (param.AuthorizeId.ParseToLong() > 0)
                {
                    expression = expression.And(t => t.AuthorizeId == param.AuthorizeId);
                }
                if (param.AuthorizeType.ParseToInt() > 0)
                {
                    expression = expression.And(t => t.AuthorizeType == param.AuthorizeType);
                }
                if (!param.AuthorizeIds.IsEmpty())
                {
                    long[] authorizeIdArr = TextHelper.SplitToArray<long>(param.AuthorizeIds, ',');
                    expression = expression.And(t => authorizeIdArr.Contains(t.AuthorizeId.Value));
                }
            }
            return expression;
        }

        #endregion
    }
}
