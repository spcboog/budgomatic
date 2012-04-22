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
    public class IncomeAccountMapFixture : MapFixture
    {
        [Test]
        public void CanMap()
        {
            new PersistenceSpecification<IncomeAccount>(Session)
                .CheckProperty<IncomeAccount>(x => x.Id, Guid.NewGuid())
                .CheckProperty<IncomeAccount>(x => x.Name, "some name")
                .CheckProperty<IncomeAccount>(x => x.Deleted, true)
                .VerifyTheMappings();
        }
    }
}
