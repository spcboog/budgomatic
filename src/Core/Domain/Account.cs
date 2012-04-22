using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budgomatic.Core.Domain
{
    public abstract class Account : Entity
    {
        public abstract AccountType AccountType { get; }

        public virtual string Name { get; set; }

        public virtual IList<Entry> Entries { get; set; }

        public Account()
        {
            Entries = new List<Entry>();
        }
    }
}
