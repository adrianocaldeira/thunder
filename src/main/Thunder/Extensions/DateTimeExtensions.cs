using System;

namespace Thunder.Extensions
{
    /// <summary>
    /// Date time extensions
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get first day of year
        /// </summary>
        /// <param name="dt">Date</param>
        /// <returns>First day of year</returns>
        public static DateTime FirstDayOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        /// <summary>
        /// Get first day of year of <see cref="DayOfWeek"/>
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="dayOfWeek"><see cref="DayOfWeek"/></param>
        /// <returns>Last first of year of <see cref="DayOfWeek"/></returns>
        public static DateTime FirstDayOfYear(this DateTime dt, DayOfWeek dayOfWeek)
        {
            return dt.FirstDayOfYear().NextDay(dayOfWeek, true);
        }

        /// <summary>
        /// Get last day of year
        /// </summary>
        /// <param name="dt">Date</param>
        /// <returns>Last day of year</returns>
        public static DateTime LastDayOfYear(this DateTime dt)
        {
            return dt.FirstDayOfYear().AddYears(1).AddDays(-1);
        }

        /// <summary>
        /// Get last day of year of <see cref="DayOfWeek"/>
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="dayOfWeek"><see cref="DayOfWeek"/></param>
        /// <returns>Last day of year of <see cref="DayOfWeek"/></returns>
        public static DateTime LastDayOfYear(this DateTime dt, DayOfWeek dayOfWeek)
        {
            return dt.LastDayOfYear().PreviousDay(dayOfWeek, true);
        }

        /// <summary>
        /// Get first day of month
        /// </summary>
        /// <param name="dt">Date</param>
        /// <returns>First day of month</returns>
        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// Get first day of month of <see cref="DayOfWeek"/>
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="dayOfWeek"><see cref="DayOfWeek"/></param>
        /// <returns>Last first of month of <see cref="DayOfWeek"/></returns>
        public static DateTime FirstDayOfMonth(this DateTime dt, DayOfWeek dayOfWeek)
        {
            return dt.FirstDayOfMonth().NextDay(dayOfWeek, true);
        }
        /// <summary>
        /// Get last day of month
        /// </summary>
        /// <param name="dt">Date</param>
        /// <returns>Last day of month</returns>
        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            return dt.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Get last day of month of <see cref="DayOfWeek"/>
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="dayOfWeek"><see cref="DayOfWeek"/></param>
        /// <returns>Last day of month of <see cref="DayOfWeek"/></returns>
        public static DateTime LastDayOfMonth(this DateTime dt, DayOfWeek dayOfWeek)
        {
            return dt.LastDayOfMonth().PreviousDay(dayOfWeek, true);
        }

        /// <summary>
        /// Get previous day
        /// </summary>
        /// <param name="dt">Date</param>
        /// <returns>Previous day</returns>
        public static DateTime PreviousDay(this DateTime dt)
        {
            return dt.Date.AddDays(-1);
        }

        /// <summary>
        /// Get previous day of <see cref="DayOfWeek"/> exclude current date
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="dayOfWeek"><see cref="DayOfWeek"/></param>
        /// <returns>Previous day</returns>
        public static DateTime PreviousDay(this DateTime dt, DayOfWeek dayOfWeek)
        {
            return dt.PreviousDay(dayOfWeek, false);
        }

        /// <summary>
        /// Get previous day of <see cref="DayOfWeek"/> include current date if true
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="dayOfWeek"><see cref="DayOfWeek"/></param>
        /// <param name="includeThis">Include current date</param>
        /// <returns>Previous day</returns>
        public static DateTime PreviousDay(this DateTime dt, DayOfWeek dayOfWeek, bool includeThis)
        {
            int diff = dt.DayOfWeek - dayOfWeek;
            if ((includeThis && diff < 0) || (!includeThis && diff <= 0)) diff += 7;
            return dt.Date.AddDays(-diff);
        }

        /// <summary>
        /// Get next day
        /// </summary>
        /// <param name="dt">Date</param>
        /// <returns>Next day</returns>
        public static DateTime NextDay(this DateTime dt)
        {
            return dt.Date.AddDays(1);
        }

        /// <summary>
        /// Get next day of <see cref="DayOfWeek"/> exclude current date
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="dayOfWeek"><see cref="DayOfWeek"/></param>
        /// <returns>Next day</returns>
        public static DateTime NextDay(this DateTime dt, DayOfWeek dayOfWeek)
        {
            return dt.NextDay(dayOfWeek, false);
        }

        /// <summary>
        /// Get next day of <see cref="DayOfWeek"/> include current date if true
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="dayOfWeek"><see cref="DayOfWeek"/></param>
        /// <param name="includeThis">Include current date</param>
        /// <returns>Next day</returns>
        public static DateTime NextDay(this DateTime dt, DayOfWeek dayOfWeek, bool includeThis)
        {
            var diff = dayOfWeek - dt.DayOfWeek;
            if ((includeThis && diff < 0) || (!includeThis && diff <= 0)) diff += 7;
            return dt.Date.AddDays(diff);
        }

        /// <summary>
        /// Get days in year
        /// </summary>
        /// <param name="dt">Date</param>
        /// <returns>Days in year</returns>
        public static int DaysInYear(this DateTime dt)
        {
            return (dt.LastDayOfYear() - dt.FirstDayOfYear()).Days + 1;
        }

        /// <summary>
        /// Get number of <see cref="DayOfWeek"/> in year
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="dayOfWeek"><see cref="DayOfWeek"/></param>
        /// <returns>Number of <see cref="DayOfWeek"/> in year</returns>
        public static int DaysInYear(this DateTime dt, DayOfWeek dayOfWeek)
        {
            return (dt.LastDayOfYear(dayOfWeek).DayOfYear - dt.FirstDayOfYear(dayOfWeek).DayOfYear) / 7 + 1;
        }

        /// <summary>
        /// Days in month
        /// </summary>
        /// <param name="dt">Date</param>
        /// <returns>Days in month</returns>
        public static int DaysInMonth(this DateTime dt)
        {
            return (dt.LastDayOfMonth() - dt.FirstDayOfMonth()).Days + 1;
        }

        /// <summary>
        /// Get number of <see cref="DayOfWeek"/> in month
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="dayOfWeek"><see cref="DayOfWeek"/></param>
        /// <returns>Number of <see cref="DayOfWeek"/> in month</returns>
        public static int DaysInMonth(this DateTime dt, DayOfWeek dayOfWeek)
        {
            return (dt.LastDayOfMonth(dayOfWeek).Day - dt.FirstDayOfMonth(dayOfWeek).Day) / 7 + 1;
        }

        /// <summary>
        /// Check is leap year
        /// </summary>
        /// <param name="dt">Date</param>
        /// <returns>Is leap year</returns>
        public static bool IsLeapYear(this DateTime dt)
        {
            return dt.DaysInYear() == 366;
        }

        /// <summary>
        /// Add weeks
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="weeks">Weeks</param>
        /// <returns>Date</returns>
        public static DateTime AddWeeks(this DateTime dt, int weeks)
        {
            return dt.AddDays(7 * weeks);
        }

        private static int DateValue(this DateTime dt)
        {
            return dt.Year * 372 + (dt.Month - 1) * 31 + dt.Day - 1;
        }

        /// <summary>
        /// Years between date
        /// </summary>
        /// <param name="one">Date one</param>
        /// <param name="two">Date two</param>
        /// <returns>Years</returns>
        public static int YearsBetween(this DateTime one, DateTime two)
        {
            return one.MonthsBetween(two) / 12;
        }

        /// <summary>
        /// Years between date
        /// </summary>
        /// <param name="one">Date one</param>
        /// <param name="two">Date two</param>
        /// <param name="includeLastDay">Include last day</param>
        /// <returns>Years</returns>
        public static int YearsBetween(this DateTime one, DateTime two, bool includeLastDay)
        {
            return one.MonthsBetween(two, includeLastDay) / 12;
        }

        /// <summary>
        /// Years between date
        /// </summary>
        /// <param name="one">Date one</param>
        /// <param name="two">Date two</param>
        /// <param name="includeLastDay">Include last day</param>
        /// <param name="excessMonths">Excess months</param>
        /// <returns>Years</returns>
        public static int YearsBetween(this DateTime one, DateTime two, bool includeLastDay, out int excessMonths)
        {
            var months = one.MonthsBetween(two, includeLastDay);
            excessMonths = months % 12;
            return months / 12;
        }

        /// <summary>
        /// Get months between date
        /// </summary>
        /// <param name="one">Date one</param>
        /// <param name="two">Date two</param>
        /// <returns>Months</returns>
        public static int MonthsBetween(this DateTime one, DateTime two)
        {
            var months = (two.DateValue() - one.DateValue()) / 31;
            return Math.Abs(months);
        }

        /// <summary>
        /// Get months between date
        /// </summary>
        /// <param name="one">Date one</param>
        /// <param name="two">Date two</param>
        /// <param name="includeLastDay">Include last day</param>
        /// <returns>Months</returns>
        public static int MonthsBetween(this DateTime one, DateTime two, bool includeLastDay)
        {
            if (!includeLastDay) return one.MonthsBetween(two);
            int days;
            if (two >= one)
                days = two.AddDays(1).DateValue() - one.DateValue();
            else
                days = one.AddDays(1).DateValue() - two.DateValue();
            return days / 31;
        }

        /// <summary>
        /// Get weeks between date
        /// </summary>
        /// <param name="one">Date one</param>
        /// <param name="two">Date two</param>
        /// <returns>Weeks</returns>
        public static int WeeksBetween(this DateTime one, DateTime two)
        {
            return one.DaysBetween(two) / 7;
        }

        /// <summary>
        /// Get weeks between date
        /// </summary>
        /// <param name="one">Date one</param>
        /// <param name="two">Date two</param>
        /// <param name="includeLastDay">Include last day</param>
        /// <returns>Weeks</returns>
        public static int WeeksBetween(this DateTime one, DateTime two, bool includeLastDay)
        {
            return one.DaysBetween(two, includeLastDay) / 7;
        }

        /// <summary>
        /// Get weeks between date
        /// </summary>
        /// <param name="one">Date one</param>
        /// <param name="two">Date two</param>
        /// <param name="includeLastDay">Include last day</param>
        /// <param name="excessDays">Excess days</param>
        /// <returns>Weeks</returns>
        public static int WeeksBetween(this DateTime one, DateTime two, bool includeLastDay, out int excessDays)
        {
            var days = one.DaysBetween(two, includeLastDay);
            excessDays = days % 7;
            return days / 7;
        }

        /// <summary>
        /// Get days between dates
        /// </summary>
        /// <param name="one">Date one</param>
        /// <param name="two">Date two</param>
        /// <returns>Days</returns>
        public static int DaysBetween(this DateTime one, DateTime two)
        {
            return (two.Date - one.Date).Duration().Days;
        }

        /// <summary>
        /// Get days between dates
        /// </summary>
        /// <param name="one">Date one</param>
        /// <param name="two">Date two</param>
        /// <param name="includeLastDay">Include last day</param>
        /// <returns>Days between dates</returns>
        public static int DaysBetween(this DateTime one, DateTime two, bool includeLastDay)
        {
            int days = one.DaysBetween(two);
            if (!includeLastDay) return days;
            return days + 1;
        }
    }
}