using BLL_Core.Abstract.AccountFunctions;
using BLL_Core.ModelClassess.Account;
using BLL_Core.RavenDocuments;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Core.DbClasses
{
    public class AccountDb : BaseDb
    {
        public readonly IDocumentSession session;
        public IDocumentStore store;
        private readonly DefaultSecurityEncoder _encoder = new DefaultSecurityEncoder();
        public AccountDb()
        {
        }

        public AccountDb(IDocumentSession session) : base(session)
        {
            this.session = session;
        }
        public AccountDb(IDocumentStore store) : base(store)
        {
            this.store = store;
        }

        #region User-Account Setting
        public bool Login(string username, string password, bool rememberMe = false)
        {
            UserDocument user = GetUserUsingUserName(username);
            if (user == null)
            {
                return false;
            }
            bool passed = false;
            if (user.Salt != null && user.Salt != "")
            {
                string encodedPassword = _encoder.Encode(password, user.Salt);
                if (encodedPassword.Equals(user.Password) || user.Password == password)
                { //allow the login if the password given is correct or correct and already encoded (shouldn't allow encoded password)
                    passed = true;
                }
            }
            if (passed)
            {
                AccountFunction.IssueAuthTicket(username, rememberMe);
                return true;
            }
            return false;
        }

        public bool LoginCheck(string username, string password)
        {
            UserDocument user = GetUserUsingUserName(username);
            if (user == null)
            {
                return false;
            }

            string encodedPassword = _encoder.Encode(password, user.Salt);
            bool passed = encodedPassword.Equals(user.Password);
            if (passed)
            {
                return true;
            }
            return false;
        }

        public bool FacebookLogin(string username, string facebookId, bool rememberMe = false)
        {
           
            UserDocument user = GetUserUsingUserName(username);
            if (user == null)
            {
                return false;
            }
            if (user.OAuthAccounts == null)
            {
                return false;
            }
            var userFacebookOAuth = user.OAuthAccounts.FirstOrDefault(fb => fb.Provider == "facebook");
            if (userFacebookOAuth == null)
            {
                return false;
            }
            if (userFacebookOAuth.ProviderUserId == null)
            {
                return false;
            }
            bool passed = userFacebookOAuth.ProviderUserId.Equals(facebookId);

            if (passed)
            {
                AccountFunction.IssueAuthTicket(username, rememberMe);
                return true;
            }
            return false;
        }
        #endregion
    }
}
