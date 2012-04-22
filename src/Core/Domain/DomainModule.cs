using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using NHibernate;

namespace Budgomatic.Core.Domain
{
    public class DomainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAccountFactory>().To<AccountFactory>();
        }
    }
}
