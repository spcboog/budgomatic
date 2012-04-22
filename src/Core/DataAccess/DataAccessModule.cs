using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using NHibernate;

namespace Budgomatic.Core.DataAccess
{
    public class DataAccessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISessionFactory>().ToProvider(new SessionFactoryProvider()).InSingletonScope();
            Bind<IRepository>().To<Repository>();
            Bind<IGetAccountBalanceForDateCommand>().To<GetAccountBalanceForDateCommand>();
        }
    }
}
