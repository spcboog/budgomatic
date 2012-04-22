using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate;

namespace Budgomatic.Core.Tests.DataAccess.Maps
{
    public class MapFixture : DataAccessFixture
    {
        protected ISession Session { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Session = SessionFactory.OpenSession();
        }

        [TearDown]
        public void TearDown()
        {
            if (Session != null)
            {
                Session.Dispose();
            }
        }
    }
}
