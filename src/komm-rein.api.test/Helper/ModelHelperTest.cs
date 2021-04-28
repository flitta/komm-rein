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
    public class ModelHelperTest
    {
        readonly Facility _facility = new()
        {
            ID = Guid.NewGuid(),
            MainAddress = new (){Street_1 = "Steet 1", City = "Cloud City" },
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

        [Fact]
        public void TestFacilityDto()
        {
            // Act
            var dto = _facility.ToDto();

            // Assert
            dto.Should().NotBeNull();
            dto.Should().NotBe(_facility);
            dto.ModificationStampsShouldNotBeSet();

            dto.ID.Should().Be(_facility.ID);
            dto.Name.Should().Be(_facility.Name);
            dto.IsActive.Should().Be(_facility.IsActive);
            dto.IsLive.Should().Be(_facility.IsLive);
        }

        [Fact]
        public void TestFacilityWithAddressDto()
        {
            // Act
            var dto = _facility.ToDto(withMainAddress: true);

            // Assert
            dto.Should().NotBeNull();
            dto.Should().NotBe(_facility);
            dto.ModificationStampsShouldNotBeSet();

            dto.MainAddress.Should().NotBeNull();
        }

        [Fact]
        public void TestFacilityWithSettingDto()
        {
            // Act
            var dto = _facility.ToDto(withSettings: true);

            // Assert
            dto.Should().NotBeNull();
            dto.Should().NotBe(_facility);
            dto.ModificationStampsShouldNotBeSet();

            
            dto.Settings.Should().NotBeNull();
        }

        [Fact]
        public void TestFacilityWithOpeningHoursDto()
        {
            // Act
            var dto = _facility.ToDto(withOpeningHours: true);

            // Assert
            dto.Should().NotBeNull();
            dto.Should().NotBe(_facility);
            dto.ModificationStampsShouldNotBeSet();

            dto.OpeningHours.Should().NotBeNull();
        }

        [Fact]
        public void TestSettingsDto()
        {
            // Act
            var dto = _facility.Settings.ToDto();

            // Assert
            dto.Should().NotBeNull();
            dto.Should().NotBe(_facility.Settings);
            dto.ModificationStampsShouldNotBeSet();

            dto.ID.Should().Be(_facility.Settings.ID);
            dto.CountingMode.Should().Be(_facility.Settings.CountingMode);
            dto.CrowdedAt.Should().Be(_facility.Settings.CrowdedAt);
            dto.MaxNumberofVisitors.Should().Be(_facility.Settings.MaxNumberofVisitors);
            dto.SlotSizeMinutes.Should().Be((int)_facility.Settings.SlotSize.TotalMinutes);
            dto.SlotStatusThreshold.Should().Be(_facility.Settings.SlotStatusThreshold);
        }

        [Fact]
        public void TestOpeningHoursDto()
        {
            // prepare
            var item = _facility.OpeningHours.First();

            // Act
            var dto = item.ToDto();

            // Assert
            dto.Should().NotBeNull();
            dto.Should().NotBe(item);
            dto.ModificationStampsShouldNotBeSet();

            dto.ID.Should().Be(item.ID);
            dto.From.Should().Be(item.From);
            dto.To.Should().Be(item.To);
            dto.DayOfWeek.Should().Be(item.DayOfWeek);
        }

        [Fact]
        public void TestAddress()
        {
            // prepare
            var item = _facility.MainAddress;

            // Act
            var dto = item.ToDto();

            // Assert
            dto.Should().NotBeNull();
            dto.Should().NotBe(item);
            dto.ModificationStampsShouldNotBeSet();

            dto.ID.Should().Be(item.ID);
            dto.City.Should().Be(item.City);
            dto.ContactEmail.Should().Be(item.ContactEmail);
            dto.ContactName.Should().Be(item.ContactName);
            dto.ContactPhone.Should().Be(item.ContactPhone);
            dto.Country.Should().Be(item.Country);
            dto.Region.Should().Be(item.Region);
            dto.Street_1.Should().Be(item.Street_1);
            dto.Street_2.Should().Be(item.Street_2);
            dto.Street_3.Should().Be(item.Street_3);
            dto.ZipCode.Should().Be(item.ZipCode);
        }
    }
}


