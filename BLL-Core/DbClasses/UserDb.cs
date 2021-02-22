using BLL_Core.ModelClassess.Account;
using BLL_Core.RavenDocuments;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace BLL_Core.DbClasses
{
    public class UserDb : BaseDb
    {
        public readonly IDocumentSession session;
        public IDocumentStore store;
        private readonly DefaultSecurityEncoder _encoder = new DefaultSecurityEncoder();
        public UserDb()
        {
        }

        public UserDb(IDocumentSession session) : base(session)
        {
            this.session = session;
        }
        public UserDb(IDocumentStore store) : base(store)
        {
            this.store = store;
        }

        
    }

}
