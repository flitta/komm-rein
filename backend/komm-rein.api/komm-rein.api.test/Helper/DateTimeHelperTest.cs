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

    }
}


