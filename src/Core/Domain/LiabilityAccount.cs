using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budgomatic.Core.Domain
{
    public class LiabilityAccount : CreditIncreaseAccount
    {
        public override AccountType AccountType
        {
            get { return AccountType.Liability; }
        }
    }
}
