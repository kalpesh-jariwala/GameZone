using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UI_Web.HtmlHelpers
{
    public static partial class HtmlHelpers
    {

        public static IHtmlString Label(this HtmlHelper htmlHelper, string expression, string labelText, object htmlAttributes)
        {
            var metadata = ModelMetadata.FromStringExpression(expression, htmlHelper.ViewData);
            return LabelHelper(htmlHelper, metadata, expression, labelText, new RouteValueDictionary(htmlAttributes));
        }

        public static IHtmlString Label(this HtmlHelper htmlHelper, string expression, string labelText, IDictionary<string, object> htmlAttributes)
        {
            var metadata = ModelMetadata.FromStringExpression(expression, htmlHelper.ViewData);
            return LabelHelper(htmlHelper, metadata, expression, labelText, htmlAttributes);
        }


        private static IHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, 
            string htmlFieldName, string labelText, IDictionary<string, object> htmlAttributes)
        {
            string str = labelText ?? (metadata.DisplayName ?? (metadata.PropertyName ?? htmlFieldName.Split(new char[] { '.' }).Last<string>()));
            if (string.IsNullOrEmpty(str))
            {
                return MvcHtmlString.Empty;
            }
            TagBuilder tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.SetInnerText(str);
            return new HtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}
