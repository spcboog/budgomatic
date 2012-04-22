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
using Budgomatic.Web.Models.Account;

namespace Budgomatic.Web.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerFixture
    {
        private IRepository _repository;
        private IAccountFactory _accountFactory;

        [SetUp]
        public void SetUp()
        {
            _repository = MockRepository.GenerateMock<IRepository>();
            _accountFactory = MockRepository.GenerateMock<IAccountFactory>();
        }

        private AccountController GetController()
        {
            return new AccountController(_repository, _accountFactory);
        }

        [Test]
        public void Index_ShouldGetAccountsFromRepository()
        {
            _repository.Expect(x => x.Get<Account>()).Return(null);

            var controller = GetController();
            var result = controller.Index();

            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Index_ShouldReturnViewWithModel()
        {
            var accounts = new List<Account>();

            _repository.Stub(x => x.Get<Account>()).Return(accounts);

            var controller = GetController();
            var result = (ViewResult)controller.Index();

            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(((Models.Account.IndexModel)result.Model).Accounts, Is.EqualTo(accounts));
        }

        [Test]
        public void Create_ShouldReturnView()
        {
            var controller = GetController();
            var result = (ViewResult)controller.Create();

            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void Create_ShouldGetAccountFromFactoryAndSaveToRepository()
        {
            const string name = "some name";
            const AccountType type = AccountType.Asset;
            var account = new AssetAccount();

            _accountFactory.Expect(x => x.Create(type)).Return(account);
            _repository.Expect(x => x.Save(
                Arg<Account>.Matches(y => 
                    (y.Name == name) && 
                    (y.AccountType == type)
                    )));

            var controller = GetController();
            controller.Create(new CreateModel { Name = name, AccountType = type });

            _accountFactory.VerifyAllExpectations();
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Create_ShouldReturnRedirectToIndex()
        {
            const AccountType type = AccountType.Asset;
            var account = new AssetAccount();
            _accountFactory.Stub(x => x.Create(type)).Return(account);

            var controller = GetController();
            var result = (RedirectToRouteResult)controller.Create(new CreateModel { AccountType = type });

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void Edit_ShouldReturnViewWithModel()
        {
            const string accountName = "some account";
            var id = Guid.NewGuid();
            var account = new AssetAccount { Name = accountName };

            _repository.Stub(x => x.Find<Account>(id)).Return(account);

            var controller = GetController();
            var result = (ViewResult)controller.Edit(id);

            Assert.That(result.ViewName, Is.EqualTo("Edit"));
            Assert.That(((EditModel)result.Model).Id, Is.EqualTo(id));
            Assert.That(((EditModel)result.Model).Name, Is.EqualTo(accountName));
            Assert.That(((EditModel)result.Model).AccountType, Is.EqualTo(account.AccountType));
        }

        [Test]
        public void Edit_ShouldSaveAccountToRepository()
        {
            var id = Guid.NewGuid();
            const string newName = "some other name";
            var model = new EditModel { Id = id, Name = newName };
            var account = new IncomeAccount { Id = id };

            _repository.Stub(x => x.Find<Account>(id)).Return(account);
            _repository.Expect(x => x.Save(
                Arg<Account>.Matches(y => 
                    (y.Id == id) && (y.Name == newName))));

            var controller = GetController();
            controller.Edit(model);

            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Edit_ShouldReturnRedirectToIndex()
        {
            var id = Guid.NewGuid();
            var model = new EditModel { Id = id };

            _repository.Stub(x => x.Find<Account>(id)).Return(new AssetAccount());
            
            var controller = GetController();
            var result = (RedirectToRouteResult)controller.Edit(model);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void Delete_ShouldDeleteFromRepository()
        {
            var id = Guid.NewGuid();
            var account = new IncomeAccount();

            _repository.Stub(x => x.Find<Account>(id)).Return(account);
            _repository.Expect(x => x.Delete(account));

            var controller = GetController();
            controller.Delete(id);

            _repository.VerifyAllExpectations();
        }
    }
}
