using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace UI_Web.RavenStore_Initilize
{
    public class RavenInitilize
    {
        public static IDocumentStore Initialize()
        {
            var connection = new BLL_Core.Certificates.Connections(System.Web.Configuration.WebConfigurationManager.AppSettings["Environment"], System.Web.Configuration.WebConfigurationManager.AppSettings["Database"]);
            IDocumentStore Store = new DocumentStore
            {
                Urls = connection.Urls,
                Database = connection.DatabaseName,
                Certificate = connection.Certificate,
            };
            Store.Initialize();

            IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), Store);
            return Store;
        }
    }
}