using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.DataAccess.Maps
{
    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Date);
            Map(x => x.Comments);
            HasMany(x => x.Entries)
                .Cascade.AllDeleteOrphan()
                .Inverse()
                .Not.LazyLoad();
            Map(x => x.Deleted);
        }
    }

    public class IncomeTransactionMap : SubclassMap<IncomeTransaction>
    {
        public IncomeTransactionMap()
        {

        }
    }

    public class ExpenseTransactionMap : SubclassMap<ExpenseTransaction>
    {
        public ExpenseTransactionMap()
        {

        }
    }

    public class TransferTransactionMap : SubclassMap<TransferTransaction>
    {
        public TransferTransactionMap()
        {

        }
    }
}
