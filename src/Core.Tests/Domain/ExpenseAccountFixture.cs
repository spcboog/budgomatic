using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.Tests.Domain
{
    [TestFixture]
    public class ExpenseAccountFixture
    {
        [Test]
        public void AccountType_ShouldReturnExpense()
        {
            var account = new ExpenseAccount();

            Assert.That(account.AccountType, Is.EqualTo(AccountType.Expense));
        }
    }
}
