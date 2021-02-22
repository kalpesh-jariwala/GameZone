﻿using System.Linq;
using System.Web.Mvc;

namespace UI_Web.HtmlHelpers
{
    public static partial class HtmlHelpers
    {
        /// <summary>
        /// Returns an error alert that lists each model error, much like the standard ValidationSummary only with
        /// altered markup for the Twitter bootstrap styles.
        /// </summary>
        public static MvcHtmlString BootstrapValidationSummary(this HtmlHelper helper, bool closeable)
        {
            # region Equivalent view markup
            // var errors = ViewData.ModelState.SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage));
            //
            // if (errors.Count() > 0)
            // {
            //     <div class="alert alert-error alert-block">
            //         <button type="button" class="close" data-dismiss="alert">&times;</button>
            //         <strong>Validation error</strong> - please fix the errors listed below and try again.
            //         <ul>
            //             @foreach (var error in errors)
            //             {
            //                 <li class="text-error">@error</li>
            //             }
            //         </ul>
            //     </div>
            // }
            # endregion

            var errors = helper.ViewContext.ViewData.ModelState.SelectMany(state => state.Value.Errors.Select(error => error.ErrorMessage));

            int errorCount = errors.Count();

            if (errorCount == 0)
            {
                return new MvcHtmlString(string.Empty);
            }

            var div = new TagBuilder("div");
            div.AddCssClass("alert");
            div.AddCssClass("alert-error");

            string message;

            if (errorCount == 1)
            {
                message = errors.First();
            }
            else
            {
                message = "Please fix the errors listed below and try again.";
                div.AddCssClass("alert-block");
            }

            if (closeable)
            {
                var button = new TagBuilder("button");
                button.AddCssClass("close");
                button.MergeAttribute("type", "button");
                button.MergeAttribute("data-dismiss", "alert");
                button.SetInnerText("x");
                div.InnerHtml += button.ToString();
            }

            div.InnerHtml += "<strong>Validation error</strong> - " + message;

            if (errorCount > 1)
            {
                var ul = new TagBuilder("ul");

                foreach (var error in errors)
                {
                    var li = new TagBuilder("li");
                    li.AddCssClass("text-error");
                    li.SetInnerText(error);
                    ul.InnerHtml += li.ToString();
                }

                div.InnerHtml += ul.ToString();
            }

            return new MvcHtmlString(div.ToString());
        }

        /// <summary>
        /// Overload allowing no arguments.
        /// </summary>
        public static MvcHtmlString BootstrapValidationSummary(this HtmlHelper helper)
        {
            return BootstrapValidationSummary(helper, true);
        }
    }
}

