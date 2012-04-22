using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budgomatic.Core.Domain
{
    public interface IConfigureIncomeTransaction
    {
        IConfigureIncomeTransaction ForDate(DateTime date);
        IConfigureIncomeTransaction WithComments(string comments);
        IConfigureIncomeTransaction IncomeAccount(Account incomeAccount);
        IConfigureIncomeTransaction AccountToDebit(Account accountToDebit);
    }

    public class IncomeTransaction : Transaction, IConfigureIncomeTransaction
    {
        private decimal _amount;

        public override TransactionType Type
        {
            get { return TransactionType.Income; }
        }

        public virtual IConfigureIncomeTransaction Configure(decimal amount)
        {
            _amount = amount;
            Entries.Clear();
            return this;
        }

        IConfigureIncomeTransaction IConfigureIncomeTransaction.ForDate(DateTime date)
        {
            Date = date;
            return this;
        }

        IConfigureIncomeTransaction IConfigureIncomeTransaction.WithComments(string comments)
        {
            Comments = comments;
            return this;
        }

        IConfigureIncomeTransaction IConfigureIncomeTransaction.IncomeAccount(Account incomeAccount)
        {
            AddEntry(new Entry { Account = incomeAccount, Type = EntryType.Credit, Amount = _amount });
            return this;
        }

        IConfigureIncomeTransaction IConfigureIncomeTransaction.AccountToDebit(Account accountToDebit)
        {
            AddEntry(new Entry { Account = accountToDebit, Type = EntryType.Debit, Amount = _amount });
            return this;
        }

        public override string Description
        {
            get
            {
                return string.Format("{0} from {1} to {2}",
                    Entries.Where(x => x.Type == EntryType.Credit).Sum(x => x.Amount),
                    Entries.First(x => x.Type == EntryType.Credit).Account.Name,
                    Entries.First(x => x.Type == EntryType.Debit).Account.Name);
            }
        }
    }
}
