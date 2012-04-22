using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budgomatic.Core.DataAccess
{
    public interface IGetAccountBalanceForDateCommand
    {
        decimal Execute(Guid accountId, DateTime date);
    }
}
