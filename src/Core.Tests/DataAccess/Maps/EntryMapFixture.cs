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
    public class EntryMapFixture : MapFixture
    {
        [Test]
        public void CanMap()
        {
            new PersistenceSpecification<Entry>(Session)
                .CheckProperty<Entry>(x => x.Id, Guid.NewGuid())
                .CheckProperty<Entry>(x => x.Amount, 100m)
                .CheckProperty<Entry>(x => x.Type, EntryType.Debit)
                .CheckProperty<Entry>(x => x.Deleted, true)
                .VerifyTheMappings();
        }
    }
}
