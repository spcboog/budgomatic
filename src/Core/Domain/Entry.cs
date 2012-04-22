using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budgomatic.Core.Domain
{
    public class Entry : Entity
    {
        public virtual EntryType Type { get; set; }

        public virtual Account Account { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
