using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using FluentAssertions;

using komm_rein.model;

namespace komm_rein.api.test.Helper
{
    public class DateTimeHelperTest
    {
        [Fact]
        public void TestDaysOfWeekConversion()
        {

            // Act
            model.DayOfWeek result_1 = System.DayOfWeek.Sunday.FromSystem();
            model.DayOfWeek result_2 = System.DayOfWeek.Monday.FromSystem();
            model.DayOfWeek result_3 = System.DayOfWeek.Tuesday.FromSystem();
            model.DayOfWeek result_4 = System.DayOfWeek.Wednesday.FromSystem();
            model.DayOfWeek result_5 = System.DayOfWeek.Thursday.FromSystem();
            model.DayOfWeek result_6 = System.DayOfWeek.Friday.FromSystem();
            model.DayOfWeek result_7 = System.DayOfWeek.Saturday.FromSystem();

            // Assert
            result_1.Should().Be(model.DayOfWeek.Sunday);
            result_2.Should().Be(model.DayOfWeek.Monday);
            result_3.Should().Be(model.DayOfWeek.Tuesday);
            result_4.Should().Be(model.DayOfWeek.Wednesday);
            result_5.Should().Be(model.DayOfWeek.Thursday);
            result_6.Should().Be(model.DayOfWeek.Friday);
            result_7.Should().Be(model.DayOfWeek.Saturday);
        }

        [Fact]
        public void FindCorrectWeekDayOpeningHours_ShouldNotFind()
        {
            // Arrange
            // 2021-03-11 10:30 - Thursday
            DateTime date = new DateTime(2021, 3, 11) + TimeSpan.FromHours(13.5);

            var openingHours = new List<OpeningHours>
            {
                new () {From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = model.DayOfWeek.All},
                new () {From = new DateTime().AddHours(15), To = new DateTime().AddHours(20),DayOfWeek = model.DayOfWeek.All},
                new () {From = new DateTime().AddHours(12), To = new DateTime().AddHours(15),DayOfWeek = model.DayOfWeek.Weekend},
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
                new () {From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = model.DayOfWeek.All},
                new () {From = new DateTime().AddHours(15), To = new DateTime().AddHours(20),DayOfWeek = model.DayOfWeek.All},
                new () {From = new DateTime().AddHours(12), To = new DateTime().AddHours(15),DayOfWeek = model.DayOfWeek.Weekend},
                new () {From = new DateTime().AddHours(20), To = new DateTime().AddHours(22),DayOfWeek = model.DayOfWeek.Saturday},
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
                new () {From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = model.DayOfWeek.All},
                new () {From = new DateTime().AddHours(15), To = new DateTime().AddHours(20),DayOfWeek = model.DayOfWeek.All},
                new () {From = new DateTime().AddHours(12), To = new DateTime().AddHours(15),DayOfWeek = model.DayOfWeek.Weekend},
                new () {From = new DateTime().AddHours(20), To = new DateTime().AddHours(22),DayOfWeek = model.DayOfWeek.Saturday},
            };

            // Act
            var result = openingHours.RemainingForDay(saturday_4pm.Date, saturday_4pm);

            // Assert
            // 15.00-20.00 and 20:00-22:00
            result.Should().HaveCount(2);
        }
    }
}


