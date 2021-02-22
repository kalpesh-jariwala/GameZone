using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BLL_Core.Infrastructure.ExtensionMethods;

namespace UI_Web.HtmlHelpers
{
    public static partial class HtmlHelpers
    {
        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues, object labelAttributes, object inputAttributes)
        {
            if (listOfValues == null) throw new ArgumentNullException("listOfValues");
            return htmlHelper.RadioButtonForSelectList(expression, listOfValues,
                labelAttributes == null ? null : HtmlHelper.AnonymousObjectToHtmlAttributes(labelAttributes),
                inputAttributes == null ? null : HtmlHelper.AnonymousObjectToHtmlAttributes(inputAttributes));
        }

        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues, IDictionary<string, object> labelAttributes,
            IDictionary<string, object> inputAttributes)
        {
            var propertyName = ExpressionHelper.GetExpressionText(expression);
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName);
            var fullId = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName);
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();

            if (listOfValues != null)
            {
                if (labelAttributes == null)
                {
                    labelAttributes = new Dictionary<string, object>();
                }
                if (inputAttributes == null)
                {
                    inputAttributes = new Dictionary<string, object>();
                } // Create a radio button for each item in the list 
                var unobtrusiveattributes = htmlHelper.GetUnobtrusiveValidationAttributes(propertyName, metaData);

                foreach (var item in listOfValues)
                {
                    var labelid = item.Value.NormaliseForHtmlId();
                    //// Generate an id to be given to the radio button field 
                    //string id = string.Format("{0}_{1}", propertyName, counter);
                    //inputAttributes.Add("id", id);

                    // Create and populate a radio button using the existing html helpers 
                    var label =
                        htmlHelper.Label(labelid, HttpUtility.HtmlEncode(item.Text), labelAttributes).ToHtmlString();


                    //Begin building html
                    var tag = new TagBuilder("input");   // <input 
                    tag.Attributes.Add("type", "radio");     // type="checkbox"
                    tag.Attributes.Add("name", fullName);       // name={selectedValuesPropertyName}
                    tag.Attributes.Add("value", item.Value);         // value="{value}"
                    tag.Attributes.Add("id", string.Format("{0}_{1}", fullId, labelid));
                    //See if this item is checked
                    if (item.Selected)
                    {
                        tag.Attributes.Add("checked", string.Empty);
                    }

                    tag.MergeAttributes(unobtrusiveattributes);
                    tag.MergeAttributes(inputAttributes); //Add attribut collection to element
                    var radio = MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing)).ToHtmlString(); //Render html
                    var labelcontaininginput = label.Insert(label.IndexOf(">") + 1, radio);
                    sb.AppendLine(labelcontaininginput);





                    //inputAttributes.Add("id", label.GetAttributeValueFromHtml("for"));

                    //var radio = string.Empty;
                    //if (item.Selected)
                    //{
                    //    inputAttributes.Add("checked", "checked");
                    //    radio = htmlHelper.RadioButtonFor(expression, item.Value, inputAttributes).ToHtmlString();
                    //}
                    //else
                    //{
                    //    radio = htmlHelper.RadioButtonFor(expression, item.Value, inputAttributes).ToHtmlString();
                    //}

                    //inputAttributes.Remove("id");
                    //inputAttributes.Remove("checked");
                    //var labelcontaininginput = label.Insert(label.IndexOf(">") + 1, radio);
                    //sb.AppendLine(labelcontaininginput);
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}