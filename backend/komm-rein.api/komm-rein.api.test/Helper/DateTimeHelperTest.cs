using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using FluentAssertions;

using komm_rein.api.Models;

namespace komm_rein.api.test.Helper
{
    public class DateTimeHelperTest
    {
        [Fact]
        public void TestDaysOfWeekConversion()
        {

            // Act
            Models.DayOfWeek result_1 = System.DayOfWeek.Sunday.FromSystem();
            Models.DayOfWeek result_2 = System.DayOfWeek.Monday.FromSystem();
            Models.DayOfWeek result_3 = System.DayOfWeek.Tuesday.FromSystem();
            Models.DayOfWeek result_4 = System.DayOfWeek.Wednesday.FromSystem();
            Models.DayOfWeek result_5 = System.DayOfWeek.Thursday.FromSystem();
            Models.DayOfWeek result_6 = System.DayOfWeek.Friday.FromSystem();
            Models.DayOfWeek result_7 = System.DayOfWeek.Saturday.FromSystem();

            // Assert
            result_1.Should().Be(Models.DayOfWeek.Sunday);
            result_2.Should().Be(Models.DayOfWeek.Monday);
            result_3.Should().Be(Models.DayOfWeek.Tuesday);
            result_4.Should().Be(Models.DayOfWeek.Wednesday);
            result_5.Should().Be(Models.DayOfWeek.Thursday);
            result_6.Should().Be(Models.DayOfWeek.Friday);
            result_7.Should().Be(Models.DayOfWeek.Saturday);
        }

        [Fact]
        public void FindCorrectWeekDayOpeningHours_ShouldNotFind()
        {
            // Arrange
            // 2021-03-11 10:30 - Thursday
            DateTime date = new DateTime(2021, 3, 11) + TimeSpan.FromHours(13.5);

            var openingHours = new List<OpeningHours>
            {
                new () {From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = Models.DayOfWeek.All},
                new () {From = new DateTime().AddHours(15), To = new DateTime().AddHours(20),DayOfWeek = Models.DayOfWeek.All},
                new () {From = new DateTime().AddHours(12), To = new DateTime().AddHours(15),DayOfWeek = Models.DayOfWeek.Weekend},
            };

            // Act
            var result = openingHours.ForDateTime(date.Date);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetAllOpeningHoursForDay()
        {
            // Arrange
            // 2021-03-11 10:30 - Thursday
            var thursday = new DateTime(2021, 3, 11) + TimeSpan.FromHours(13.5);
            var saturday_3pm = thursday.AddDays(2) + TimeSpan.FromHours(1.5);

            var openingHours = new List<OpeningHours>
            {
                new () {From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = Models.DayOfWeek.All},
                new () {From = new DateTime().AddHours(15), To = new DateTime().AddHours(20),DayOfWeek = Models.DayOfWeek.All},
                new () {From = new DateTime().AddHours(12), To = new DateTime().AddHours(15),DayOfWeek = Models.DayOfWeek.Weekend},
                new () {From = new DateTime().AddHours(20), To = new DateTime().AddHours(22),DayOfWeek = Models.DayOfWeek.Saturday},
            };

            // Act
            var resultWorkDay = openingHours.ForDay(thursday);
            var resultWeekend = openingHours.ForDay(saturday_3pm);

            // Assert
            resultWorkDay.Should().Contain(openingHours[0]);
            resultWorkDay.Should().Contain(openingHours[1]);
            resultWorkDay.Should().NotContain(openingHours[2]);

            resultWeekend.Should().HaveCount(4);
        }

        [Fact]
        public void GetRemainingOpeningHoursForDay()
        {
            // Arrange
            // 2021-03-11 10:30 - Thursday
            var thursday = new DateTime(2021, 3, 11) + TimeSpan.FromHours(13.5);
            var saturday_4pm = thursday.AddDays(2) + TimeSpan.FromHours(2.5);

            var openingHours = new List<OpeningHours>
            {
                new () {From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = Models.DayOfWeek.All},
                new () {From = new DateTime().AddHours(15), To = new DateTime().AddHours(20),DayOfWeek = Models.DayOfWeek.All},
                new () {From = new DateTime().AddHours(12), To = new DateTime().AddHours(15),DayOfWeek = Models.DayOfWeek.Weekend},
                new () {From = new DateTime().AddHours(20), To = new DateTime().AddHours(22),DayOfWeek = Models.DayOfWeek.Saturday},
            };

            // Act
            var result = openingHours.RemainingForDay(saturday_4pm.Date, saturday_4pm);

            // Assert
            // 15.00-20.00 and 20:00-22:00
            result.Should().HaveCount(2);
        }

        //[Fact]
        //public void GetFullDay()
        //{
        //    // Testing the case that a 24h day ends add the following day
        //    // 01.01.2021 + 1 day == 02.01.2021

        //    // Arrange
        //    // 2021-03-11 10:30 - Thursday
        //    var thursday = new DateTime(2021, 3, 11);
            

        //    var openingHours = new List<OpeningHours>
        //    {
        //        new () {From = new DateTime().AddHours(0), To = new DateTime().AddHours(24), DayOfWeek = Models.DayOfWeek.All},
        //    };

        //    // Act
        //    var result = openingHours.RemainingForDay(thursday, thursday);

        //    // Assert
        //    // should return 96 slots for 24 hours * 4
        //    result.Should().HaveCount(96);
        //}
    }

}


