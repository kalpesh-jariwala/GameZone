using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace UI_Web.HtmlHelpers
{
    public static partial class HtmlHelpers
    {

        public static MvcHtmlString SubmitButton(this HtmlHelper htmlHelper, string buttonText, bool withContainer,
                                          object htmlAttributes)
        {
            return htmlHelper.SubmitButton(buttonText, withContainer, new RouteValueDictionary(htmlAttributes));
        }


        public static MvcHtmlString SubmitButton(this HtmlHelper htmlHelper, string buttonText, bool withContainer,
                                           IDictionary<string, object> htmlAttributes)
        {

            var builder = new TagBuilder("button");
            builder.InnerHtml = buttonText;
            builder.Attributes.Add("type", "submit");
            builder.MergeAttributes(htmlAttributes);

            if (!withContainer)
            {
                return MvcHtmlString.Create(builder.ToString());
            }
            else
            {
                var divbuilder = new TagBuilder("div");
                divbuilder.Attributes.Add("class", "form-actions");
                divbuilder.InnerHtml = builder.ToString();
                return MvcHtmlString.Create(divbuilder.ToString(TagRenderMode.Normal));

            }
        }

        public static MvcHtmlString ButtonLink(this HtmlHelper htmlHelper,
           string buttonText, string actionName, string controllerName, object routeValues,
                                          object htmlAttributes)
        {

            return htmlHelper.ButtonLink(buttonText, actionName, controllerName, new RouteValueDictionary(routeValues),
                                         new RouteValueDictionary(htmlAttributes));

        }

        public static MvcHtmlString ButtonLink(this HtmlHelper htmlHelper,
            string buttonText, string actionName, string controllerName, RouteValueDictionary routeValues,
                                           object htmlAttributes)
        {

            return htmlHelper.ButtonLink(buttonText, actionName, controllerName, routeValues,
                                         new RouteValueDictionary(htmlAttributes));

        }
        public static MvcHtmlString ButtonLink(this HtmlHelper htmlHelper, string buttonText, string actionName, string controllerName, RouteValueDictionary routeValues,
                                           IDictionary<string, object> htmlAttributes)
        {
            var href = UrlHelper.GenerateUrl("default", actionName, controllerName, routeValues, RouteTable.Routes, htmlHelper.ViewContext.RequestContext, false);
            var builder = new TagBuilder("button");
            builder.Attributes.Add("onclick", string.Format("location='{0}'", href));
            builder.InnerHtml = buttonText;
            builder.MergeAttributes(htmlAttributes);
            return MvcHtmlString.Create(builder.ToString());

            //var buttonHtml = string.Format("<input type=\"button\" title=\"{0}\" value=\"{0}\" onclick=\"location.href='{1}'\" class=\"button\" />", buttonText, href);
            //return new MvcHtmlString(buttonHtml);
        }



        public static MvcHtmlString SubmitInput(this HtmlHelper htmlHelper, string buttonText, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("input");
            builder.Attributes.Add("type", "submit");
            builder.Attributes.Add("title", buttonText);
            builder.MergeAttributes(htmlAttributes);
            return new MvcHtmlString(builder.ToString());
        }
    }
}