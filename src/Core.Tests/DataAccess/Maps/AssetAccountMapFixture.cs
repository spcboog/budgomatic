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
    public class AssetAccountMapFixture : MapFixture
    {
        [Test]
        public void CanMap()
        {
            new PersistenceSpecification<AssetAccount>(Session)
                .CheckProperty<AssetAccount>(x => x.Id, Guid.NewGuid())
                .CheckProperty<AssetAccount>(x => x.Name, "some name")
                .CheckProperty<AssetAccount>(x => x.Deleted, true)
                .VerifyTheMappings();
        }
    }
}
