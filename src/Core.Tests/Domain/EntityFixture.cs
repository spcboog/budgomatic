using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Core.Domain;

namespace Budgomatic.Core.Tests.Domain
{
    [TestFixture]
    public class EntityFixture
    {
        private class TestEntity : Entity
        {

        }

        [Test]
        public void Id_ShouldBeSetByDefault()
        {
            var entity = new TestEntity();

            Assert.That(entity.Id, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void Id_ShouldSetAndGet()
        {
            var id = Guid.NewGuid();

            var entity = new TestEntity();

            Assert.That(entity.Id, Is.Not.EqualTo(id));

            entity.Id = id;

            Assert.That(entity.Id, Is.EqualTo(id));
        }

        [Test]
        public void Deleted_ShouldSetAndGet()
        {
            var entity = new TestEntity();

            Assert.That(entity.Deleted, Is.False);

            entity.Deleted = true;

            Assert.That(entity.Deleted, Is.True);
        }
    }
}
