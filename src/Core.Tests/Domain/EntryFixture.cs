using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.Tests.Domain
{
    [TestFixture]
    public class EntryFixture
    {
        [Test]
        public void Type_ShouldSetAndGet()
        {
            var type = EntryType.Credit;
            var entry = new Entry();

            Assert.That(entry.Type, Is.Not.EqualTo(type));

            entry.Type = type;

            Assert.That(entry.Type, Is.EqualTo(type));
        }

        [Test]
        public void Account_ShouldSetAndGet()
        {
            var account = new AssetAccount();
            var entry = new Entry();

            Assert.That(entry.Account, Is.Null);

            entry.Account = account;

            Assert.That(entry.Account, Is.EqualTo(account));
        }

        [Test]
        public void Amount_ShouldSetAndGet()
        {
            const decimal amount = 100;
            var entry = new Entry();

            Assert.That(entry.Amount, Is.EqualTo(0));

            entry.Amount = amount;

            Assert.That(entry.Amount, Is.EqualTo(amount));
        }

        [Test]
        public void Transaction_ShouldSetAndGet()
        {
            var transaction = new IncomeTransaction();
            var entry = new Entry();

            Assert.That(entry.Transaction, Is.Null);

            entry.Transaction = transaction;

            Assert.That(entry.Transaction, Is.EqualTo(transaction));
        }
    }
}
