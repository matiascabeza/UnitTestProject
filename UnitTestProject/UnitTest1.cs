using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class YYYTest
    {
        //when... then... should...
        [TestMethod]
        public void ADayOfWeekCanBeHoliday()
        {
            var holidayCalendar = new HolidayCalendar();
            holidayCalendar.MakeDayOfWeekAsHoliday(new DayOfWeekHolidayRule(DayOfWeek.Saturday));
            var aSaturday = new DateTime(2014, 3, 1);
            Assert.IsTrue(holidayCalendar.IsHoliday(aSaturday));
        }
        [TestMethod]
        public void DayOfWeekCanNotBeHoliday()
        {
            var holidayCalendar = new HolidayCalendar();
            var aMonday = new DateTime(2014, 3, 3);

            Assert.IsFalse(holidayCalendar.IsHoliday(aMonday));
        }
        [TestMethod]
        public void MoreThanOneDayOfWeekCanBeHoliday()
        {
            var holidayCalendar = new HolidayCalendar();
            holidayCalendar.MakeDayOfWeekAsHoliday(new DayOfWeekHolidayRule(DayOfWeek.Sunday));
            holidayCalendar.MakeDayOfWeekAsHoliday(new DayOfWeekHolidayRule(DayOfWeek.Saturday));
            var aSunday = new DateTime(2014, 3, 2);
            var aSaturday = new DateTime(2014, 3, 2);
            Assert.IsTrue(holidayCalendar.IsHoliday(aSunday));
            Assert.IsTrue(holidayCalendar.IsHoliday(aSaturday));
        }
        [TestMethod]
        public void Test1()
        {
            var holidayCalendar = new HolidayCalendar();
            holidayCalendar.MakeDayMonthAsHoliday(new DayOfMonthHolidayRule(1,1));
            var aJanuaryFirst = new DateTime(2014, 1, 1);
            Assert.IsTrue(holidayCalendar.IsHoliday(aJanuaryFirst));
        } 
        [TestMethod]
        public void Test2()
        {
            var holidayCalendar = new HolidayCalendar();
            holidayCalendar.MakeDayMonthAsHoliday(new DayOfMonthHolidayRule(1, 1));
            var aNavidad = new DateTime(2014, 12, 25);
            Assert.IsFalse(holidayCalendar.IsHoliday(aNavidad));
        } 
        [TestMethod]
        public void Test3()
        {
            var holidayCalendar = new HolidayCalendar();
            holidayCalendar.MakeDayMonthAsHoliday(new DayOfMonthHolidayRule(1, 1));
            holidayCalendar.MakeDayMonthAsHoliday(new DayOfMonthHolidayRule(12, 25));
            var aNavidad = new DateTime(2014, 12, 25);
            var aJanuaryFirst = new DateTime(2014, 1, 1);
            Assert.IsTrue(holidayCalendar.IsHoliday(aNavidad));
            Assert.IsTrue(holidayCalendar.IsHoliday(aJanuaryFirst));
        }
        [TestMethod]
        public void Test4()
        {
            var holidayCalendar = new HolidayCalendar();
            var aDate = new DateTime(2014, 1, 12);
            holidayCalendar.MakeDateAsHoliday(aDate);
            Assert.IsTrue(holidayCalendar.IsHoliday(aDate));
        }
        //[TestMethod]
        //public void Test5()
        //{
        //    var holidayCalendar = new HolidayCalendar();
        //    var aDate = new DateTime(1998, 3, 2);
        //    holidayCalendar.MakeDayOfWeekAsHoliday(new DateTime(1990,1,1), new DateTime(1999,12,31), DayOfWeek.Monday);
        //    Assert.IsTrue(holidayCalendar.IsHoliday(aDate));
        //}
    }

    public class HolidayCalendar
    {
        private IList<HolidayRule> _daysOfWeekHoliday = new List<HolidayRule>();
        private IList<Tuple<int,int>> _daysOfMonthHoliday = new List<Tuple<int,int>>();
        private IList<DateTime> _datesHolidays = new List<DateTime>();

        public bool IsHoliday(DateTime aDate)
        {
            return IsDatesHolidays(aDate) ||
                IsDaysOfMonthHolidays(aDate) ||
                IsDayOfWeekHoliday(aDate);
        }
        private bool IsDatesHolidays(DateTime aDate)
        {
            return _datesHolidays.Contains(aDate);
        }

        private bool IsDaysOfMonthHolidays(DateTime aDate)
        {
            return _daysOfMonthHoliday.Contains(new Tuple<int, int>(aDate.Month, aDate.Day));
        }

        private bool IsDayOfWeekHoliday(DateTime aDate)
        {
            return _daysOfWeekHoliday.Any(aHolidayRule => aHolidayRule.IsHoliday(aDate));
        }

        public void MakeDayOfWeekAsHoliday(DayOfWeekHolidayRule dayOfWeekHolidayRule)
        {
            _daysOfWeekHoliday.Add(dayOfWeekHolidayRule);
        }

        public void MakeDayMonthAsHoliday(DayOfMonthHolidayRule dayOfMonthHolidayRule)
        {
            _daysOfWeekHoliday.Add(dayOfMonthHolidayRule);
        }

        public void MakeDateAsHoliday(DateTime aDate)
        {
            _datesHolidays.Add(aDate);
        }

        public void MakeDayOfWeekAsHoliday(DateTime from, DateTime to, DayOfWeek dayOfWeek)
        {
            throw new NotImplementedException();
        }
    }

    public class DayOfMonthHolidayRule  : HolidayRule
    {
        private int aMonthNumber;
        private int aDayNumber;

        public DayOfMonthHolidayRule(int aMonthNumber, int aDayNumber)
        {
            this.aMonthNumber = aMonthNumber;
            this.aDayNumber = aDayNumber;
        }

        public int ADayNumber { get => aDayNumber;}
        public int AMonthNumber { get => aMonthNumber;}

        public bool IsHoliday(DateTime aDate)
        {
            return aDate.Month.Equals(aMonthNumber) && aDate.Day.Equals(aDayNumber);
        }
    }
    public interface HolidayRule
    {
        bool IsHoliday(DateTime aDate);
    }
    public class DayOfWeekHolidayRule : HolidayRule
    {
        private DayOfWeek _aDayOfWeek;
        public DayOfWeekHolidayRule(DayOfWeek aDayOfWeek)
        {
            _aDayOfWeek = aDayOfWeek;
        }
        public DayOfWeek ADayOfWeek { get => _aDayOfWeek;}

        public bool IsHoliday(DateTime aDate)
        {
            return aDate.DayOfWeek.Equals(this.ADayOfWeek);
        }
    }


}
