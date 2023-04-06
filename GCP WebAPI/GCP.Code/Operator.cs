using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GCP.Util;
using Newtonsoft.Json;
using GCP.Cache.Factory;

namespace GCP.Code
{
    public class Operator
    {
        public static Operator Instance
        {
            get { return new Operator(); }
        }

        private string LoginProvider = GlobalContext.Configuration.GetSection("SystemConfig:LoginProvider").Value;
        private string TokenName = "UserToken"; //cookie name or session name

        public void AddCurrent(string token)
        {
            new CookieHelper().WriteCookie(TokenName, token);
        }

        /// <summary>
        /// Api接口需要传入apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        public void RemoveCurrent(string apiToken = "")
        {
            new CookieHelper().RemoveCookie(TokenName);
        }

        /// <summary>
        /// Api接口需要传入apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public async Task<OperatorInfo> Current(string apiToken = "")
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            OperatorInfo user = null;
            string token = string.Empty;
            if (hca.HttpContext != null)
            {
                token = new CookieHelper().GetCookie(TokenName);
            }
            if (string.IsNullOrEmpty(token))
            {
                return user;
            }
            token = token.Trim('"');
            user = CacheFactory.Cache.GetCache<OperatorInfo>("Token_" + token);
            return user;
        }
    }
}
