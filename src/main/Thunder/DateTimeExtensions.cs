using System;

namespace Thunder
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Check year is bisixth
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns>Year bisixth</returns>
        public static bool IsYearBisixth(this DateTime dateTime)
        {
            return (dateTime.Year%4 == 0 && (dateTime.Year%400 == 0 || dateTime.Year%100 != 0));
        }
    }
}