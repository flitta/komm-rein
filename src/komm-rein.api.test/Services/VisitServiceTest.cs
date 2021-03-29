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

namespace komm_rein.api.test.Services
{
    public class VisitServiceTest
    {
        readonly Mock<IFacilityService> _facilityService = new();
        readonly Mock<IVisitRepository> _repo = new();

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
                new() { From = new DateTime().AddHours(0), To = new DateTime().AddHours(24), DayOfWeek = model.DayOfWeek.All }
            } 
        };

        DateTime _fixedNowDate = new DateTime(2021, 3, 11);

        [Fact]
        public async Task TestBookVisit()
        {
            // Visit must be in an available solot

            // Arrange
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
                new Slot {From = visit.From, To = visit.To, Facility = visit.Facility, Status = SlotStatus.Free},
                new Slot {From = visit.To, To = visit.To.AddMinutes(15), Facility = visit.Facility, Status = SlotStatus.Full}
            };

            _facilityService.Setup(x => x.GetSlotsForVisit(_facility.ID, visit.From.Date, visit)).ReturnsAsync(slots);
            _facilityService.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);

            var service = new VisitService(_repo.Object, _facilityService.Object);
            
            // Act
            var result = await service.BookVisit(visit, sid);


            // Assert
            _repo.Verify(mock => mock.Create(It.IsAny<Visit>()), Times.Once());

            result.Should().NotBeNull();
            result.Facility.ID.Should().Be(visit.Facility.ID);
            result.OwnerSid.Should().Be(sid);

            result.CreatedBySid.Should().Be(sid);
            result.CreatedDate.Should().BeAfter(new DateTime());

            result.Should().NotBe(visit);
            result.From.Should().Be(visit.From);
            result.To.Should().Be(visit.To);
            result.Households.Should().HaveSameCount(visit.Households);

            result.Households.Sum(h => h.NumberOfPersons).Should().Be(visit.Households.Sum(vh => vh.NumberOfPersons));
            result.Households.Sum(h => h.NumberOfChildren).Should().Be(visit.Households.Sum(vh => vh.NumberOfChildren));
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

            var service = new VisitService(_repo.Object, _facilityService.Object);

            // Act
            var result = await service.Cancel(visit.ID , sid);

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

            var service = new VisitService(_repo.Object, _facilityService.Object);

            // Act
            Func<Task> test = async () => await service.Cancel(visit.ID, "wrong");

            // Assert
            test.Should().Throw<SecurityException>();

        }

        [Fact]
        public async Task TestGetbyIdSecurity()
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

            var service = new VisitService(_repo.Object, _facilityService.Object);

            // Act
            Func<Task> test = async () => await service.GetById(visit.ID, "wrong");

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

            var service = new VisitService(_repo.Object, _facilityService.Object);

            // Act
            var result =  await service.GetById(visit.ID, visit.CreatedBySid);

            // Assert
            result.Should().NotBeNull();
            result.ID.Should().Be(visit.ID);
        }

    }
}
