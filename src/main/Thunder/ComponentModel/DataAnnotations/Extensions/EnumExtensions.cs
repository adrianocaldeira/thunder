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
            var outString = ((DisplayAttribute) attrs[0]).Name;

            if (((DisplayAttribute) attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute) attrs[0]).GetName();
            }

            return outString;
        }
    }
}