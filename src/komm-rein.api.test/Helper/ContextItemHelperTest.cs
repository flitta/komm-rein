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
    public class ContextItemHelperTest
    {
        [Fact]
        public void TestAddCreateInfos()
        {
            // Arrange
            TestItem item = new();
            String sid = "testsid";

            // Act
            var result = item.AddCreatedInfo(sid);

            // Assert
            result.Should().Be(item);
            item.OwnerSid.Should().Be(sid);
            item.CreatedBySid.Should().Be(sid);
            item.CreatedDate.Should().BeAfter(new DateTime());
        }

        private class TestItem : ContextItem
        {
        }

    }
}


