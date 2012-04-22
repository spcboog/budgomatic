using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using NHibernate;

namespace Budgomatic.Web.Controllers.Commands
{
    public class CommandModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IGetAccountBalancesCommand>().To<GetAccountBalancesCommand>();
        }
    }
}
