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
using System.Threading;
using System.Security.Cryptography;

namespace komm_rein.api.test.Services
{
    public class DataProtectionTest
    {
        readonly IDataProtectionProvider _dataprotectionProvider = DataProtectionProvider.Create("komm_rein.test");

        [Fact]
        public void TestEncryptSlot()
        {
            // prepare 
            Slot slot = new() {From= DateTime.Today.AddHours(9), To = DateTime.Today.AddHours(9).AddMinutes(15), FacilityId = Guid.NewGuid() };

            var protector = new ProtectionService(_dataprotectionProvider);

            // act
            string encrypted = protector.Encrypt(slot);
            Slot decrypted = protector.Decrypt<Slot>(encrypted);

            // assert
            decrypted.FacilityId.Should().Be(slot.FacilityId);
            decrypted.From.Should().Be(slot.From);
            decrypted.To.Should().Be(slot.To);

        }

        [Fact]
        public void TestEncryptSlotWithExpiration()
        {
            // prepare 
            Slot slot = new() { From = DateTime.Today.AddHours(9), To = DateTime.Today.AddHours(9).AddMinutes(15), FacilityId = Guid.NewGuid() };

            var protector = new ProtectionService(_dataprotectionProvider);

            // act
            string encrypted = protector.Encrypt(slot, TimeSpan.FromMinutes(5));
            Slot decrypted = protector.Decrypt<Slot>(encrypted);

            // assert
            decrypted.FacilityId.Should().Be(slot.FacilityId);
            decrypted.From.Should().Be(slot.From);
            decrypted.To.Should().Be(slot.To);

        }

        [Fact]
        public void TestEncryptExpriration()
        {
            // prepare 
            Slot slot = new() { From = DateTime.Today.AddHours(9), To = DateTime.Today.AddHours(9).AddMinutes(15), FacilityId = Guid.NewGuid() };

            var protector = new ProtectionService(_dataprotectionProvider);

            // act
            string encrypted = protector.Encrypt(slot, TimeSpan.FromMilliseconds(10));
            Thread.Sleep(10);

            Action test = () => protector.Decrypt<Slot>(encrypted);

            // Assert
            test.Should().Throw<CryptographicException>();
        }

        [Fact]
        public void TestSign()
        {
            // prepare 
            Slot slot = new() { From = DateTime.Today.AddHours(9), To = DateTime.Today.AddHours(9).AddMinutes(15), FacilityId = Guid.NewGuid() };

            IProtectionService protector = new ProtectionService(_dataprotectionProvider);

            // act
            string signature = protector.Sign(slot);
            bool result = protector.Verify<Slot>(signature, slot);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void TestSignWithExpiration()
        {
            // prepare 
            Slot slot = new() { From = DateTime.Today.AddHours(9), To = DateTime.Today.AddHours(9).AddMinutes(15), FacilityId = Guid.NewGuid() };

            IProtectionService protector = new ProtectionService(_dataprotectionProvider);

            // act
            string signature = protector.Sign(slot, TimeSpan.FromMinutes(5));
            bool result = protector.Verify<Slot>(signature, slot);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void TestSignExpiration()
        {
            // prepare 
            Slot slot = new() { From = DateTime.Today.AddHours(9), To = DateTime.Today.AddHours(9).AddMinutes(15), FacilityId = Guid.NewGuid() };

            IProtectionService protector = new ProtectionService(_dataprotectionProvider);

            // act
            string signature = protector.Sign(slot, TimeSpan.FromMilliseconds(10));
            Thread.Sleep(10);

            Action test = () => protector.Verify<Slot>(signature, slot);

            // Assert
            test.Should().Throw<CryptographicException>();
        }

        [Fact]
        public void TestSignValueChanged()
        {
            // prepare 
            Slot slot = new() { From = DateTime.Today.AddHours(9), To = DateTime.Today.AddHours(9).AddMinutes(15), FacilityId = Guid.NewGuid() };

            IProtectionService protector = new ProtectionService(_dataprotectionProvider);

            // act
            string signature = protector.Sign(slot, TimeSpan.FromMinutes(5));
            slot.To = slot.To.AddHours(1);
            bool result = protector.Verify<Slot>(signature, slot);

            
            // assert
            result.Should().BeFalse();
        }

    }
}
