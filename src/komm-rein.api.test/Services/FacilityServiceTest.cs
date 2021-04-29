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
    public class FacilityServiceTest
    {
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

        readonly Mock<IFacilityRepository> _repo = new();

        DateTime _fixedNowDate = new DateTime(2021, 3, 11);

        [Fact]
        public async Task TestGetOpeningHours()
        {
            // Arrange

            var openingHours = new List<OpeningHours>
                {
                    new () { From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = model.DayOfWeek.All },
                    new () { From = new DateTime().AddHours(15), To = new DateTime().AddHours(20), DayOfWeek = model.DayOfWeek.All }
                };

            Facility testItem = new() { ID = Guid.NewGuid(), Name = "Hannes Blumeneck", OwnerSid = "testsid", OpeningHours = openingHours };

            _repo.Setup(x => x.GetWithOpeningHours(testItem.ID)).ReturnsAsync(testItem);

            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetOpeningHours(testItem.ID);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(openingHours.Count);
            result.Should().ContainEquivalentOf(openingHours.First());
            result.Should().ContainEquivalentOf(openingHours.Last());
        }

        [Fact]
        public async Task TestSetOpeningHours()
        {
            // Arrange

            var openingHours = new List<OpeningHours>
                {
                    new () { From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = model.DayOfWeek.All },
                    new () { From = new DateTime().AddHours(15), To = new DateTime().AddHours(20), DayOfWeek = model.DayOfWeek.All }
                };

            Facility testItem = new() { ID = Guid.NewGuid(), Name = "Hannes Blumeneck", OwnerSid = "testsid" };

            _repo.Setup(x => x.GetWithOpeningHours(testItem.ID)).ReturnsAsync(testItem);
            _repo.Setup(x => x.SaveItem(testItem)).ReturnsAsync(testItem);

            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            var result = await service.SetOpeningHours(testItem.ID, openingHours.ToArray(), testItem.OwnerSid);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(openingHours.Count);
            result.Should().ContainEquivalentOf(openingHours.First());
            result.Should().ContainEquivalentOf(openingHours.Last());
        }

        [Fact]
        public async Task TestUpdateOpeningHours()
        {
            // Arrange

            var openingHours = new List<OpeningHours>
                {
                    new () { From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = model.DayOfWeek.All },
                    new () { From = new DateTime().AddHours(15), To = new DateTime().AddHours(20), DayOfWeek = model.DayOfWeek.All }
                };

            Facility testItem = new()
            {
                ID = Guid.NewGuid(),
                Name = "Hannes Blumeneck",
                OwnerSid = "testsid",
                OpeningHours = new List<OpeningHours>
                {
                    new () { ID =Guid.NewGuid(), From = new DateTime().AddHours(7), To = new DateTime().AddHours(12), DayOfWeek = model.DayOfWeek.All },
                }
            };

            _repo.Setup(x => x.GetWithOpeningHours(testItem.ID)).ReturnsAsync(testItem);
            _repo.Setup(x => x.SaveItem(testItem)).ReturnsAsync(testItem);

            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            var result = await service.SetOpeningHours(testItem.ID, openingHours.ToArray(), testItem.OwnerSid);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(openingHours.Count);
            result.Should().ContainEquivalentOf(openingHours.First());
            result.Should().ContainEquivalentOf(openingHours.Last());
        }

        [Fact]
        public void TestSetOpeningHoursSecurity()
        {
            // Arrange
            Facility testItem = new() { ID = Guid.NewGuid(), Name = "Hannes Blumeneck", OwnerSid = "testsid" };
            Facility testItem2 = new() { ID = Guid.NewGuid(), Name = "Salon Sahra", OwnerSid = "testsid2" };

            _repo.Setup(x => x.GetWithOpeningHours(testItem.ID)).ReturnsAsync(testItem);

            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            Func<Task> test = async () => await service.SetOpeningHours(testItem.ID, new OpeningHours[0], testItem2.OwnerSid);

            // Assert
            test.Should().Throw<SecurityException>();
        }

        [Fact]
        public async Task TestGetSettings()
        {
            // Arrange
            FacilitySettings newSettings = new() { SlotSize = TimeSpan.FromMinutes(15), MaxNumberofVisitors = 4 };
            Facility testItem = new() { ID = Guid.NewGuid(), Name = "Hannes Blumeneck", OwnerSid = "testsid", Settings = newSettings };
            Facility testItem2 = new() { ID = Guid.NewGuid(), Name = "Salon Sahra", OwnerSid = "testsid2" };

            _repo.Setup(x => x.GetWithSettings(testItem.ID)).ReturnsAsync(testItem);

            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetSettings(testItem.ID, testItem.OwnerSid);

            // Assert
            result.Should().NotBeNull();
            result.SlotSize.Should().Be(newSettings.SlotSize);
        }

        [Fact]
        public void TestSetSettingSecurity()
        {
            // Arrange
            Facility testItem = new() { ID = Guid.NewGuid(), Name = "Hannes Blumeneck", OwnerSid = "testsid" };
            Facility testItem2 = new() { ID = Guid.NewGuid(), Name = "Salon Sahra", OwnerSid = "testsid2" };
            FacilitySettings newSettings = new() { SlotSize = TimeSpan.FromMinutes(15), MaxNumberofVisitors = 4 };

            _repo.Setup(x => x.GetWithSettings(testItem.ID)).ReturnsAsync(testItem);

            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            Func<Task> test = async () => await service.SetSettings(testItem.ID, newSettings, testItem2.OwnerSid);

            // Assert
            test.Should().Throw<SecurityException>();

        }

        [Fact]
        public async Task TestSetSetting()
        {
            // Arrange
            Facility testItem = new() { ID = Guid.NewGuid(), Name = "Hannes Blumeneck", OwnerSid = "testsid" };
            FacilitySettings newSettings = new() { SlotSizeMinutes = 15, MaxNumberofVisitors = 4 };

            _repo.Setup(x => x.GetWithSettings(testItem.ID)).ReturnsAsync(testItem);
            _repo.Setup(x => x.SaveItem(testItem)).ReturnsAsync(testItem);

            var service = new FacilityService(_repo.Object);

            // Act
            var result = await service.SetSettings(testItem.ID, newSettings, testItem.OwnerSid);

            // Assert

            // should save
            _repo.Verify(mock => mock.SaveItem(testItem), Times.Once());

            testItem.Settings.Should().NotBeNull();
            testItem.Settings.SlotSize.TotalMinutes.Should().Be(newSettings.SlotSizeMinutes);
        }

        [Fact]
        public async Task TestGetByIdWithSetting()
        {
            // Arrange
            Facility testItem = new() { ID = Guid.NewGuid(), Name = "Hannes Blumeneck", OwnerSid = "testsid" };
            FacilitySettings newSettings = new() { SlotSize = TimeSpan.FromMinutes(15), MaxNumberofVisitors = 4 };

            _repo.Setup(x => x.GetByIdWithAssociations(testItem.ID)).ReturnsAsync(testItem);
            _repo.Setup(x => x.SaveItem(testItem)).ReturnsAsync(testItem);

            var service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetByIdWithSettings(testItem.ID, testItem.OwnerSid);

            // Assert

            // should save
            _repo.Verify(mock => mock.GetByIdWithAssociations(testItem.ID), Times.Once());
        }


        [Fact]
        public async Task TestCreate()
        {
            // Arrange
            IFacilityService service = new FacilityService(_repo.Object);
            Facility newItem = new()
            {
                Name = "Hannes Blumeneck",
                MainAddress =
                new()
                {
                    Street_1 = "Test Stree1 23",
                    ZipCode = "60316",
                    City = "Frankfurt",
                }
            };

            String testSid = "testsid";

            // Act
            var result = await service.Create(newItem, testSid);

            // Assert
            // save should never be called on input DTO
            _repo.Verify(mock => mock.Create(newItem), Times.Never());
            _repo.Verify(mock => mock.Create(It.IsAny<Facility>()), Times.Once());

            // result should not be reference-equal to input
            result.Should().NotBe(newItem);

            result.OwnerSid.Should().Be(testSid);

            result.Name.Should().Be(newItem.Name);
            result.MainAddress.Should().Be(newItem.MainAddress);

            // created timestamp and sid should be set
            result.CreatedBySid.Should().Be(testSid);
            result.CreatedDate.Should().BeAfter(new DateTime());
        }

        [Fact]
        public void TestUpdateSecurity()
        {
            // Arrange
            Facility testItem = new() { ID = Guid.NewGuid(), Name = "Hannes Blumeneck", OwnerSid = "testsid" };
            Facility testItem2 = new() { ID = Guid.NewGuid(), Name = "Salon Sahra", OwnerSid = "testsid2" };

            _repo.Setup(x => x.GetByIdWithAssociations(testItem.ID)).ReturnsAsync(testItem);

            IFacilityService service = new FacilityService(_repo.Object);

            // Act
            Func<Task> test = async () => await service.Update(testItem, testItem2.OwnerSid);

            // Assert
            test.Should().Throw<SecurityException>();
        }

        [Fact]
        public async Task TestUpdate()
        {
            // Arrange
            String testSid = "testsid";
            Facility repoItem = new()
            {
                ID = Guid.NewGuid(),
                OwnerSid = testSid,
                Name = "Hannes Blumeneck",
                MainAddress =
               new()
               {
                   Street_1 = "Test Stree1 23",
                   ZipCode = "60316",
                   City = "Frankfurt",
               }
            };

            Facility updateItem = new()
            {
                ID = repoItem.ID,
                IsActive = true,
                IsLive = true,
                Name = "Test",
                MainAddress =
                 new()
                 {
                     Street_1 = "Test",
                     ZipCode = "30459",
                     City = "Hannover",
                 }
            };

            _repo.Setup(x => x.GetByIdWithAssociations(repoItem.ID)).ReturnsAsync(repoItem);

            IFacilityService service = new FacilityService(_repo.Object);

            //// Act
            var result = await service.Update(updateItem, testSid);

            // Assert
            // save should never be called on input DTO
            _repo.Verify(mock => mock.SaveItem(updateItem), Times.Never());
            _repo.Verify(mock => mock.SaveItem(It.IsAny<Facility>()), Times.Once());

            // result should not be reference-equal to input
            result.Should().NotBe(updateItem);

            result.OwnerSid.Should().Be(testSid);

            result.IsLive.Should().Be(updateItem.IsLive);
            result.Name.Should().Be(updateItem.Name);
            result.MainAddress.Street_1.Should().Be(updateItem.MainAddress.Street_1);
            result.MainAddress.ZipCode.Should().Be(updateItem.MainAddress.ZipCode);
            result.MainAddress.City.Should().Be(updateItem.MainAddress.City);

            //tempering with Active should be ignored
            result.IsActive.Should().BeFalse();

            // update timestamp and sid should be set
            result.UpdatedBySid.Should().Be(testSid);
            result.UpdatedDate.Should().BeAfter(new DateTime());

        }


        [Fact]
        public void GetFacility_date_is_future()
        {
            // Arrange
            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);

            var service = new FacilityService(_repo.Object);

            // Act
            Func<Task> test = async () => await service.GetAvailableSlots(_facility.ID, _fixedNowDate.AddDays(-1), _fixedNowDate);

            // Assert
            test.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async Task GetAllSlotsBySize()
        {
            // Arrange
            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetAvailableSlots(_facility.ID, _fixedNowDate.AddDays(1), _fixedNowDate);

            // Assert
            // expect all slots for one 24h day: 24*4==96
            result.Should().HaveCount(96);
        }

        [Fact]
        public async Task GetAllSlotsForOpeningHours()
        {
            // Arrange
            _facility.OpeningHours = new List<OpeningHours>
            {
                new () {From = new DateTime().AddHours(8), To = new DateTime().AddHours(20), DayOfWeek = model.DayOfWeek.All},
            };

            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Assert
            // expect all slots for one 12h day: 12*4==48 slots
            result.Should().HaveCount(48);
        }


        [Fact]
        public async Task GetAllRemainingSlots()
        {
            // Arrange
            // today 12:00
            _fixedNowDate += TimeSpan.FromHours(12);

            _facility.Settings.SlotSize = TimeSpan.FromMinutes(15);

            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);

            var service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Assert
            // expect all slots for one 24h day: 12*4==48
            result.Should().HaveCount(48);
        }

        [Fact]
        public async Task SlotsMustAlignWithStartTime()
        {
            // Arrange
            // today 12:10
            _fixedNowDate = DateTime.Now.Date + TimeSpan.FromHours(12) + TimeSpan.FromMinutes(20);

            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

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
        public async Task SlotsMustReturnDateTimes()
        {
            // Arrange
            // today 12:10
            _fixedNowDate = DateTime.Now.Date + TimeSpan.FromHours(12) + TimeSpan.FromMinutes(20);

            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            var service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Assert
            result.First().From.Date.Should().Be(_fixedNowDate.Date);
            result.First().To.Date.Should().Be(_fixedNowDate.Date);

        }

        [Fact]
        public async Task TestApplySlotStatusInvalid()
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

            Slot slot = new() { FacilityId = visit.Facility.ID, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).ReturnsAsync(new List<Visit> { visit });

            var service = new FacilityService(_repo.Object);
            var slots = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);
            var targetSlot = slots.First(s => s.From == visit.From);

            // Act
            await service.ApplySlotStatus(targetSlot, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert

            targetSlot.Status.Should().Be(SlotStatus.Invalid);
        }

        [Fact]
        public async Task TestVisitOverManySlots()
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

            Slot slot = new() { FacilityId = visit.Facility.ID, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).ReturnsAsync(new List<Visit> { visit });

            var service = new FacilityService(_repo.Object);
            var slots = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            await service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            // 3 slot should be set
            slots.Where(s => s.Status == SlotStatus.Crowded).Should().HaveCount(3);
        }

        [Fact]
        public async Task TestVisitOverManySlotsExactOverlay()
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

            Slot slot = new() { FacilityId = visit.Facility.ID, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).ReturnsAsync(new List<Visit> { visit });

            var service = new FacilityService(_repo.Object);
            var slots = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            await service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            // 3 slot should be set
            slots.Where(s => s.Status == SlotStatus.Crowded).Should().HaveCount(3);
        }


        [Fact]
        public async Task TestApplySlotStatusBatch()
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

            Slot slot = new() { FacilityId = visit.Facility.ID, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).ReturnsAsync(new List<Visit> { visit });

            var service = new FacilityService(_repo.Object);
            var slots = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            await service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);
            targetSlot.Status.Should().Be(SlotStatus.Crowded);

            // only 1 slot should be set
            slots.Where(s => s.Status == SlotStatus.Crowded).Should().HaveCount(1);
        }


        [Fact]
        public async Task TestSlotStatusConsiderHouseholdSetting()
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

            Slot slot = new() { FacilityId = visit.Facility.ID, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).ReturnsAsync(new List<Visit> { visit, visit_2 });

            var service = new FacilityService(_repo.Object);
            var slots = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            await service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);
            targetSlot.Status.Should().Be(SlotStatus.Crowded);
        }


        [Fact]
        public async Task TestSlotStatusConsiderChildrenSetting()
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
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 2 } }
            };

            Slot slot = new() { FacilityId = visit.Facility.ID, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).ReturnsAsync(new List<Visit> { visit, visit_2 });

            var service = new FacilityService(_repo.Object);
            var slots = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            await service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);
            targetSlot.Status.Should().Be(SlotStatus.Crowded);
        }


        [Fact]
        public async Task TestSlotStatusCountAll()
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

            Slot slot = new() { FacilityId = visit.Facility.ID, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).ReturnsAsync(new List<Visit> { visit, visit_2 });

            var service = new FacilityService(_repo.Object);
            var slots = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            await service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24));

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);
            targetSlot.Status.Should().Be(SlotStatus.Full);
        }


        [Fact]
        public async Task TestNewVisitOnSlots()
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

            Slot slot = new() { FacilityId = visit.Facility.ID, From = visit.From, To = visit.To };

            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, slot.From.Date, slot.From.Date.AddHours(24))).ReturnsAsync(new List<Visit> { visit });

            var service = new FacilityService(_repo.Object);
            var slots = await service.GetAvailableSlots(_facility.ID, _fixedNowDate, _fixedNowDate);

            // Act
            await service.ApplySlotStatus(slots, _facility, slot.From.Date, slot.From.Date.AddHours(24), visit_new);

            // Assert
            var targetSlot = slots.First(s => s.From == visit.From);

            // it should be crowded to allow the new visit to be added
            targetSlot.Status.Should().Be(SlotStatus.Crowded);
        }


        [Fact]
        public async Task TestGetSlotsForVisit()
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

            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, _fixedNowDate.Date, _fixedNowDate.Date.AddDays(1))).ReturnsAsync(new List<Visit> { visit });
            var service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetSlotsForVisit(_facility.ID, _fixedNowDate, visit, _fixedNowDate);

            // Assert
            var targetSlot = result.First(s => s.From == visit_new.From);
            targetSlot.Status.Should().Be(SlotStatus.Crowded);
        }


        [Fact]
        public async Task TestGetSlotsForVisitByName()
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

            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetByName(_facility.Name)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, _fixedNowDate.Date, _fixedNowDate.Date.AddDays(1))).ReturnsAsync(new List<Visit> { visit });
            var service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetSlotsForVisit(_facility.Name, _fixedNowDate, visit, _fixedNowDate);

            // Assert
            var targetSlot = result.First(s => s.From == visit_new.From);
            targetSlot.Status.Should().Be(SlotStatus.Crowded);
        }


        [Fact]
        public async Task TestGetSlotsForVisitFull()
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
                Households = new List<Household> { new() { NumberOfPersons = 4, NumberOfChildren = 1 } }
            };

            _repo.Setup(x => x.GetById(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetByIdWithAssociations(_facility.ID)).ReturnsAsync(_facility);
            _repo.Setup(x => x.GetVisits(_facility.ID, _fixedNowDate.Date, _fixedNowDate.Date.AddDays(1))).ReturnsAsync(new List<Visit> { visit });
            var service = new FacilityService(_repo.Object);

            // Act
            var result = await service.GetSlotsForVisit(_facility.ID, _fixedNowDate, visit_new, _fixedNowDate);

            // Assert
            var targetSlot = result.First(s => s.From == visit_new.From);
            targetSlot.Status.Should().Be(SlotStatus.Full);
        }

        [Fact]
        public async Task Verify()
        {
            var protection = new Mock<IProtectionService>();

            // arrange
            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            _repo.Setup(x => x.GetVisit(_facility.ID, visit.ID, "test_sid")).ReturnsAsync(visit);
            protection.Setup(x => x.Verify(It.IsAny<String>(), It.IsAny<Visit>())).Returns(true);

            var service = new FacilityService(_repo.Object, protection.Object);

            var result = await service.Verify(_facility.ID, visit.ID, "some_test_string", "test_sid");
            result.ID.Should().Be(visit.ID);
        }

        [Fact]
        public async Task VerifyFailed()
        {
            var protection = new Mock<IProtectionService>();

            // arrange
            Visit visit = new()
            {
                Facility = _facility,
                From = _fixedNowDate.AddHours(10),
                To = _fixedNowDate.AddHours(10).AddMinutes(15),
                Households = new List<Household> { new() { NumberOfPersons = 1, NumberOfChildren = 1 } }
            };

            _repo.Setup(x => x.GetVisit(_facility.ID, visit.ID, "test_sid")).ReturnsAsync(visit);
            protection.Setup(x => x.Verify(It.IsAny<String>(), It.IsAny<Visit>())).Returns(false);

            var service = new FacilityService(_repo.Object, protection.Object);

            var result = await service.Verify(_facility.ID, visit.ID, "some_test_string", "test_sid");
            result.ID.Should().Be(Guid.Empty);
        }

    }
}
