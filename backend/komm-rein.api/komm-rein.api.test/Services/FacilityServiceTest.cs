using FluentAssertions;
using komm_rein.api.Models;
using komm_rein.api.Repositories;
using komm_rein.api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace komm_rein.api.test.Services
{
    public class FacilityServiceTest
    {
        Facility _facility = new () { ID = Guid.NewGuid(), Settings = new (), OpeningHours = new List<OpeningHours>()};
        Mock<IFacilityRepository> _repo = new ();
        DateTime _now = new DateTime(2021, 3, 11);

        [Fact]
        public void GetFacility_date_is_future()
        {
            // Arrange
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);

            var service = new FacilityService(_repo.Object);

            // Act
            Action test = () => service.GetAvailableSlots(_facility.ID, _now.AddDays(-1), _now);

            // Assert
            test.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetAllSlotsBySize()
        {
            // Arrange
            _facility.Settings.SlotSize = TimeSpan.FromMinutes(15);
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _now.AddDays(1), _now);

            // Assert
            // expect all slots for one 24h day: 24*4==96
            result.Should().HaveCount(96);
        }

        [Fact]
        public void GetAllRemainingSlots()
        {
            // Arrange
            // today 12:00
            _now += TimeSpan.FromHours(12);

            _facility.Settings.SlotSize = TimeSpan.FromMinutes(15);
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _now, _now);

            // Assert
            // expect all slots for one 24h day: 12*4==48
            result.Should().HaveCount(48);
        }

        [Fact]
        public void FindCorrectOpeningHours()
        {
            // Arrange
            // 2021-03-11 10:30
            _now = _now += TimeSpan.FromHours(16.5);

            _facility.OpeningHours = new List<OpeningHours>
            {
                new () {From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = Models.DayOfWeek.All},
                new () {From = new DateTime().AddHours(15), To = new DateTime().AddHours(20),DayOfWeek = Models.DayOfWeek.All},
            };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = service.FindOpeningHours(_facility.ID, _now);

            // Assert
            result.Should().Be(_facility.OpeningHours.Last());
        }

        [Fact]
        public void FindNoOpeningHours()
        {
            // Arrange
            // 2021-03-11 10:30
            _now = _now += TimeSpan.FromHours(13.5);

            _facility.OpeningHours = new List<OpeningHours>
            {
                new () {From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = Models.DayOfWeek.All},
                new () {From = new DateTime().AddHours(15), To = new DateTime().AddHours(20),DayOfWeek = Models.DayOfWeek.All},
            };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = service.FindOpeningHours(_facility.ID, _now);

            // Assert
            // expect all slots for one 24h day: 12*4==48
            result.Should().BeNull();
        }


        [Fact]
        public void FindCorrectWeekDayOpeningHours_ShouldNotFind()
        {
            // Arrange
            // 2021-03-11 10:30 - Thursday
            _now = _now += TimeSpan.FromHours(13.5);

            _facility.OpeningHours = new List<OpeningHours>
            {
                new () {From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = Models.DayOfWeek.All},
                new () {From = new DateTime().AddHours(15), To = new DateTime().AddHours(20),DayOfWeek = Models.DayOfWeek.All},
                new () {From = new DateTime().AddHours(12), To = new DateTime().AddHours(15),DayOfWeek = Models.DayOfWeek.Weekend},
            };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = service.FindOpeningHours(_facility.ID, _now);

            // Assert
            result.Should().BeNull();
        }


        //[Fact]
        //public void SlotsMustAlignWithStartTime()
        //{
        //    // Arrange
        //    // today 12:00
        //    _now = DateTime.Now.Date + TimeSpan.FromHours(12) + TimeSpan.FromMinutes(10);

        //    _facility.Settings.SlotSize = TimeSpan.FromMinutes(15);
        //    _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
        //    var service = new FacilityService(_repo.Object);

        //    // Act
        //    var result = service.GetAvailableSlots(_facility.ID, _now, _now);

        //    // Assert
        //    // expect all slots for one 24h day: 12*4==48 - 1
        //    // one less, because we started 10 minutes later
        //    result.Should().HaveCount(47);


        //    result.First().From.Should().Be()

        //}
    }
}
