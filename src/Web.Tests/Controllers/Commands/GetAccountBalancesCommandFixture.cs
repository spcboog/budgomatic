using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Web.Controllers.Commands;
using Rhino.Mocks;
using Budgomatic.Core.DataAccess;
using Budgomatic.Core.Domain;

namespace Budgomatic.Web.Tests.Controllers.Commands
{
    [TestFixture]
    public class GetAccountBalancesCommandFixture
    {
        [Test]
        public void Execute_WillGetBalanceForOnlyAssetAndLiabilityAccounts()
        {
            var repository = MockRepository.GenerateMock<IRepository>();
            var getAccountBalanceForDateCommand = MockRepository.GenerateMock<IGetAccountBalanceForDateCommand>();
            var date = DateTime.Today.AddDays(1);
            const decimal assetAccountBalance = 100;
            const decimal liabilityAccountBalance = 20;

            var assetAccount = new AssetAccount { Name = "some asset account" };
            var liabilityAccount = new LiabilityAccount { Name = "some liability account" };
            var expenseAccount = new ExpenseAccount { Name = "some expense account" };
            var incomeAccount = new IncomeAccount { Name = "some income account" };

            var accounts = new List<Account> { assetAccount, liabilityAccount, expenseAccount, incomeAccount };

            repository.Stub(x => x.Get<Account>()).Return(accounts);
            getAccountBalanceForDateCommand.Stub(x => x.Execute(assetAccount.Id, date)).Return(assetAccountBalance);
            getAccountBalanceForDateCommand.Stub(x => x.Execute(liabilityAccount.Id, date)).Return(liabilityAccountBalance);

            var command = new GetAccountBalancesCommand(repository, getAccountBalanceForDateCommand);
            var result = command.Execute(date);

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(x => (x.AccountName == assetAccount.Name) && (x.Balance == assetAccountBalance)), Is.True);
            Assert.That(result.Any(x => (x.AccountName == liabilityAccount.Name) && (x.Balance == liabilityAccountBalance)), Is.True);
        }
    }
}
