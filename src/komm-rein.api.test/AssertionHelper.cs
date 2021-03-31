using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

using komm_rein.model;


namespace komm_rein.api.test
{
    public static class AssertionHelper
    {
        public static void ModificationStampsShouldNotBeSet(this ContextItem item)
        {
            item.OwnerSid.Should().BeNull();
            item.CreatedBySid.Should().BeNull();
            item.UpdatedBySid.Should().BeNull();
            item.DeletedBySid.Should().BeNull();

            item.CreatedDate.Should().Be(new DateTime());
            item.UpdatedDate.Should().Be(new DateTime());
            item.DeleteddDate.Should().BeNull();
        }
    }
}
