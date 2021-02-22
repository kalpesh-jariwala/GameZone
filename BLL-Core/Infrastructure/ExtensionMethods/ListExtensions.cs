using BLL_Core.Infrastructure.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class ListExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null) return true;
            return !source.Any();
        }

        public static List<SelectListItem> ToSimpleSelectList(this IEnumerable<string> source)
        {
            return source.Select(a => new SelectListItem
            {
                Text = a,
                Value = a
            })
                .ToList();
        }

        public static List<SelectListItem> ToSimpleSelectList(this IEnumerable<string> source, string selectedValue)
        {
            var selectList = source.Select(a => new SelectListItem
            {
                Selected = a == selectedValue,
                Text = a,
                Value = a
            })
                .ToList();
            return selectList;
        }

        public static System.Web.Mvc.SelectList FromEnumToSelectList<TEnum>(this TEnum obj)
         where TEnum : struct, IComparable, IFormattable, IConvertible
        {

            return new SelectList(Enum.GetValues(typeof(TEnum)).OfType<Enum>()
                .Select(x =>
                    new SelectListItem
                    {
                        Text = Enum.GetName(typeof(TEnum), x),
                        Value = (Convert.ToInt32(x)).ToString()
                    }), "Value", "Text");
        }

        public static System.Web.Mvc.SelectList FromEnumToSelectListTextValue<TEnum>(this TEnum obj)
        where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            return new SelectList(Enum.GetValues(typeof(TEnum)).OfType<Enum>()
                .Select(x =>
                    new SelectListItem
                    {
                        Text = Enum.GetName(typeof(TEnum), x),
                        Value = Enum.GetName(typeof(TEnum), x)
                    }), "Value", "Text");
        }

        public static void RemoveNulls<T>(this IList<T> collection) where T : class
        {
            for (var i = collection.Count - 1; i >= 0; i--)
            {
                if (collection[i] == null)
                    collection.RemoveAt(i);
            }
        }

        public static void RemoveEmptyStrings(this IList<string> collection)
        {
            if (collection.IsNotNullOrEmpty())
            {
                for (var i = collection.Count - 1; i >= 0; i--)
                {
                    if (collection[i] == ""|| collection[i] == " ")
                        collection.RemoveAt(i);
                }
            }

        }
        public static List<SelectListItem> InsertHeader(this List<SelectListItem> source, string selectedValue)
        {
            string item = "--" + selectedValue + "--";
            source.Insert(0, new SelectListItem()
            {
                Text = item,
                Value = ""

            });
            return source;
        }


        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> sequence, Func<T, IEnumerable<T>> childFetcher)
        {
            var itemsToYield = new Queue<T>(sequence);
            while (itemsToYield.Count > 0)
            {
                var item = itemsToYield.Dequeue();
                yield return item;

                var children = childFetcher(item);
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        itemsToYield.Enqueue(child);
                    }
                }
            }
        }

        public static string FormattedConcat(this IEnumerable<string> sequence, string separator = ", ")
        {
            var count = sequence.Count();
            if (count == 0)
                return null;
            if (count == 1)
                return sequence.First();
            var newString = new StringBuilder();
            bool first = true;
            foreach (var item in sequence)
            {
                if (first)
                {
                    first = false;
                    newString.Append(item.Trim());
                }
                else
                {
                    newString.Append(separator);
                    newString.Append(item.Trim());
                }
            }
            return newString.ToString();
        }

        public static MvcHtmlString EnumDropDownListWithStringFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel, object htmlAttributes)
        {
            var selectListItem = Enum.GetNames(Nullable.GetUnderlyingType(typeof(TEnum))).Select(p => new SelectListItem() { Value = p, Text = p }).ToList();
            return SelectExtensions.DropDownListFor(htmlHelper, expression, selectListItem, optionLabel, htmlAttributes);
        }
    }
}
