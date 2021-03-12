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
                CountingMode = CountingMode.EverySinglePerson,
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
        public void TestApplySlotStatusInvalid()
        {
            // Arrange

            // one household with 6 person
            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 6 } }
            };

            Slot slot = new() { Facility = visit.Facility, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).Returns(new List<Visit> { visit });

            IFacilityService service = new FacilityService(_repo.Object);
            var slots = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);
            var targetSlot = slots.First(s => s.From == visit.From);

            // Act
            service.ApplySlotStatus(targetSlot, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
           
            targetSlot.Status.Should().Be(SlotStatus.Invalid);
        }

        [Fact]
        public void TestVisitOverManySlots()
        {
            // Arrange

            // one household with 2 person
            // from 09:55 to 10:20 should span over 3 slots
            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(9).AddMinutes(55),
                To = _fixedNowDate.AddHours(10).AddMinutes(20),
                Households = new List<Household> { new() { NumberOfPersons = 2 } }
            };

            Slot slot = new() { Facility = visit.Facility, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).Returns(new List<Visit> { visit });

            IFacilityService service = new FacilityService(_repo.Object);
            var slots = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            // 3 slot should be set
            slots.Where(s => s.Status == SlotStatus.Crowded).Should().HaveCount(3);
        }

        [Fact]
        public void TestVisitOverManySlotsExactOverlay()
        {
            // Arrange

            // one household with 2 person
            // from 09:45 to 10:30 should EXACTLY span over 3 slots
            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(9).AddMinutes(45),
                To = _fixedNowDate.AddHours(10).AddMinutes(30),
                Households = new List<Household> { new() { NumberOfPersons = 2 } }
            };

            Slot slot = new() { Facility = visit.Facility, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).Returns(new List<Visit> { visit });

            IFacilityService service = new FacilityService(_repo.Object);
            var slots = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            // 3 slot should be set
            slots.Where(s => s.Status == SlotStatus.Crowded).Should().HaveCount(3);
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
             service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);
            targetSlot.Status.Should().Be(SlotStatus.Crowded);

            // only 1 slot should be set
            slots.Where(s => s.Status == SlotStatus.Crowded).Should().HaveCount(1);
        }


        [Fact]
        public void TestSlotStatusConsiderHouseholdSetting()
        {
            // Arrange
            _facility.Settings.CountingMode = CountingMode.HouseHolds;

            // one household with 2 person
            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 2 } }
            };

            // another household with 2 person
            Visit visit_2 = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 2 } }
            };

            Slot slot = new() { Facility = visit.Facility, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).Returns(new List<Visit> { visit, visit_2 });

            IFacilityService service = new FacilityService(_repo.Object);
            var slots = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);
            targetSlot.Status.Should().Be(SlotStatus.Crowded);
        }


        [Fact]
        public void TestSlotStatusConsiderChildrenSetting()
        {
            // Arrange
            _facility.Settings.CountingMode = CountingMode.SinglePersonWithoutChildren;

            // one household with 2 person
            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 4 } }
            };

            // another household with 2 person
            Visit visit_2 = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1 , NumberOfChildren = 2} }
            };

            Slot slot = new() { Facility = visit.Facility, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).Returns(new List<Visit> { visit, visit_2 });

            IFacilityService service = new FacilityService(_repo.Object);
            var slots = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);
            targetSlot.Status.Should().Be(SlotStatus.Crowded);
        }


        [Fact]
        public void TestSlotStatusCountAll()
        {
            // Arrange
            _facility.Settings.CountingMode = CountingMode.EverySinglePerson;

            // one household with 2 person
            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            // another household with 2 person
            Visit visit_2 = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            Slot slot = new() { Facility = visit.Facility, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).Returns(new List<Visit> { visit, visit_2 });

            IFacilityService service = new FacilityService(_repo.Object);
            var slots = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);
            targetSlot.Status.Should().Be(SlotStatus.Full);
        }


        [Fact]
        public void TestNewVisitOnSlots()
        {
            // Arrange
            // one household with 2 person
            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            // another household with 2 person
            Visit visit_new = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            Slot slot = new() { Facility = visit.Facility, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).Returns(new List<Visit> { visit});

            IFacilityService service = new FacilityService(_repo.Object);
            var slots = service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24), visit_new);

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);
            targetSlot.Status.Should().Be(SlotStatus.Full);
        }


        [Fact]
        public void TestGetSlotsForVisit()
        {
            // Arrange
            // one household with 2 person
            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            // another household with 2 person
            Visit visit_new = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, _fixedNowDate.Date, _fixedNowDate.Date.AddDays(1))).Returns(new List<Visit> { visit });
            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            var result = service.GetSlotsForVisit(_facility.ID, _fixedNowDate, visit, _fixedNowDate);

            // Assert
            var targetSlot = result.First(s => s.From == visit_new.From);
            targetSlot.Status.Should().Be(SlotStatus.Crowded);
        }

    }
}
