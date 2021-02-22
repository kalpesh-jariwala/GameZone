using BLL_Core.Infrastructure.ExtensionMethods;
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
    public class BaseDb
    {
        public readonly IDocumentSession session;
        public IDocumentStore store;

        public BaseDb()
        {

        }

        public BaseDb(IDocumentSession session)
        {
            this.session = session;
        }
        public BaseDb(IDocumentStore store)
        {
            this.store = store;
        }

        public UserDocument GetUserUsingUserName(string userName)
        {
            var user = session.Query<UserDocument>().Where(x => x.Username == userName).SingleOrDefault();
            return user;
        }

    }
}
