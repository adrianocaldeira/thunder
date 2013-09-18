using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Thunder.Web.Mvc
{
    ///<summary>
    /// SelectList extension
    ///</summary>
    public static class SelectListExtensions
    {
        ///<summary>
        /// Convert list items to SelectList collection
        ///</summary>
        ///<param name="items">Items</param>
        ///<param name="name">Name</param>
        ///<param name="value">Value</param>
        ///<typeparam name="T">Type</typeparam>
        ///<returns>Select list item collection</returns>
        public static IList<SelectListItem> ToSelectList<T>(this IList<T> items, Func<T, string> name,
                                                            Func<T, string> value)
        {
            return items.ToSelectList(name, value, "", null);
        }

        /// <summary>
        /// Convert list items to SelectList collection
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="firstItem">First item</param>
        ///<returns>Select list item collection</returns>
        public static IList<SelectListItem> ToSelectList<T>(this IList<T> items, Func<T, string> name,
                                                            Func<T, string> value, SelectListItem firstItem)
        {
            return items.ToSelectList(name, value, "", firstItem);
        }

        /// <summary>
        /// Convert list items to SelectList collection
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="selectedValue">Selected value</param>
        ///<returns>Select list item collection</returns>
        public static IList<SelectListItem> ToSelectList<T>(this IList<T> items, Func<T, string> name,
                                                            Func<T, string> value, string selectedValue)
        {
            return items.ToSelectList(name, value, x => value(x) == selectedValue, null);
        }

        /// <summary>
        /// Convert list items to SelectList collection
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="selectedValue">Selected value</param>
        /// <param name="firstItem">First item</param>
        ///<returns>Select list item collection</returns>
        public static IList<SelectListItem> ToSelectList<T>(this IList<T> items, Func<T, string> name,
                                                            Func<T, string> value, string selectedValue,
                                                            SelectListItem firstItem)
        {
            return items.ToSelectList(name, value, x => value(x) == selectedValue, firstItem);
        }

        /// <summary>
        /// Convert list items to SelectList collection
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="isSelected">Check is selected</param>
        /// <param name="firstItem">First item</param>
        ///<returns>Select list item collection</returns>
        public static IList<SelectListItem> ToSelectList<T>(this IList<T> items, Func<T, string> name,
                                                            Func<T, string> value, Func<T, bool> isSelected,
                                                            SelectListItem firstItem)
        {
            if (items == null)
                return new List<SelectListItem>();

            var list = new List<SelectListItem>();

            if (firstItem != null)
            {
                list.Add(firstItem);
            }

            list.AddRange(items.Select(item =>
                                       new SelectListItem
                                       {
                                           Text = name(item),
                                           Value = value(item),
                                           Selected = isSelected(item)
                                       }
                              ));

            return list;
        }

        /// <summary>
        /// Convert list items to SelectList collection for multiselect
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="selectedValues">Selected values</param>
        ///<returns>Select list item collection</returns>
        public static IEnumerable<SelectListItem> ToMultiSelectList<T>(this IList<T> items, Func<T, string> name,
                                                                       Func<T, string> value,
                                                                       IEnumerable<string> selectedValues)
        {
            if (items == null)
                return new List<SelectListItem>();

            if (selectedValues == null)
                selectedValues = new List<string>();

            return items.ToMultiSelectList(name, value, x => selectedValues.Contains(value(x)));
        }

        /// <summary>
        /// Convert list items to SelectList collection for multiselect
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="isSelected">Check is selected</param>
        ///<returns>Select list item collection</returns>
        public static IEnumerable<SelectListItem> ToMultiSelectList<T>(this IList<T> items, Func<T, string> name,
                                                                       Func<T, string> value, Func<T, bool> isSelected)
        {
            if (items == null)
                return new List<SelectListItem>();

            return items.Select(item => new SelectListItem
            {
                Text = name(item),
                Value = value(item),
                Selected = isSelected(item)
            });
        }

        /// <summary>
        /// Convert enumerator to <see cref="SelectList"/>.
        /// </summary>
        /// <param name="selectedValue">Selected value</param>
        /// <typeparam name="T">Enumerator type</typeparam>
        /// <returns><see cref="SelectList"/></returns>
        public static SelectList ToSelectList<T>(T selectedValue) where T : struct, IConvertible
        {
            var values = new List<dynamic>();
            foreach (T suit in Enum.GetValues(typeof(T)))
            {
                var member = suit.GetType().GetMember(suit.ToString(CultureInfo.InvariantCulture));
                var attributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                var displayName = ((DisplayAttribute)attributes[0]).Name;

                values.Add(new { Id = suit, Name = displayName });
            }

            return new SelectList(values, "Id", "Name", selectedValue);
        }

        /// <summary>
        /// Convert enumerator to <see cref="SelectList"/>.
        /// </summary>
        /// <typeparam name="T">Enumerator type</typeparam>
        /// <returns><see cref="SelectList"/></returns>
        public static SelectList ToSelectList<T>() where T : struct, IConvertible
        {
            return ToSelectList(default(T));
        }
    }
}
