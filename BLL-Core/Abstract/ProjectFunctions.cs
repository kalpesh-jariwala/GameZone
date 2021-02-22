using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL_Core.Abstract
{
    public class ProjectFunctions
    {
        public static string GetSiteUrl()
        {

            var siteUrl = string.Empty;
            var environment = System.Web.Configuration.WebConfigurationManager.AppSettings["Environment"];
            int port = 44316;
            string originUrl = "";
            try
            {
                port = HttpContext.Current == null ? 44316 : (HttpContext.Current.Request.Url == null ? 80 : HttpContext.Current.Request.Url.Port);
                originUrl = HttpContext.Current == null ? "" : HttpContext.Current.Request.Url.Host;
            }
            catch (System.Exception) { }
            if (originUrl == "" && System.Diagnostics.Debugger.IsAttached)
            { //debugger is attached should be only on local
                originUrl = "localhost";
            }
            if (originUrl.ToLower().Contains("localhost"))
            {
                siteUrl = string.Format("https://localhost:{0}", port);
            }
            else
            {
                siteUrl = string.Format("https://localhost:{0}", port);
                //switch (environment.ToLower())
                //{
                //    case "test":
                //        siteUrl = "https://playcaddytest.apphb.com";
                //        break;
                //    case "newdata":
                //        siteUrl = "https://playwazenewdata.apphb.com";
                //        break;
                //    case "uat":
                //        siteUrl = "https://playwazeuat.apphb.com";
                //        break;
                //    case "dev":
                //        siteUrl = "https://playwazeraven4.apphb.com";
                //        break;
                //    default:
                //        siteUrl = "https://playwaze.com";
                //        break;

                //}
            }
            return siteUrl;
        }
    }
}
