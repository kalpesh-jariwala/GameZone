using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class MvcExtensions
    {
        public static List<string> GetErrorListFromModelState(this ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }

        public static string GetAttributeValueFromHtml(this string html, string attribute)
        {
            var regex = new Regex(@"(?<=\b" + attribute + @"="")[^""]*");
            var match = regex.Match(html);
            return match.Value;
        }

        public static System.Web.HtmlString ToHTMLAttributeString(this Object attributes)
        {
            var props = attributes.GetType().GetProperties();
            var pairs = props.Select(x => string.Format(@"{0}=""{1}""", x.Name, x.GetValue(attributes, null))).ToArray();
            return new HtmlString(string.Join(" ", pairs));
        }

        public static RouteValueDictionary ConditionalDisable(bool disabled,object htmlAttributes = null)
        {
            var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (disabled)
                dictionary.Add("disabled", "disabled");

            return dictionary;
        }
    }

}