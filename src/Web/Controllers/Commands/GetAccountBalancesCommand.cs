using System;
using System.Collections.Generic;
using System.Linq;
using Budgomatic.Core.DataAccess;
using Budgomatic.Core.Domain;

namespace Budgomatic.Web.Controllers.Commands
{
    public interface IGetAccountBalancesCommand
    {
        IEnumerable<Models.Home.AccountBalance> Execute(DateTime date);
    }

    public class GetAccountBalancesCommand : IGetAccountBalancesCommand
    {
        private readonly IRepository _repository;
        private readonly IGetAccountBalanceForDateCommand _getAccountBalanceForDateCommand;

        public GetAccountBalancesCommand(
            IRepository repository,
            IGetAccountBalanceForDateCommand getAccountBalanceForDateCommand)
        {
            _repository = repository;
            _getAccountBalanceForDateCommand = getAccountBalanceForDateCommand;
        }

        public IEnumerable<Models.Home.AccountBalance> Execute(DateTime date)
        {
            var accountBalances = new List<Models.Home.AccountBalance>();

            foreach (var account in _repository.Get<Account>().Where(x => 
                (x.AccountType == AccountType.Asset) || (x.AccountType == AccountType.Liability)))
            {
                accountBalances.Add(new Models.Home.AccountBalance
                                    {
                                    AccountName = account.Name,
                                    Balance = _getAccountBalanceForDateCommand.Execute(account.Id, date)
                                    });
            }

            return accountBalances;
        }
    }
}
