using System.Web.Mvc;

namespace UI_Web.HtmlHelpers
{
    public static partial class HtmlHelpers
    {
        public static string ResolveServerUrl(this HtmlHelper helper, string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1) return serverUrl;

            var newUrl = serverUrl;
            var originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) + "://" + originalUri.Authority + newUrl;
            return newUrl;
        } 
    }
}