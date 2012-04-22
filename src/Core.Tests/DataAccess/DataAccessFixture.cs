using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Core.DataAccess;
using NHibernate;
using Ninject;

namespace Budgomatic.Core.Tests.DataAccess
{
    public class DataAccessFixture
    {
        protected ISessionFactory SessionFactory { get; private set; }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var kernel = new StandardKernel();
            kernel.Load<DataAccessModule>();

            SessionFactory = kernel.Get<ISessionFactory>();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            SessionFactory.Close();
        }
    }
}
