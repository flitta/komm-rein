using FluentAssertions;
using komm_rein.model;
using komm_rein.api.Repositories;
using komm_rein.api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Security;
using Microsoft.AspNetCore.DataProtection;

namespace komm_rein.api.test.Services
{
    public class VisitServiceTest
    {
        readonly Mock<IFacilityService> _facilityService = new();
        readonly Mock<IVisitRepository> _repo = new();
        readonly Mock<IFacilityRepository> _facilityRepo = new();
        readonly IDataProtectionProvider _dataprotectionProvider = DataProtectionProvider.Create("komm_rein.test");


        readonly Facility _facility = new()
        {
            ID = Guid.NewGuid(),
            Name = "Test Facility",
            Settings = new()
            {
                SlotSize = TimeSpan.FromMinutes(15),
                SlotStatusThreshold = .5,
                MaxNumberofVisitors = 4,
                CountingMode = CountingMode.EverySinglePerson,
            },
            OpeningHours = new List<OpeningHours>
            {
                new() { From = new DateTime().AddHours(0), To = new DateTime().AddHours(24), DayOfWeek = model.DayOfWeek.All }
            }
        };

        DateTime _fixedNowDate = new DateTime(2021, 3, 11);

        [Fact]
        public async Task TestBookVisit()
        {
            // Visit must be in an available solot

            // Arrange
            var protector = new ProtectionService(_dataprotectionProvider);

            string sid = "test123";

            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            Slot[] slots = new[]
            {
                new Slot {From = visit.From, To = visit.To, FacilityId = visit.Facility.ID, Status = SlotStatus.Free},
                new Slot {From = visit.To, To = visit.To.AddMinutes(15), FacilityId = visit.Facility.ID, Status = SlotStatus.Full}
            };

            _facilityService.Setup(x => x.GetSlotsForVisit(_facility.ID, visit.From.Date, It.IsAny<Visit>())).ReturnsAsync(slots);
            _facilityService.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _facilityRepo.Setup(x => x.GetByNameWithAssociations(_facility.Name)).ReturnsAsync(_facility);

            var service = new VisitService(_repo.Object, _facilityService.Object, _facilityRepo.Object, protector);

            // Act
            var result = await service.BookVisit(visit.Facility.Name, visit.From, visit.To, 1, 1, sid);

            // Assert
            _repo.Verify(mock => mock.Create(It.IsAny<Visit>()), Times.Once());

            result.Should().NotBeNull();
            result.Payload.Facility.ID.Should().Be(visit.Facility.ID);

            result.Should().NotBe(visit);
            result.Payload.From.Should().Be(visit.From);
            result.Payload.To.Should().Be(visit.To);
            result.Payload.Households.Should().HaveSameCount(visit.Households);

            result.Payload.Households.Sum(h => h.NumberOfPersons).Should().Be(visit.Households.Sum(vh => vh.NumberOfPersons));
            result.Payload.Households.Sum(h => h.NumberOfChildren).Should().Be(visit.Households.Sum(vh => vh.NumberOfChildren));
        }

        [Fact]
        public void TestBookWrongSlot()
        {
            // Visit must be in an available solot

            // Arrange
            var protector = new ProtectionService(_dataprotectionProvider);

            string sid = "test123";

            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            Slot[] slots = new[]
            {
                new Slot {From = visit.From, To = visit.To, FacilityId = visit.Facility.ID, Status = SlotStatus.Free},
                new Slot {From = visit.To, To = visit.To.AddMinutes(15), FacilityId = visit.Facility.ID, Status = SlotStatus.Full}
            };

            _facilityService.Setup(x => x.GetSlotsForVisit(_facility.ID, visit.From.Date, It.IsAny<Visit>())).ReturnsAsync(slots);
            _facilityService.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _facilityRepo.Setup(x => x.GetByNameWithAssociations(_facility.Name)).ReturnsAsync(_facility);

            var service = new VisitService(_repo.Object, _facilityService.Object, _facilityRepo.Object, protector);

            // Act
            Func<Task> test = async () => await service.BookVisit(visit.Facility.Name, visit.From.AddHours(1), visit.To.AddHours(1), 1, 1, sid);

            test.Should().Throw<ArgumentException>();
        }


        [Fact]
        public async Task TestCancelVisit()
        {
            // Visit must be in an available solot

            // Arrange
            string sid = "test123";

            Visit visit = new()
            {
                OwnerSid = sid,
                ID = Guid.NewGuid(),
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            _repo.Setup(x => x.GetById(visit.ID)).ReturnsAsync(visit);

            var service = new VisitService(_repo.Object, _facilityService.Object, null, null);

            // Act
            var result = await service.Cancel(visit.ID, sid);

            // Assert
            _repo.Verify(mock => mock.SaveItem(It.IsAny<Visit>()), Times.Once());

            result.Should().NotBeNull();
            result.IsCanceled.Should().BeTrue();
        }

        [Fact]
        public async Task TestCancelSecurity()
        {
            // Visit must be in an available solot

            // Arrange
            string sid = "test123";

            Visit visit = new()
            {
                ID = Guid.NewGuid(),
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            _repo.Setup(x => x.GetById(visit.ID)).ReturnsAsync(visit);

            var service = new VisitService(_repo.Object, _facilityService.Object, null, null);

            // Act
            Func<Task> test = async () => await service.Cancel(visit.ID, "wrong");

            // Assert
            test.Should().Throw<SecurityException>();

        }

        [Fact]
        public void TestGetbyIdSecurity()
        {
            // Visit must be in an available solot

            // Arrange
            string sid = "test123";

            Visit visit = new()
            {
                ID = Guid.NewGuid(),
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            _repo.Setup(x => x.GetById(visit.ID)).ReturnsAsync(visit);

            var service = new VisitService(_repo.Object, _facilityService.Object, null, null);

            // Act
            Func<Task> test = async () => await service.GetByIdForOwner(visit.ID, "wrong");

            // Assert
            test.Should().Throw<SecurityException>();
        }


        [Fact]
        public async Task TestGetbyId()
        {
            // Visit must be in an available solot

            // Arrange
            string sid = "test123";

            Visit visit = new()
            {
                ID = Guid.NewGuid(),
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            _repo.Setup(x => x.GetById(visit.ID)).ReturnsAsync(visit);

            var service = new VisitService(_repo.Object, _facilityService.Object, null, null);

            // Act
            var result = await service.GetByIdForOwner(visit.ID, visit.CreatedBySid);

            // Assert
            result.Should().NotBeNull();
            result.ID.Should().Be(visit.ID);
        }

        [Fact]
        public async Task TestGetAllForUser()
        {
            // Visit must be in an available solot

            // Arrange
            string sid = "test123";

            List<Visit> visits = new List<Visit>()
            {
                new (){
                    ID = Guid.NewGuid(),
                    Facility = _facility,
                    From = _fixedNowDate.AddHours(10),
                    To = _fixedNowDate.AddHours(10).AddMinutes(15),
                    Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
                },

               new () {
                    ID = Guid.NewGuid(),
                    Facility = _facility,
                    From = _fixedNowDate.AddDays(1).AddHours(10),
                    To = _fixedNowDate.AddDays(1).AddHours(10).AddMinutes(15),
                    Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
               }
               };

            _repo.Setup(x => x.GetAllForSid(sid)).ReturnsAsync(visits);

            var service = new VisitService(_repo.Object, _facilityService.Object, null, null);

            // Act
            var result = await service.GetAll(sid);

            // Assert
            result.Should().HaveSameCount(visits);
        }

    }
}
