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
        Facility _facility = new () { ID = Guid.NewGuid(), Settings = new ()};
        Mock<IFacilityRepository> _repo = new ();
        DateTime _now = DateTime.Now;

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
            _now = DateTime.Now.Date + TimeSpan.FromHours(12);

            _facility.Settings.SlotSize = TimeSpan.FromMinutes(15);
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _now, _now);

            // Assert
            // expect all slots for one 24h day: 12*4==48
            result.Should().HaveCount(48);
        }

    }
}
