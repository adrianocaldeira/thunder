using System;
using NUnit.Framework;

namespace Thunder.Extensions
{
    [TestFixture]
    class DateTimeExtensionsTest
    {
        [Test]
        public void FirstDayOfYear()
        {
            var dateTime1 = new DateTime(2013, 3, 22);
            var date1 = dateTime1.FirstDayOfYear();
            var date2 = dateTime1.FirstDayOfYear(DayOfWeek.Monday);

            Assert.AreEqual(new DateTime(2013, 1, 1), date1);
            Assert.AreEqual(new DateTime(2013, 1, 7), date2);
        }

        [Test]
        public void LastDayOfYear()
        {
            var dateTime1 = new DateTime(2013, 3, 22);
            var date1 = dateTime1.LastDayOfYear();
            var date2 = dateTime1.LastDayOfYear(DayOfWeek.Monday);

            Assert.AreEqual(new DateTime(2013, 12, 31), date1);
            Assert.AreEqual(new DateTime(2013, 12, 30), date2);
        }


        [Test]
        public void FirstDayOfMonth()
        {
            var dateTime1 = new DateTime(2013, 3, 22);
            var date1 = dateTime1.FirstDayOfMonth();
            var date2 = dateTime1.FirstDayOfMonth(DayOfWeek.Monday);

            Assert.AreEqual(new DateTime(2013, 3, 1), date1);
            Assert.AreEqual(new DateTime(2013, 3, 4), date2);
        }

        [Test]
        public void LastDayOfMonth()
        {
            var dateTime1 = new DateTime(2013, 3, 22);
            var dateTime2 = new DateTime(2013, 2, 22);
            var dateTime3 = new DateTime(2012, 2, 22);

            var date1 = dateTime1.LastDayOfMonth();
            var date2 = dateTime1.LastDayOfMonth(DayOfWeek.Monday);
            var date3 = dateTime2.LastDayOfMonth();
            var date4 = dateTime3.LastDayOfMonth();

            Assert.AreEqual(new DateTime(2013, 3, 31), date1);
            Assert.AreEqual(new DateTime(2013, 3, 25), date2);
            Assert.AreEqual(new DateTime(2013, 2, 28), date3);
            Assert.AreEqual(new DateTime(2012, 2, 29), date4);
        }

        [Test]
        public void PreviousDay()
        {
            var dateTime1 = new DateTime(2013, 3, 22);
            var date1 = dateTime1.PreviousDay();
            var date2 = dateTime1.PreviousDay(DayOfWeek.Monday);
            var date3 = dateTime1.PreviousDay(DayOfWeek.Monday, true);

            Assert.AreEqual(new DateTime(2013, 3, 21), date1);
            Assert.AreEqual(new DateTime(2013, 3, 18), date2);
            Assert.AreEqual(new DateTime(2013, 3, 18), date3);
        }

        [Test]
        public void NextDay()
        {
            var dateTime1 = new DateTime(2013, 3, 22);
            var date1 = dateTime1.NextDay();
            var date2 = dateTime1.NextDay(DayOfWeek.Monday);
            var date3 = dateTime1.NextDay(DayOfWeek.Monday, true);

            Assert.AreEqual(new DateTime(2013, 3, 23), date1);
            Assert.AreEqual(new DateTime(2013, 3, 25), date2);
            Assert.AreEqual(new DateTime(2013, 3, 25), date3);
        }

        [Test]
        public void DaysInYear()
        {
            var actual1 = new DateTime(2013, 3, 22).DaysInYear();
            var actual2 = new DateTime(2013, 3, 22).DaysInYear(DayOfWeek.Monday);
            var actual3 = new DateTime(2012, 3, 22).DaysInYear();

            Assert.AreEqual(365, actual1);
            Assert.AreEqual(52, actual2);
            Assert.AreEqual(366, actual3);
        }

        [Test]
        public void DaysInMonth()
        {
            var actual1 = new DateTime(2013, 3, 22).DaysInMonth();
            var actual2 = new DateTime(2013, 3, 22).DaysInMonth(DayOfWeek.Monday);
            var actual3 = new DateTime(2012, 2, 22).DaysInMonth();

            Assert.AreEqual(31, actual1);
            Assert.AreEqual(4, actual2);
            Assert.AreEqual(29, actual3);
        }

        [Test]
        public void IsLeapYear()
        {
            var actual1 = new DateTime(2013, 3, 22).IsLeapYear();
            var actual2 = new DateTime(2012, 3, 22).IsLeapYear();

            Assert.AreEqual(false, actual1);
            Assert.AreEqual(true, actual2);
        }

        [Test]
        public void AddWeeks()
        {
            var actual1 = new DateTime(2013, 3, 22).AddWeeks(3);
            var actual2 = new DateTime(2012, 2, 22).AddWeeks(3);

            Assert.AreEqual(new DateTime(2013, 4, 12), actual1);
            Assert.AreEqual(new DateTime(2012, 3, 14), actual2);
        }

        [Test]
        public void YearsBetween()
        {
            var actual1 = new DateTime(2013, 3, 22).YearsBetween(new DateTime(2011, 1, 1));
            var actual2 = new DateTime(2013, 3, 22).YearsBetween(new DateTime(2011, 1, 1), true);
            int excessMonths;
            var actual3 = new DateTime(2013, 3, 22).YearsBetween(new DateTime(2010, 3, 1), true, out excessMonths);

            Assert.AreEqual(2, actual1);
            Assert.AreEqual(2, actual2);
            Assert.AreEqual(3, actual3);
            Assert.AreEqual(0, excessMonths);
        }

        [Test]
        public void MonthsBetween()
        {
            var actual1 = new DateTime(2013, 3, 22).MonthsBetween(new DateTime(2011, 1, 1));
            var actual2 = new DateTime(2013, 3, 22).MonthsBetween(new DateTime(2011, 1, 1), true);

            Assert.AreEqual(26, actual1);
            Assert.AreEqual(26, actual2);
        }

        [Test]
        public void WeeksBetween()
        {
            var actual1 = new DateTime(2013, 3, 22).WeeksBetween(new DateTime(2011, 1, 1));
            var actual2 = new DateTime(2013, 3, 22).WeeksBetween(new DateTime(2011, 1, 1), true);
            int excessDays;
            var actual3 = new DateTime(2013, 3, 22).WeeksBetween(new DateTime(2011, 1, 1), true, out excessDays);

            Assert.AreEqual(115, actual1);
            Assert.AreEqual(116, actual2);
            Assert.AreEqual(116, actual3);
            Assert.AreEqual(0, excessDays);
        }

        [Test]
        public void DaysBetween()
        {
            var actual1 = new DateTime(2013, 3, 22).DaysBetween(new DateTime(2011, 1, 1));
            var actual2 = new DateTime(2013, 3, 22).DaysBetween(new DateTime(2011, 1, 1), true);

            Assert.AreEqual(811, actual1);
            Assert.AreEqual(812, actual2);
        }
    }
}
