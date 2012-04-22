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
    public class ExpenseTransactionMapFixture : MapFixture
    {
        [Test]
        public void CanMap()
        {
            new PersistenceSpecification<ExpenseTransaction>(Session)
                .CheckProperty<ExpenseTransaction>(x => x.Id, Guid.NewGuid())
                .CheckProperty<ExpenseTransaction>(x => x.Date, DateTime.Today)
                .CheckProperty<ExpenseTransaction>(x => x.Comments, "some comment")
                .CheckProperty<ExpenseTransaction>(x => x.Deleted, true)
                .VerifyTheMappings();
        }
    }
}
