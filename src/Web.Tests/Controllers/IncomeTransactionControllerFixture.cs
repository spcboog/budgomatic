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
using Budgomatic.Web.Models.IncomeTransaction;

namespace Budgomatic.Web.Tests.Controllers
{
    [TestFixture]
    public class IncomeTransactionControllerFixture
    {
        private IRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = MockRepository.GenerateMock<IRepository>();
        }

        private IncomeTransactionController GetController()
        {
            return new IncomeTransactionController(_repository);
        }

        [Test]
        public void Create_ShouldReturnViewWithModel()
        {
            var incomeAccount1 = new IncomeAccount();
            var incomeAccount2 = new IncomeAccount();
            var assetAccount1 = new AssetAccount();
            var assetAccount2 = new AssetAccount();

            var accounts = new Account[] { incomeAccount1, incomeAccount2, assetAccount1, assetAccount2 };
            _repository.Stub(x => x.Get<Account>()).Repeat.Times(2).Return(accounts);

            var controller = GetController();
            var result = (ViewResult)controller.Create();

            Assert.That(result.ViewName, Is.EqualTo("Create"));

            var viewModel = (CreateModel)result.Model;
            Assert.That(viewModel.IncomeAccounts.Count(), Is.EqualTo(2));
            Assert.That(viewModel.IncomeAccounts.Any(x => x.Id == incomeAccount1.Id), Is.True);
            Assert.That(viewModel.IncomeAccounts.Any(x => x.Id == incomeAccount2.Id), Is.True);
            Assert.That(viewModel.SelectedIncomeAccountId, Is.EqualTo(incomeAccount1.Id));
            Assert.That(viewModel.AssetAccounts.Count(), Is.EqualTo(2));
            Assert.That(viewModel.AssetAccounts.Any(x => x.Id == assetAccount1.Id), Is.True);
            Assert.That(viewModel.AssetAccounts.Any(x => x.Id == assetAccount2.Id), Is.True);
            Assert.That(viewModel.SelectedAssetAccountId, Is.EqualTo(assetAccount1.Id));
            Assert.That(viewModel.Date, Is.EqualTo(DateTime.Today));
            Assert.That(viewModel.Amount, Is.EqualTo(0));
            Assert.That(viewModel.Comments, Is.Null);
        }

        [Test]
        public void Create_ShouldSaveTransactionToRepository()
        {
            const decimal amount = 100;
            var date = DateTime.Today.AddDays(1);
            const string comments = "something";
            var incomeAccount = new IncomeAccount();
            var assetAccount = new AssetAccount();

            var model = new CreateModel();
            model.Amount = amount;
            model.Date = date;
            model.Comments = comments;
            model.SelectedIncomeAccountId = incomeAccount.Id;
            model.SelectedAssetAccountId = assetAccount.Id;

            _repository.Stub(x => x.Find<Account>(incomeAccount.Id)).Return(incomeAccount);
            _repository.Stub(x => x.Find<Account>(assetAccount.Id)).Return(assetAccount);
            _repository.Expect(x => x.Save(Arg<IncomeTransaction>.Matches(
                y =>
                    (y.Date == date) &&
                    (y.Comments == comments) &&
                    (y.Entries.Count == 2) &&
                    (y.Entries.Any(z => (z.Account.Id == incomeAccount.Id) && (z.Amount == amount) && (z.Type == EntryType.Credit))) &&
                    (y.Entries.Any(z => (z.Account.Id == assetAccount.Id) && (z.Amount == amount) && (z.Type == EntryType.Debit)))))).Return(null);

            var controller = GetController();
            controller.Create(model);

            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Create_ShouldReturnRedirectToIndex()
        {
            var incomeAccount = new IncomeAccount();
            var assetAccount = new AssetAccount();

            var model = new CreateModel();
            model.SelectedIncomeAccountId = incomeAccount.Id;
            model.SelectedAssetAccountId = assetAccount.Id;

            _repository.Stub(x => x.Find<Account>(incomeAccount.Id)).Return(incomeAccount);
            _repository.Stub(x => x.Find<Account>(assetAccount.Id)).Return(assetAccount);

            var controller = GetController();
            var result = (RedirectToRouteResult)controller.Create(model);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Transaction"));
        }

        [Test]
        public void Edit_ShouldReturnViewWithModel()
        {
            var incomeAccount1 = new IncomeAccount();
            var incomeAccount2 = new IncomeAccount();
            var assetAccount1 = new AssetAccount();
            var assetAccount2 = new AssetAccount();
            var accounts = new Account[] { incomeAccount1, incomeAccount2, assetAccount1, assetAccount2 };

            const string comments = "something";
            const decimal amount = 100;
            var date = DateTime.Today.AddDays(1);

            var IncomeTransaction = new IncomeTransaction();
            IncomeTransaction.Configure(amount)
                .ForDate(date)
                .WithComments(comments)
                .IncomeAccount(incomeAccount2)
                .AccountToDebit(assetAccount2);

            _repository.Stub(x => x.Find<IncomeTransaction>(IncomeTransaction.Id)).Return(IncomeTransaction);
            _repository.Stub(x => x.Get<Account>()).Repeat.Times(2).Return(accounts);

            var controller = GetController();
            var result = (ViewResult)controller.Edit(IncomeTransaction.Id);

            Assert.That(result.ViewName, Is.EqualTo("Edit"));

            var viewModel = (EditModel)result.Model;
            Assert.That(viewModel.Id, Is.EqualTo(IncomeTransaction.Id));
            Assert.That(viewModel.IncomeAccounts.Count(), Is.EqualTo(2));
            Assert.That(viewModel.IncomeAccounts.Any(x => x.Id == incomeAccount1.Id), Is.True);
            Assert.That(viewModel.IncomeAccounts.Any(x => x.Id == incomeAccount2.Id), Is.True);
            Assert.That(viewModel.SelectedIncomeAccountId, Is.EqualTo(incomeAccount2.Id));
            Assert.That(viewModel.AssetAccounts.Count(), Is.EqualTo(2));
            Assert.That(viewModel.AssetAccounts.Any(x => x.Id == assetAccount1.Id), Is.True);
            Assert.That(viewModel.AssetAccounts.Any(x => x.Id == assetAccount2.Id), Is.True);
            Assert.That(viewModel.SelectedAssetAccountId, Is.EqualTo(assetAccount2.Id));
            Assert.That(viewModel.Date, Is.EqualTo(date));
            Assert.That(viewModel.Amount, Is.EqualTo(amount));
            Assert.That(viewModel.Comments, Is.EqualTo(comments));
        }

        [Test]
        public void Edit_ShouldSaveTransactionToRepository()
        {
            const decimal amount = 100;
            var date = DateTime.Today.AddDays(1);
            const string comments = "something";
            var incomeAccount = new IncomeAccount();
            var assetAccount = new AssetAccount();
            var IncomeTransaction = new IncomeTransaction();

            var model = new EditModel();
            model.Id = IncomeTransaction.Id;
            model.Amount = amount;
            model.Date = date;
            model.Comments = comments;
            model.SelectedIncomeAccountId = incomeAccount.Id;
            model.SelectedAssetAccountId = assetAccount.Id;

            _repository.Stub(x => x.Find<IncomeTransaction>(IncomeTransaction.Id)).Return(IncomeTransaction);
            _repository.Stub(x => x.Find<Account>(incomeAccount.Id)).Return(incomeAccount);
            _repository.Stub(x => x.Find<Account>(assetAccount.Id)).Return(assetAccount);
            _repository.Expect(x => x.Save(Arg<IncomeTransaction>.Matches(
                y =>
                    (y.Date == date) &&
                    (y.Comments == comments) &&
                    (y.Entries.Count == 2) &&
                    (y.Entries.Any(z => (z.Account.Id == incomeAccount.Id) && (z.Amount == amount) && (z.Type == EntryType.Credit))) &&
                    (y.Entries.Any(z => (z.Account.Id == assetAccount.Id) && (z.Amount == amount) && (z.Type == EntryType.Debit)))))).Return(null);

            var controller = GetController();
            controller.Edit(model);

            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Edit_ShouldReturnRedirectToIndex()
        {
            var IncomeTransaction = new IncomeTransaction();
            var model = new EditModel();
            model.Id = IncomeTransaction.Id;
            model.SelectedIncomeAccountId = Guid.NewGuid();
            model.SelectedAssetAccountId = Guid.NewGuid();

            _repository.Stub(x => x.Find<IncomeTransaction>(IncomeTransaction.Id)).Return(IncomeTransaction);
            _repository.Stub(x => x.Find<Account>(model.SelectedIncomeAccountId)).Return(new IncomeAccount());
            _repository.Stub(x => x.Find<Account>(model.SelectedAssetAccountId)).Return(new AssetAccount());

            var controller = GetController();
            var result = (RedirectToRouteResult)controller.Edit(model);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Transaction"));
        }
    }
}
