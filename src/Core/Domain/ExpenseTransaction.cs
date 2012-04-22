using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budgomatic.Core.Domain
{
    public interface IConfigureExpenseTransaction
    {
        IConfigureExpenseTransaction ForDate(DateTime date);
        IConfigureExpenseTransaction WithComments(string comments);
        IConfigureExpenseTransaction ExpenseAccount(Account expenseAccount);
        IConfigureExpenseTransaction AccountToCredit(Account accountToCredit);
    }

    public class ExpenseTransaction : Transaction, IConfigureExpenseTransaction
    {
        private decimal _amount;

        public override TransactionType Type
        {
            get { return TransactionType.Expense; }
        }

        public virtual IConfigureExpenseTransaction Configure(decimal amount)
        {
            _amount = amount;
            Entries.Clear();
            return this;
        }

        IConfigureExpenseTransaction IConfigureExpenseTransaction.ForDate(DateTime date)
        {
            Date = date;
            return this;
        }

        IConfigureExpenseTransaction IConfigureExpenseTransaction.WithComments(string comments)
        {
            Comments = comments;
            return this;
        }

        IConfigureExpenseTransaction IConfigureExpenseTransaction.ExpenseAccount(Account expenseAccount)
        {
            AddEntry(new Entry { Account = expenseAccount, Type = EntryType.Debit, Amount = _amount });
            return this;
        }

        IConfigureExpenseTransaction IConfigureExpenseTransaction.AccountToCredit(Account accountToCredit)
        {
            AddEntry(new Entry { Account = accountToCredit, Type = EntryType.Credit, Amount = _amount });
            return this;
        }

        public override string Description
        {
            get
            {
                return string.Format("{0} {1} from {2}",
                    Entries.Where(x => x.Type == EntryType.Credit).Sum(x => x.Amount),
                    Entries.First(x => x.Type == EntryType.Debit).Account.Name,
                    Entries.First(x => x.Type == EntryType.Credit).Account.Name);
            }
        }
    }
}
