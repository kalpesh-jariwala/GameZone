using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace UI_Web.HtmlHelpers
{
    public static partial class HtmlHelpers
    {
        public static MvcHtmlString TelephoneLink (this HtmlHelper helper, string telephonenumber, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("a");
            builder.InnerHtml = telephonenumber;
            builder.Attributes.Add("href", string.Format("tel:{0}", telephonenumber));
            builder.MergeAttributes(htmlAttributes);
            return MvcHtmlString.Create(builder.ToString());
            
        }

        //private static Regex regExHttpLinks = new Regex(@"(?<=\()\b(https?://|www\.)[-A-Za-z0-9+&@#/%?=~_()|!:,.;]*[-A-Za-z0-9+&@#/%=~_()|](?=\))|(?<=(?<wrap>[=~|_#]))\b(https?://|www\.)[-A-Za-z0-9+&@#/%?=~_()|!:,.;]*[-A-Za-z0-9+&@#/%=~_()|](?=\k<wrap>)|\b(https?://|www\.)[-A-Za-z0-9+&@#/%?=~_()|!:,.;]*[-A-Za-z0-9+&@#/%=~_()|]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex regExHttpLinks = new Regex(@"(?<=\()\b[-A-Za-z0-9+&@#/%?=~_()|!:,.;]*[-A-Za-z0-9+&@#/%=~_()|](?=\))|(?<=(?<wrap>[=~|_#]))\b[-A-Za-z0-9+&@#/%?=~_()|!:,.;]*[-A-Za-z0-9+&@#/%=~_()|](?=\k<wrap>)|\b(https?://|www\.)[-A-Za-z0-9+&@#/%?=~_()|!:,.;]*[-A-Za-z0-9+&@#/%=~_()|]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static MvcHtmlString Linkify(this HtmlHelper htmlHelper, string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return MvcHtmlString.Create(html);
            }

            html = htmlHelper.Encode(html);
            html = html.Replace(Environment.NewLine, "<br />");

            // replace periods on numeric values that appear to be valid domain names
            var periodReplacement = "[[[replace:period]]]";
            html = Regex.Replace(html, @"(?<=\d)\.(?=\d)", periodReplacement);

            // create links for matches
            var linkMatches = regExHttpLinks.Matches(html);
            for (int i = 0; i < linkMatches.Count; i++)
            {
                var temp = linkMatches[i].ToString();

                if (!temp.Contains("://"))
                {
                    temp = "http://" + temp;
                }

                html = html.Replace(linkMatches[i].ToString(), String.Format("<a target='_blank' href=\"{0}\" title=\"{0}\">{1}</a>", temp.Replace(".", periodReplacement).ToLower(), linkMatches[i].ToString().Replace(".", periodReplacement)));
            }

            // Clear out period replacement
            html = html.Replace(periodReplacement, ".");

            return MvcHtmlString.Create(html);
        }
    }
}