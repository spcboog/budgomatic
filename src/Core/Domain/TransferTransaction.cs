using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budgomatic.Core.Domain
{
    public interface IConfigureTransferTransaction
    {
        IConfigureTransferTransaction ForDate(DateTime date);
        IConfigureTransferTransaction WithComments(string comments);
        IConfigureTransferTransaction FromAccount(Account fromAccount);
        IConfigureTransferTransaction ToAccount(Account toAccount);
    }

    public class TransferTransaction : Transaction, IConfigureTransferTransaction
    {
        private decimal _amount;

        public override TransactionType Type
        {
            get { return TransactionType.Transfer; }
        }

        public virtual IConfigureTransferTransaction Configure(decimal amount)
        {
            _amount = amount;
            Entries.Clear();
            return this;
        }

        IConfigureTransferTransaction IConfigureTransferTransaction.ForDate(DateTime date)
        {
            Date = date;
            return this;
        }

        IConfigureTransferTransaction IConfigureTransferTransaction.WithComments(string comments)
        {
            Comments = comments;
            return this;
        }

        IConfigureTransferTransaction IConfigureTransferTransaction.FromAccount(Account fromAccount)
        {            
            AddEntry(new Entry { Account = fromAccount, Type = EntryType.Credit, Amount = _amount });
            return this;
        }

        IConfigureTransferTransaction IConfigureTransferTransaction.ToAccount(Account toAccount)
        {
            AddEntry(new Entry { Account = toAccount, Type = EntryType.Debit, Amount = _amount });
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
