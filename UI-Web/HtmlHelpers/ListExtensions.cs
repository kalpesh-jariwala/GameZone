using System;
using System.Text;
using System.Web.Mvc;

namespace UI_Web.HtmlHelpers
{
    public static partial class HtmlHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, int currentPage, int totalPages, int maxeitherside, Func<int, string> pageUrl)
        {
            var result = new StringBuilder();
            var i = 1;
            var maxpagenumber = totalPages;



            for ( i = 1; i <= maxpagenumber; i++)
            {
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == currentPage)
                    tag.AddCssClass("selected");
                result.AppendLine(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}
