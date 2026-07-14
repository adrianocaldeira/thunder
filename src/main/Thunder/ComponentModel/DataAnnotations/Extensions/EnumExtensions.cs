using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations.Extensions
{
    /// <summary>
    /// Enumerator extensions
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Display name from value of enumerator display name attribute
        /// </summary>
        /// <param name="value">Enumerator value</param>
        /// <returns>Display name</returns>
        public static string DisplayName(this Enum value)
        {
            var enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            var member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof (DisplayAttribute), false);

            if (attrs.Length == 0)
                return enumValue;

            var display = (DisplayAttribute) attrs[0];

            return display.ResourceType != null ? display.GetName() : display.Name ?? enumValue;
        }
    }
}