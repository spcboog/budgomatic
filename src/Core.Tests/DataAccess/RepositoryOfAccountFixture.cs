using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Core.Domain;
using Budgomatic.Core.DataAccess;

namespace Budgomatic.Core.Tests.DataAccess
{
    [TestFixture]
    public class RepositoryOfAccountFixture : RepositoryFixture
    {
        [Test]
        public void CanSaveAndGet()
        {
            var account = new IncomeAccount();
            account.Name = "some name";

            Repository.Save(account);

            var retrieved = Repository.Find<IncomeAccount>(account.Id);

            Assert.That(retrieved, Is.Not.Null);
            Assert.That(retrieved.Id, Is.EqualTo(account.Id));
            Assert.That(retrieved.Name, Is.EqualTo(account.Name));
        }

        private Entry SaveEntry(Account account)
        {
            var entry = new Entry();
            entry.Account = account;

            return Repository.Save(entry);
        }

        [Test]
        public void CanSaveAndGetEntries()
        {
            var account = new IncomeAccount();
            account.Name = "some name";

            Repository.Save(account);

            var entry1 = SaveEntry(account);
            var entry2 = SaveEntry(account);

            var retrieved = Repository.Find<IncomeAccount>(account.Id);

            Assert.That(retrieved, Is.Not.Null);
            Assert.That(retrieved.Entries.Count, Is.EqualTo(2));
            Assert.That(retrieved.Entries.SingleOrDefault(x => x.Id == entry1.Id), Is.Not.Null);
            Assert.That(retrieved.Entries.SingleOrDefault(x => x.Id == entry2.Id), Is.Not.Null);
        }
    }
}
