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
    public class DataProtectionTest
    {
        readonly Mock<IFacilityService> _facilityService = new();


        [Fact]
        public void TestEncryptSlot()
        {
            // prepare 
            Slot slot = new() {From= DateTime.Today.AddHours(9), To = DateTime.Today.AddHours(9).AddMinutes(15),Facility = new Facility {ID = Guid.NewGuid() } };

            var protector = new ProtectionService();

            // act
            string encrypted = protector.Encrypt(slot, TimeSpan.FromMinutes(5));
            Slot decrypted = protector.Decrypt<Slot>(encrypted);

            // assert
            decrypted.Facility.ID.Should().Be(slot.Facility.ID);
            decrypted.From.Should().Be(slot.From);
            decrypted.To.Should().Be(slot.To);

        }

        [Fact]
        public void TestSign()
        {
            // prepare 
            Slot slot = new() { From = DateTime.Today.AddHours(9), To = DateTime.Today.AddHours(9).AddMinutes(15), Facility = new Facility { ID = Guid.NewGuid() } };

            IProtectionService protector = new ProtectionService();

            // act
            string signature = protector.Sign(slot, TimeSpan.FromMinutes(5));
            bool result = protector.Verify<Slot>(signature, slot);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void TestSignValueChanged()
        {
            // prepare 
            Slot slot = new() { From = DateTime.Today.AddHours(9), To = DateTime.Today.AddHours(9).AddMinutes(15), Facility = new Facility { ID = Guid.NewGuid() } };

            IProtectionService protector = new ProtectionService();

            // act
            string signature = protector.Sign(slot, TimeSpan.FromMinutes(5));
            slot.To = slot.To.AddHours(1);
            bool result = protector.Verify<Slot>(signature, slot);

            // assert
            result.Should().BeFalse();
        }

    }
}
