using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI_Web.Controllers
{
    public class BaseController : Controller
    {

        public IDocumentSession Session { get; set; }
        public static IDocumentStore Store => MvcApplication.Store;
        // GET: Base

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.Session = Store.OpenSession();
            int pageNumber;
            if (!Int32.TryParse((string)RouteData.Values["pagenumber"], out pageNumber))
            {
                pageNumber = 1;
            }
            int pageSize;
            if (!Int32.TryParse((string)RouteData.Values["pagesize"], out pageSize))
            {
                pageSize = 75;
            }
            //CommunityRouteInfo = new BaseCommunityInput
            //{
            //    Id = RouteData.Values["id"] == null ? null : RouteData.Values["id"].ToString(),
            //    Slug = RouteData.Values["slug"] == null ? null : RouteData.Values["slug"].ToString(),
            //    InnerId = RouteData.Values["innerid"] == null ? null : RouteData.Values["innerid"].ToString(),
            //    InnerName = RouteData.Values["innerName"] == null ? null : RouteData.Values["innerName"].ToString(),
            //    LdbTemplate = RouteData.Values["ldbTemplate"] == null ? null : RouteData.Values["ldbTemplate"].ToString(),
            //    PageNumber = pageNumber,
            //    PageSize = pageSize,
            //    Username = Request.IsAuthenticated ? User.Identity.Name.ToLower() : null,
            //    Message = TempData["Message"] == null ? null : TempData["Message"] as string,
            //    Errors = TempData["Errors"] == null ? null : TempData["Errors"] as List<string>,
            //    ViewTab = TempData["ViewTab"] == null ? "List" : TempData["ViewTab"] as string
            //};
            //InitialiseDbClasses();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                //CommunityRouteInfo.Message = TempData["Message"] == null ? null : TempData["Message"] as string;

                if (filterContext.IsChildAction)
                {
                    return;
                }
                using (Session)
                {
                    if (filterContext.Exception != null)
                    {
                        return;
                    }
                    if (Session != null)
                    {
                        Session.SaveChanges();
                        Session.Dispose();
                    }
                    else
                    {
                        Session.Dispose();
                    }
                }
                base.OnActionExecuted(filterContext);
            }

            catch (Exception ex)
            {

            }


        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            using (Session)
            {
                if (Session != null)
                {
                    Session.Dispose();
                }
            }
        }
    }
}