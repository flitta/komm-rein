﻿using FluentAssertions;
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
        Facility _facility = new () { ID = Guid.NewGuid(), Settings = new FacilitySettings { TimeToPlanAhead = TimeSpan.FromHours(1) } };
        Mock<IFacilityRepository> _repo = new ();

        [Fact]
        public void GetFacility_date_is_future()
        {
            // Arrange
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);

            var service = new FacilityService(_repo.Object);

            // Act
            Action test = () => service.GetAvailableSlots(_facility.ID, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));

            // Assert
            test.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetFacility_date_is_to_close()
        {
            // Arrange
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);

            var service = new FacilityService(_repo.Object);

            // Act
            Action test = () => service.GetAvailableSlots(_facility.ID, DateTime.Now.AddHours(0.5), DateTime.Now.AddDays(1));

            // Assert
            test.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetFacility_date_is_invalid()
        {
            // Arrange
            _repo.Setup(x => x.GetById(_facility.ID)).Returns(_facility);

            var service = new FacilityService(_repo.Object);

            // Act
            Action test = () => service.GetAvailableSlots(_facility.ID, DateTime.Now.AddDays(1), DateTime.Now.AddDays(-1));

            // Assert
            test.Should().Throw<ArgumentException>();
        }

    }
}
