using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budgomatic.Core.Domain
{
    public class ExpenseAccount : DebitIncreaseAccount
    {
        public override AccountType AccountType
        {
            get { return AccountType.Expense; }
        }
    }
}
