using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class EnumerableExtensions
    {
       
            public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> collection,
                int n)
            {
                if (collection == null)
                    throw new ArgumentNullException("collection");
                if (n < 0)
                    throw new ArgumentOutOfRangeException("n", "n must be 0 or greater");

                LinkedList<T> temp = new LinkedList<T>();

                foreach (var value in collection)
                {
                    temp.AddLast(value);
                    if (temp.Count > n)
                        temp.RemoveFirst();
                }

                return temp;
            }
       
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            if (list == null || list.Count() == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> list)
        {
            if (list == null || list.Count() == 0)
            {
                return false;
            }
            return true;
        }


        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> text,
                                                           Func<T, string> value, string defaultOption)
        {
            var items = enumerable.Where(x => text(x).IsNotNull()).Select(f => new SelectListItem { Text = text(f), Value = value(f) }).ToList();
            if (defaultOption != null)
            {
                items.Insert(0, new SelectListItem { Text = defaultOption, Value = "-1" });
            }
            return items;
        }

        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> text,
                                                           Func<T, string> value)
        {
            var items = enumerable.Select(f => new SelectListItem { Text = text(f), Value = value(f) }).ToList();            
            return items;
        }

        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> text,
                                                           Func<T, string> value, string defaultOption,
                                                           IEnumerable<string> existingvalues)
        {
            var items = enumerable.Select(f => new SelectListItem { Text = text(f), Value = value(f) }).ToList();
            if (defaultOption != null)
            {
                items.Insert(0, new SelectListItem { Text = defaultOption, Value = "-1" });
            }
            foreach (var existingvalue in existingvalues)
            {
                var item = items.FirstOrDefault(e => e.Value == existingvalue);
                if (item != null)
                {
                    item.Selected = true;
                }
            }
            return items;
        }

        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> text,
                                                         Func<T, string> value, string defaultOption,
                                                         string selectedValue)
        {
            var items = enumerable.Select(f => new SelectListItem { Text = text(f), Value = value(f) }).ToList();
            if (defaultOption != null)
            {
                items.Insert(0, new SelectListItem { Text = defaultOption, Value = "-1" });
            }
           
                var item = items.FirstOrDefault(e => e.Value == selectedValue);
                if (item != null)
                {
                    item.Selected = true;
                }
            
            return items;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                          && method.IsGenericMethodDefinition
                          && method.GetGenericArguments().Length == 2
                          && method.GetParameters().Length == 2)
                                              .MakeGenericMethod(typeof(T), type)
                                              .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

        public static int IndexOf<T>(this IEnumerable<T> list, Predicate<T> condition)
        {
            System.Diagnostics.Contracts.Contract.Requires(list != null, "list can't be null");
            System.Diagnostics.Contracts.Contract.Requires(condition != null, "condition can't be null");

            int index = -1;
            return list.Any(item => { index++; return condition(item); }) ? index : -1;
        }

        /// <summary>
        /// Break a list of items into chunks of a specific size
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        public static IEnumerable<IEnumerable<T>> ChunkIntoParts<T>(this IEnumerable<T> source, int parts)
        {
            var list = new List<T>(source);
            int defaultSize = (int)((double)list.Count / (double)parts);
            int offset = list.Count % parts;
            int position = 0;

            for (int i = 0; i < parts; i++)
            {
                int size = defaultSize;
                if (i < offset)
                    size++; // Just add one to the size (it's enough).

                yield return list.GetRange(position, size);

                // Set the new position after creating a part list, so that it always start with position zero on the first yield return above.
                position += size;
            }
        }

        /// <summary>
        /// Performs an action on each value of the enumerable
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="enumerable">Sequence on which to perform action</param>
        /// <param name="action">Action to perform on every item</param>
        /// <exception cref="System.ArgumentNullException">Thrown when given null <paramref name="enumerable"/> or <paramref name="action"/></exception>
        public static void ForEachLinq<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
                        if (enumerable.IsNull())
            {
                throw new ArgumentNullException("enumerable");
            };
                        if (action.IsNull())
            {
                throw new ArgumentNullException("action");
            };


            foreach (T value in enumerable)
            {
                action(value);
            }
        }

        /// <summary>
        /// Convenience method for retrieving a specific page of items within a collection.
        /// </summary>
        /// <param name="pageIndex">The index of the page to get.</param>
        /// <param name="pageSize">The size of the pages.</param>
        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
                                    if (source.IsNull())
            {
                throw new ArgumentNullException("source");
            };
                        if (pageIndex < 0)
            {
                throw new ArgumentException("The page index cannot be negative.","pageIndex");
            };
                        if (pageSize <= 0)
            {
                throw new ArgumentException("The page size must be greater than zero.","pageSize");
            };



            return source.Skip(pageIndex * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Converts an enumerable into a readonly collection
        /// </summary>
        public static IEnumerable<T> ToReadOnlyCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ReadOnlyCollection<T>(enumerable.ToList());
        }

        

        /// <summary>
        /// Concatenates the members of a collection, using the specified separator between each member.
        /// </summary>
        /// <returns>A string that consists of the members of <paramref name="values"/> delimited by the <paramref name="separator"/> string. If values has no members, the method returns null.</returns>
        public static string JoinOrDefault<T>(this IEnumerable<T> values, string separator)
        {
            if (separator.IsNullOrEmpty())
            {
                throw new ArgumentNullException("separator");
            };

            if (values == null)
                return default(string);

            return string.Join(separator, values);
        }

        /// <summary>
        /// Extension for Except on different lists if both list have something common that can be used to compare them, like an Id or Key
        /// </summary>
        public static IEnumerable<TA> Except<TA, TB, TK>(
        this IEnumerable<TA> a,
        IEnumerable<TB> b,
        Func<TA, TK> selectKeyA,
        Func<TB, TK> selectKeyB,
        IEqualityComparer<TK> comparer = null)
        {
            return a.Where(aItem => !b.Select(selectKeyB).Contains(selectKeyA(aItem), comparer));
        }

        /// <summary>
        /// Does a list contain all values of another list?
        /// </summary>
        /// <remarks>Needs .NET 3.5 or greater.  Source:  https://stackoverflow.com/a/1520664/1037948 </remarks>
        /// <typeparam name="T">list value type</typeparam>
        /// <param name="containingList">the larger list we're checking in</param>
        /// <param name="lookupList">the list to look for in the containing list</param>
        /// <returns>true if it has everything</returns>
        public static bool ContainsAllItems<T>(this IEnumerable<T> containingList, IEnumerable<T> lookupList)
        {
            return !lookupList.Except(containingList).Any();
        }

        /// <summary> 
        /// Allows usage of foreach while maintaining access to the array index, e.g. foreach (var (item, index) in collection.WithIndex()) 
        /// </summary> 
        /// <returns>A tuple of (item, index)</returns> 
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}