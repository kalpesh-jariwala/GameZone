using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace BLL_Core.Abstract.AccountFunctions
{
   public abstract class AccountFunction
    {
        #region Authencation Stored
        public static void IssueAuthTicket(string username, bool persist)
        {
            FormsAuthentication.SetAuthCookie(username, persist);
            //remove cross domain auth
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Domain = string.Concat(".PlaySports.com");
            authCookie.Expires = System.DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public static void RevokeAuthTicket()
        {
            FormsAuthentication.SignOut();
            //remove cross domain auth
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Domain = string.Concat(".PlaySports.com");
            authCookie.Expires = System.DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }
        #endregion
    }
}
