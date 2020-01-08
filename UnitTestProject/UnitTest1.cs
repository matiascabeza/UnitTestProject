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
            holidayCalendar.AddHolidayRules(new DayOfWeekHolidayRule(DayOfWeek.Saturday));
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
            holidayCalendar.AddHolidayRules(new DayOfWeekHolidayRule(DayOfWeek.Sunday));
            holidayCalendar.AddHolidayRules(new DayOfWeekHolidayRule(DayOfWeek.Saturday));
            var aSunday = new DateTime(2014, 3, 2);
            var aSaturday = new DateTime(2014, 3, 2);
            Assert.IsTrue(holidayCalendar.IsHoliday(aSunday));
            Assert.IsTrue(holidayCalendar.IsHoliday(aSaturday));
        }
        [TestMethod]
        public void Test1()
        {
            var holidayCalendar = new HolidayCalendar();
            holidayCalendar.AddHolidayRules(new DayOfMonthHolidayRule(1,1));
            var aJanuaryFirst = new DateTime(2014, 1, 1);
            Assert.IsTrue(holidayCalendar.IsHoliday(aJanuaryFirst));
        } 
        [TestMethod]
        public void Test2()
        {
            var holidayCalendar = new HolidayCalendar();
            holidayCalendar.AddHolidayRules(new DayOfMonthHolidayRule(1, 1));
            var aNavidad = new DateTime(2014, 12, 25);
            Assert.IsFalse(holidayCalendar.IsHoliday(aNavidad));
        } 
        [TestMethod]
        public void Test3()
        {
            var holidayCalendar = new HolidayCalendar();
            holidayCalendar.AddHolidayRules(new DayOfMonthHolidayRule(1, 1));
            holidayCalendar.AddHolidayRules(new DayOfMonthHolidayRule(12, 25));
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
            holidayCalendar.AddHolidayRules(new DateHolidayRule(aDate));
            Assert.IsTrue(holidayCalendar.IsHoliday(aDate));
        }
        [TestMethod]
        public void Test5()
        {
            var holidayCalendar = new HolidayCalendar();
            var aDate = new DateTime(1998, 3, 2);
            holidayCalendar.AddHolidayRules(new CompoundRangeHolidayRule(new DateTime(1990, 1, 1), new DateTime(1999, 12, 31), new DayOfWeekHolidayRule(DayOfWeek.Monday)));
            Assert.IsTrue(holidayCalendar.IsHoliday(aDate));
        }
    }
    public class HolidayCalendar
    {
        private IList<HolidayRule> _holidayRules = new List<HolidayRule>();

        public bool IsHoliday(DateTime aDate)
        {
            return _holidayRules.Any(aHolidayRule => aHolidayRule.IsHoliday(aDate));
        }
        public void AddHolidayRules(HolidayRule dayOfWeekHolidayRule)
        {
            _holidayRules.Add(dayOfWeekHolidayRule);
        }
    }
    public interface HolidayRule
    {
        bool IsHoliday(DateTime aDate);
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
    public class DateHolidayRule : HolidayRule
    {
        private DateTime aDate;
        public DateHolidayRule(DateTime aDate)
        {
            this.aDate = aDate;
        }
        public DateTime ADate { get => aDate;}
        public bool IsHoliday(DateTime aDate)
        {
            return aDate.Equals(aDate);
        }
    }
    public class CompoundRangeHolidayRule : HolidayRule
    {
        private DateTime from;
        private DateTime to;
        private DayOfWeekHolidayRule holidayRule;
        public CompoundRangeHolidayRule(DateTime from, DateTime to, DayOfWeekHolidayRule holidayRule)
        {
            this.from = from;
            this.to = to;
            this.holidayRule = holidayRule;
        }
        public bool IsHoliday(DateTime aDate)
        {
            return aDate >= from && aDate <= to && holidayRule.IsHoliday(aDate); 
        }
    }
}
