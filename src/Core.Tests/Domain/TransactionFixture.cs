using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.Tests.Domain
{
    [TestFixture]
    public class TransactionFixture
    {
        [Test]
        public void Date_ShouldSetAndGet()
        {
            var date = DateTime.Today.AddDays(1);
            var transaction = new IncomeTransaction();

            Assert.That(transaction.Date, Is.Not.EqualTo(date));

            transaction.Date = date;

            Assert.That(transaction.Date, Is.EqualTo(date));
        }

        [Test]
        public void Comments_ShouldSetAndGet()
        {
            const string comments = "something";
            var transaction = new IncomeTransaction();

            Assert.That(transaction.Comments, Is.Not.EqualTo(comments));

            transaction.Comments = comments;

            Assert.That(transaction.Comments, Is.EqualTo(comments));
        }

        [Test]
        public void Entries_ShouldByEmptyByDefault()
        {
            var transaction = new IncomeTransaction();

            Assert.That(transaction.Entries, Is.Empty);
        }
    }
}
