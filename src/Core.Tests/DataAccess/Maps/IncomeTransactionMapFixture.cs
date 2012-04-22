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
    public class IncomeTransactionMapFixture : MapFixture
    {
        [Test]
        public void CanMap()
        {
            new PersistenceSpecification<IncomeTransaction>(Session)
                .CheckProperty<IncomeTransaction>(x => x.Id, Guid.NewGuid())
                .CheckProperty<IncomeTransaction>(x => x.Date, DateTime.Today)
                .CheckProperty<IncomeTransaction>(x => x.Comments, "some comment")
                .CheckProperty<IncomeTransaction>(x => x.Deleted, true)
                .VerifyTheMappings();
        }
    }
}
