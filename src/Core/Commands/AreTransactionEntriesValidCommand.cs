using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.Commands
{
    public interface IAreTransactionEntriesValidCommand
    {
        bool Execute(IEnumerable<Entry> entries);
    }

    public class AreTransactionEntriesValidCommand
    {
        private decimal GetTotalEntryAmount(IEnumerable<Entry> entries, EntryType entryType)
        {
            return entries.Where(x => x.Type == entryType).Sum(x => x.Amount);
        }

        public bool Execute(IEnumerable<Entry> entries)
        {
            return (entries.Count() > 0) &&
                (GetTotalEntryAmount(entries, EntryType.Debit) == GetTotalEntryAmount(entries, EntryType.Credit));
        }
    }
}
