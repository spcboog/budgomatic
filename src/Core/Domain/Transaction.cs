using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budgomatic.Core.Domain
{
    public abstract class Transaction : Entity
    {
        protected decimal Amount { get; private set; }

        public Transaction()
        {
            Entries = new List<Entry>();
            Date = DateTime.Today;
        }

        public virtual DateTime Date { get; set; }

        public abstract TransactionType Type { get; }

        public virtual string Comments { get; set; }

        public virtual string Description { get { return string.Empty; } }

        public virtual IList<Entry> Entries { get; set; }

        protected virtual void AddEntry(Entry entry)
        {
            entry.Transaction = this;
            Entries.Add(entry);
        }
    }
}
