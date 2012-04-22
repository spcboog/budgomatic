using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentNHibernate.Testing;
using NHibernate;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.Tests.DataAccess.Maps
{
    [TestFixture]
    public class LiabilityAccountMapFixture : MapFixture
    {
        [Test]
        public void CanMap()
        {
            new PersistenceSpecification<LiabilityAccount>(Session)
                .CheckProperty<LiabilityAccount>(x => x.Id, Guid.NewGuid())
                .CheckProperty<LiabilityAccount>(x => x.Name, "some name")
                .CheckProperty<LiabilityAccount>(x => x.Deleted, true)
                .VerifyTheMappings();
        }
    }
}
