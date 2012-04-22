using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Budgomatic.Core.DataAccess;
using NUnit.Framework;

namespace Budgomatic.Core.Tests.DataAccess
{
    public class RepositoryFixture : DataAccessFixture
    {
        protected IRepository Repository { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Repository = new Repository(SessionFactory);
        }
    }
}
