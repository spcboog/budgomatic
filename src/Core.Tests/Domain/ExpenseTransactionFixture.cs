using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.Tests.Domain
{
    [TestFixture]
    public class ExpenseTransactionFixture
    {
        [Test]
        public void Configure_ShouldSetPropertiesAsExpected()
        {
            const decimal amount = 100;
            var date = DateTime.Today.AddDays(1);
            const string comments = "something";
            var expenseAccount = new ExpenseAccount();
            var assetAccount = new AssetAccount();

            var transaction = new ExpenseTransaction();
            transaction.Configure(amount)
                    .ForDate(date)
                    .WithComments(comments)
                    .ExpenseAccount(expenseAccount)
                    .AccountToCredit(assetAccount);

            Assert.That(transaction.Date, Is.EqualTo(date));
            Assert.That(transaction.Comments, Is.EqualTo(comments));
            Assert.That(transaction.Entries.Count, Is.EqualTo(2));
            Assert.That(transaction.Entries.Any(x => (x.Amount == amount) && (x.Type == EntryType.Debit) && (x.Account == expenseAccount)), Is.True);
            Assert.That(transaction.Entries.Any(x => (x.Amount == amount) && (x.Type == EntryType.Credit) && (x.Account == assetAccount)), Is.True);
        }
    }
}
