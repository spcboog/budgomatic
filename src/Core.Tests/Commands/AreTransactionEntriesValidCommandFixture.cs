using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Core.Commands;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.Tests.Commands
{
    [TestFixture]
    public class AreTransactionEntriesValidCommandFixture
    {
        [Test]
        public void Execute_ForNoEntries_ShouldReturnFalse()
        {
            var command = new AreTransactionEntriesValidCommand();
            var result = command.Execute(new Entry[0]);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_ForDebitNotEqualToCredit_ShouldReturnFalse()
        {
            var command = new AreTransactionEntriesValidCommand();
            var result = command.Execute(new[] {
                new Entry { Amount = 100, Type = EntryType.Debit },
                new Entry { Amount = 200, Type = EntryType.Credit }
                });

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_ForDebitEqualToCredit_ShouldReturnTrue()
        {
            var command = new AreTransactionEntriesValidCommand();
            var result = command.Execute(new[] {
                new Entry { Amount = 100, Type = EntryType.Debit },
                new Entry { Amount = 100, Type = EntryType.Credit }
                });

            Assert.That(result, Is.True);
        }
    }
}
