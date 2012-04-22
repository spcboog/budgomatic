using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budgomatic.Core.Domain
{
    public class AccountFactory : IAccountFactory
    {
        public Account Create(AccountType type)
        {
            Account account = null;

            switch (type)
            {
                case AccountType.Asset:
                    account = new AssetAccount();
                    break;
                case AccountType.Income:
                    account = new IncomeAccount();
                    break;
                case AccountType.Expense:
                    account = new ExpenseAccount();
                    break;
                case AccountType.Liability:
                    account = new LiabilityAccount();
                    break;
            }

            return account;
        }
    }
}
