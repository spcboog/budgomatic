using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Budgomatic.Core.DataAccess;
using Budgomatic.Core.Domain;

namespace Budgomatic.Web
{
    public static class DatabaseHelper
    {
        public static void PopulateDatabase(IKernel kernel)
        {
            var accountRepository = kernel.Get<IRepository>();

            accountRepository.Save(new AssetAccount { Name = "Savings" });
            accountRepository.Save(new AssetAccount { Name = "Offset" });
            accountRepository.Save(new IncomeAccount { Name = "Salary" });
            accountRepository.Save(new IncomeAccount { Name = "Dividends" });
            accountRepository.Save(new LiabilityAccount { Name = "CreditCard" });
            accountRepository.Save(new ExpenseAccount { Name = "Petrol" });
            accountRepository.Save(new ExpenseAccount { Name = "Groceries" });
        }
    }
}