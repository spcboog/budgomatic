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
    public class TransferTransactionMapFixture : MapFixture
    {
        [Test]
        public void CanMap()
        {
            new PersistenceSpecification<TransferTransaction>(Session)
                .CheckProperty<TransferTransaction>(x => x.Id, Guid.NewGuid())
                .CheckProperty<TransferTransaction>(x => x.Date, DateTime.Today)
                .CheckProperty<TransferTransaction>(x => x.Comments, "some comment")
                .CheckProperty<TransferTransaction>(x => x.Deleted, true)
                .VerifyTheMappings();
        }
    }
}
