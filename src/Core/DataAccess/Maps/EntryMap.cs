using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.DataAccess.Maps
{
    public class EntryMap : ClassMap<Entry>
    {
        public EntryMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Type);
            Map(x => x.Amount);
            References(x => x.Account).Not.LazyLoad();
            References(x => x.Transaction);
            Map(x => x.Deleted);
        }
    }
}
