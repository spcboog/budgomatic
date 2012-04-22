using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.Tests.Domain
{
    [TestFixture]
    public class AccountFixture
    {
        [Test]
        public void Name_ShouldSetAndGet()
        {
            var name = "some account name";
            var account = new AssetAccount();

            Assert.That(account.Name, Is.Null);

            account.Name = name;

            Assert.That(account.Name, Is.EqualTo(name));
        }
    }
}
