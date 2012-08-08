namespace Thunder
{
    /// <summary>
    /// Util
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Check year is bisixth
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsYearBisixth(int year)
        {
            return (year % 4 == 0 && (year % 400 == 0 || year % 100 != 0));
        }
    }
}
