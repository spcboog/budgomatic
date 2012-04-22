using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.Tests.Domain
{
    [TestFixture]
    public class AssetAccountFixture
    {
        [Test]
        public void AccountType_ShouldReturnAsset()
        {
            var account = new AssetAccount();

            Assert.That(account.AccountType, Is.EqualTo(AccountType.Asset));
        }
    }
}
