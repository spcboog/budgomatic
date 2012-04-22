using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Budgomatic.Web.Controllers;
using System.Web.Mvc;
using Rhino.Mocks;
using Budgomatic.Core.DataAccess;
using Budgomatic.Core.Domain;
using Budgomatic.Web.Models.Home;
using Budgomatic.Web.Controllers.Commands;

namespace Budgomatic.Web.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerFixture
    {
        private IGetAccountBalancesCommand _getAccountBalancesCommand;

        [SetUp]
        public void SetUp()
        {
            _getAccountBalancesCommand = MockRepository.GenerateMock<IGetAccountBalancesCommand>();
        }

        private HomeController GetController()
        {
            return new HomeController(_getAccountBalancesCommand);
        }

        [Test]
        public void Index_WithNoDateSpecified_ShouldReturnViewWithBalanceDateOfToday()
        {
            var accountBalances = new List<AccountBalance>();
            _getAccountBalancesCommand.Expect(x => x.Execute(DateTime.Today)).Return(accountBalances);

            var controller = GetController();
            var result = (ViewResult)controller.Index();

            var viewModel = (IndexModel)result.Model;
            Assert.That(viewModel.Date, Is.EqualTo(DateTime.Today));
            Assert.That(viewModel.AccountBalances, Is.SameAs(accountBalances));
        }

        [Test]
        public void Index_WithModel_ShouldReturnViewWithSpecifiedBalanceDate()
        {
            var balanceDate = DateTime.Today.AddDays(1);
            var accountBalances = new List<AccountBalance>();
            _getAccountBalancesCommand.Expect(x => x.Execute(balanceDate)).Return(accountBalances);

            var model = new IndexModel() { Date = balanceDate };

            var controller = GetController();
            var result = (ViewResult)controller.Index(model);

            var viewModel = (IndexModel)result.Model;
            Assert.That(viewModel.Date, Is.EqualTo(balanceDate));
            Assert.That(viewModel.AccountBalances, Is.SameAs(accountBalances));
        }

        [Test]
        public void About_ShouldReturnView()
        {
            var controller = GetController();
            var result = (ViewResult)controller.About();

            Assert.That(result.ViewName, Is.EqualTo("About"));
        }
    }
}

