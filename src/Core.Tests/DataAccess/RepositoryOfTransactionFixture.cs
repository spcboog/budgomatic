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
    public class RepositoryOfTransactionFixture : RepositoryFixture
    {
        [Test]
        public void CanSaveAndGet()
        {
            var transaction = new IncomeTransaction();
            transaction.Date = DateTime.Today;
            transaction.Comments = "some comment";

            Repository.Save(transaction);

            var retrieved = Repository.Find<IncomeTransaction>(transaction.Id);

            Assert.That(retrieved, Is.Not.Null);
            Assert.That(retrieved.Id, Is.EqualTo(transaction.Id));
            Assert.That(retrieved.Date, Is.EqualTo(transaction.Date));
            Assert.That(retrieved.Comments, Is.EqualTo(transaction.Comments));
        }

        private Account SaveAccount(Account account)
        {
            Repository.Save(account);

            return Repository.Save(account);
        }

        [Test]
        public void CanSaveAndGetEntries()
        {
            var transaction = new IncomeTransaction();
            transaction.Entries.Add(new Entry { Account = SaveAccount(new IncomeAccount()), Transaction = transaction });
            transaction.Entries.Add(new Entry { Account = SaveAccount(new AssetAccount()), Transaction = transaction });

            Repository.Save(transaction);

            var retrieved = Repository.Find<IncomeTransaction>(transaction.Id);

            Assert.That(retrieved, Is.Not.Null);
            Assert.That(retrieved.Entries.Count, Is.EqualTo(2));
            Assert.That(retrieved.Entries.SingleOrDefault(x => x.Id == transaction.Entries[0].Id), Is.Not.Null);
            Assert.That(retrieved.Entries.SingleOrDefault(x => x.Id == transaction.Entries[1].Id), Is.Not.Null);
        }
    }
}
