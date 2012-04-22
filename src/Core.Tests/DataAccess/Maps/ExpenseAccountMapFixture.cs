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
    public class ExpenseAccountMapFixture : MapFixture
    {
        [Test]
        public void CanMap()
        {
            new PersistenceSpecification<ExpenseAccount>(Session)
                .CheckProperty<ExpenseAccount>(x => x.Id, Guid.NewGuid())
                .CheckProperty<ExpenseAccount>(x => x.Name, "some name")
                .CheckProperty<ExpenseAccount>(x => x.Deleted, true)
                .VerifyTheMappings();
        }
    }
}
