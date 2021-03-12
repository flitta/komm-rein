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
        Facility _facility = new () { ID = Guid.NewGuid(), 
            Settings = new() {SlotSize = TimeSpan.FromMinutes(15) }, 
            OpeningHours = new List<OpeningHours>() 
            { 
                new() { From = new DateTime().AddHours(0), To = new DateTime().AddHours(24), DayOfWeek = Models.DayOfWeek.All }
            } 
        };

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
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _now.AddDays(1), _now);

            // Assert
            // expect all slots for one 24h day: 24*4==96
            result.Should().HaveCount(96);
        }

        [Fact]
        public void GetAllSlotsForOpeningHours()
        {
            // Arrange
            _facility.OpeningHours = new List<OpeningHours>
            {
                new () {From = new DateTime().AddHours(8), To = new DateTime().AddHours(20), DayOfWeek = Models.DayOfWeek.All},
            };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _now, _now);

            // Assert
            // expect all slots for one 12h day: 12*4==48 slots
            result.Should().HaveCount(48);
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
        public void SlotsMustAlignWithStartTime()
        {
            // Arrange
            // today 12:10
            _now = DateTime.Now.Date + TimeSpan.FromHours(12) + TimeSpan.FromMinutes(20);
            
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _now, _now);

            // Assert
            // expect all slots for one 24h day: 12*4==48 - 1
            // one less, because we started 10 minutes later
            result.Should().HaveCount(46);

            // it must skip the 12.00 - 12:15 slot
            // it should start with 12:15
            result.First().From.TimeOfDay.Hours.Should().Be(12);
            result.First().From.TimeOfDay.Minutes.Should().Be(30);
        }
    }
}
