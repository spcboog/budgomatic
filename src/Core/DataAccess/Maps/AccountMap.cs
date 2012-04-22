using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.DataAccess.Maps
{
    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name);
            HasMany(x => x.Entries)
                .Cascade.All()
                .Inverse()
                .Not.LazyLoad();
            Map(x => x.Deleted);
        }
    }

    public class IncomeAccountMap : SubclassMap<IncomeAccount>
    {
        public IncomeAccountMap()
        {

        }
    }

    public class AssetAccountMap : SubclassMap<AssetAccount>
    {
        public AssetAccountMap()
        {
            
        }
    }

    public class LiabilityAccountMap : SubclassMap<LiabilityAccount>
    {
        public LiabilityAccountMap()
        {
            
        }
    }

    public class ExpenseAccountMap : SubclassMap<ExpenseAccount>
    {
        public ExpenseAccountMap()
        {
            
        }
    }
}
