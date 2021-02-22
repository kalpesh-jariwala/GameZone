using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UI_Web.RavenStore_Initilize;

namespace UI_Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IDocumentStore Store { get; private set; }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Store = RavenInitilize.Initialize();
        }
    }
}
