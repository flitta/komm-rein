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
        readonly Facility _facility = new () { ID = Guid.NewGuid(), 
            Settings = new()
            {
                SlotSize = TimeSpan.FromMinutes(15), 
                SlotStatusThreshold = .5,
                MaxNumberofVisitors = 4,
                CountHousehold = true
            }, 
            OpeningHours = new List<OpeningHours>
            { 
                new() { From = new DateTime().AddHours(0), To = new DateTime().AddHours(24), DayOfWeek = Models.DayOfWeek.All }
            } 
        };

        readonly Mock<IFacilityRepository> _repo = new ();

        DateTime _fixedNowDate = new DateTime(2021, 3, 11);

        [Fact]
        public void GetFacility_date_is_future()
        {
            // Arrange
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);

            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            Action test = () => service.GetAvailableSlots(_facility.ID, _fixedNowDate.AddDays(-1), _fixedNowDate);

            // Assert
            test.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetAllSlotsBySize()
        {
            // Arrange
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _fixedNowDate.AddDays(1), _fixedNowDate);

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
            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Assert
            // expect all slots for one 12h day: 12*4==48 slots
            result.Should().HaveCount(48);
        }


        [Fact]
        public void GetAllRemainingSlots()
        {
            // Arrange
            // today 12:00
            _fixedNowDate += TimeSpan.FromHours(12);

            _facility.Settings.SlotSize = TimeSpan.FromMinutes(15);
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Assert
            // expect all slots for one 24h day: 12*4==48
            result.Should().HaveCount(48);
        }

        [Fact]
        public void SlotsMustAlignWithStartTime()
        {
            // Arrange
            // today 12:10
            _fixedNowDate = DateTime.Now.Date + TimeSpan.FromHours(12) + TimeSpan.FromMinutes(20);
            
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Assert
            // expect all slots for one 24h day: 12*4==48 - 1
            // one less, because we started 10 minutes later
            result.Should().HaveCount(46);

            // it must skip the 12.00 - 12:15 slot
            // it should start with 12:15
            result.First().From.TimeOfDay.Hours.Should().Be(12);
            result.First().From.TimeOfDay.Minutes.Should().Be(30);
        }


        [Fact]
        public void SlotsMustReturnDateTimes()
        {
            // Arrange
            // today 12:10
            _fixedNowDate = DateTime.Now.Date + TimeSpan.FromHours(12) + TimeSpan.FromMinutes(20);

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Assert
            result.First().From.Date.Should().Be(_fixedNowDate.Date);
            result.First().To.Date.Should().Be(_fixedNowDate.Date);

        }


        [Fact]
        public void TestApplySlotStatusBatch()
        {
            // Arrange
            
            // one household with 2 person
            Visit visit = new() 
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 2 } } 
            };

            Slot slot = new () { Facility = visit.Facility,From = visit.From, To = visit.To };
           
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).Returns(new List<Visit> {visit });

            IFacilityService service = new FacilityService(_repo.Object);
            var slots = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
             service.ApplySlotStatusBatch(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);
            targetSlot.Status.Should().Be(Slot.SlotStatus.Crowded);
        }


    }
}
