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
    public class GetAccountBalanceForDateCommandFixture : RepositoryFixture
    {
        private Entry GetEntry(DateTime date, decimal amount, Account account, EntryType entryType = EntryType.Debit)
        {
            var transaction = new IncomeTransaction { Date = date };
            var entry = new Entry { Amount = amount, Account = account, Transaction = transaction, Type = entryType };
            transaction.Entries.Add(entry);

            Repository.Save(transaction);

            return entry;
        }

        [Test]
        public void Execute_ShouldOnlyReturnAmountFromCurrentAndPastEntriesRelativeToSpecifiedDate()
        {
            var account = Repository.Save(new AssetAccount());
            var entryInThePast = GetEntry(DateTime.Today.AddDays(-7), 100, account);
            var entryFromToday = GetEntry(DateTime.Today, 1000, account);
            var entryInTheFuture = GetEntry(DateTime.Today.AddDays(7), 10000, account);

            var command = new GetAccountBalanceForDateCommand(Repository, SessionFactory);
            var result = command.Execute(account.Id, DateTime.Today);

            Assert.That(result, Is.EqualTo(entryInThePast.Amount + entryFromToday.Amount));
        }

        [Test]
        public void Execute_ShouldReturnAmountFromCurrentAndPastEntriesRelativeToSpecifiedDate()
        {
            var account = Repository.Save(new AssetAccount());
            var entryInThePast = GetEntry(DateTime.Today.AddDays(-7), 100, account);
            var entryFromToday = GetEntry(DateTime.Today, 1000, account);
            var entryInTheFuture = GetEntry(DateTime.Today.AddDays(7), 10000, account);

            var command = new GetAccountBalanceForDateCommand(Repository, SessionFactory);
            var result = command.Execute(account.Id, DateTime.Today.AddDays(7));

            Assert.That(result, Is.EqualTo(entryInThePast.Amount + entryFromToday.Amount + entryInTheFuture.Amount));
        }

        [Test]
        public void Execute_ForDebitIncreaseAccount_ShouldReturnSumOfDebitMinusCredit()
        {
            var account = Repository.Save(new AssetAccount());
            var debitEntry = GetEntry(DateTime.Today, 1000, account, EntryType.Debit);
            var creditEntry = GetEntry(DateTime.Today, 200, account, EntryType.Credit);

            var command = new GetAccountBalanceForDateCommand(Repository, SessionFactory);
            var result = command.Execute(account.Id, DateTime.Today);

            Assert.That(result, Is.EqualTo(debitEntry.Amount - creditEntry.Amount));
        }

        [Test]
        public void Execute_ForCreditIncreaseAccount_ShouldReturnSumOfCreditMinusDebit()
        {
            var account = Repository.Save(new LiabilityAccount());
            var debitEntry = GetEntry(DateTime.Today, 200, account, EntryType.Debit);
            var creditEntry = GetEntry(DateTime.Today, 1000, account, EntryType.Credit);

            var command = new GetAccountBalanceForDateCommand(Repository, SessionFactory);
            var result = command.Execute(account.Id, DateTime.Today);

            Assert.That(result, Is.EqualTo(creditEntry.Amount - debitEntry.Amount));
        }
    }
}
