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
using Budgomatic.Web.Models.TransferTransaction;

namespace Budgomatic.Web.Tests.Controllers
{
    [TestFixture]
    public class TransferTransactionControllerFixture
    {
        private IRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = MockRepository.GenerateMock<IRepository>();
        }

        private TransferTransactionController GetController()
        {
            return new TransferTransactionController(_repository);
        }

        [Test]
        public void Create_ShouldReturnViewWithModel()
        {
            var assetAccount1 = new AssetAccount();
            var assetAccount2 = new AssetAccount();
            var liabilityAccount1 = new LiabilityAccount();
            var liabilityAccount2 = new LiabilityAccount();

            var accounts = new Account[] { assetAccount1, assetAccount2, liabilityAccount1, liabilityAccount2 };
            _repository.Stub(x => x.Get<Account>()).Return(accounts);

            var controller = GetController();
            var result = (ViewResult)controller.Create();

            Assert.That(result.ViewName, Is.EqualTo("Create"));

            var viewModel = (CreateModel)result.Model;
            Assert.That(viewModel.AssetAndLiabilityAccounts.Count(), Is.EqualTo(4));
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == assetAccount1.Id), Is.True);
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == assetAccount2.Id), Is.True);
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == liabilityAccount1.Id), Is.True);
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == liabilityAccount2.Id), Is.True);
            Assert.That(viewModel.SelectedFromAccountId, Is.EqualTo(assetAccount1.Id));
            Assert.That(viewModel.SelectedToAccountId, Is.EqualTo(assetAccount2.Id));
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
            var assetAccount1 = new ExpenseAccount();
            var assetAccount2 = new AssetAccount();

            var model = new CreateModel();
            model.Amount = amount;
            model.Date = date;
            model.Comments = comments;
            model.SelectedFromAccountId = assetAccount1.Id;
            model.SelectedToAccountId = assetAccount2.Id;

            _repository.Stub(x => x.Find<Account>(assetAccount1.Id)).Return(assetAccount1);
            _repository.Stub(x => x.Find<Account>(assetAccount2.Id)).Return(assetAccount2);
            _repository.Expect(x => x.Save(Arg<TransferTransaction>.Matches(
                y =>
                    (y.Date == date) &&
                    (y.Comments == comments) &&
                    (y.Entries.Count == 2) &&
                    (y.Entries.Any(z => (z.Account.Id == assetAccount1.Id) && (z.Amount == amount) && (z.Type == EntryType.Credit))) &&
                    (y.Entries.Any(z => (z.Account.Id == assetAccount2.Id) && (z.Amount == amount) && (z.Type == EntryType.Debit)))))).Return(null);

            var controller = GetController();
            controller.Create(model);

            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Create_ShouldReturnRedirectToIndex()
        {
            var assetAccount1 = new ExpenseAccount();
            var assetAccount2 = new AssetAccount();

            var model = new CreateModel();
            model.SelectedFromAccountId = assetAccount1.Id;
            model.SelectedToAccountId = assetAccount2.Id;

            _repository.Stub(x => x.Find<Account>(assetAccount1.Id)).Return(assetAccount1);
            _repository.Stub(x => x.Find<Account>(assetAccount2.Id)).Return(assetAccount2);

            var controller = GetController();
            var result = (RedirectToRouteResult)controller.Create(model);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Transaction"));
        }

        [Test]
        public void Edit_ShouldReturnViewWithModel()
        {
            var assetAccount1 = new AssetAccount();
            var assetAccount2 = new AssetAccount();
            var liabilityAccount1 = new LiabilityAccount();
            var liabilityAccount2 = new LiabilityAccount();
            var accounts = new Account[] { assetAccount1, assetAccount2, liabilityAccount1, liabilityAccount2 };

            const string comments = "something";
            const decimal amount = 100;
            var date = DateTime.Today.AddDays(1);

            var TransferTransaction = new TransferTransaction();
            TransferTransaction.Configure(amount)
                .ForDate(date)
                .WithComments(comments)
                .FromAccount(assetAccount1)
                .ToAccount(assetAccount2);

            _repository.Stub(x => x.Find<TransferTransaction>(TransferTransaction.Id)).Return(TransferTransaction);
            _repository.Stub(x => x.Get<Account>()).Return(accounts);

            var controller = GetController();
            var result = (ViewResult)controller.Edit(TransferTransaction.Id);

            Assert.That(result.ViewName, Is.EqualTo("Edit"));

            var viewModel = (EditModel)result.Model;
            Assert.That(viewModel.Id, Is.EqualTo(TransferTransaction.Id));
            Assert.That(viewModel.AssetAndLiabilityAccounts.Count(), Is.EqualTo(4));
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == assetAccount1.Id), Is.True);
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == assetAccount2.Id), Is.True);
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == liabilityAccount1.Id), Is.True);
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == liabilityAccount2.Id), Is.True);
            Assert.That(viewModel.SelectedFromAccountId, Is.EqualTo(assetAccount1.Id));
            Assert.That(viewModel.SelectedToAccountId, Is.EqualTo(assetAccount2.Id));
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
            var assetAccount1 = new AssetAccount();
            var assetAccount2 = new AssetAccount();
            var TransferTransaction = new TransferTransaction();

            var model = new EditModel();
            model.Id = TransferTransaction.Id;
            model.Amount = amount;
            model.Date = date;
            model.Comments = comments;
            model.SelectedFromAccountId = assetAccount1.Id;
            model.SelectedToAccountId = assetAccount2.Id;

            _repository.Stub(x => x.Find<TransferTransaction>(TransferTransaction.Id)).Return(TransferTransaction);
            _repository.Stub(x => x.Find<Account>(assetAccount1.Id)).Return(assetAccount1);
            _repository.Stub(x => x.Find<Account>(assetAccount2.Id)).Return(assetAccount2);
            _repository.Expect(x => x.Save(Arg<TransferTransaction>.Matches(
                y =>
                    (y.Date == date) &&
                    (y.Comments == comments) &&
                    (y.Entries.Count == 2) &&
                    (y.Entries.Any(z => (z.Account.Id == assetAccount1.Id) && (z.Amount == amount) && (z.Type == EntryType.Credit))) &&
                    (y.Entries.Any(z => (z.Account.Id == assetAccount2.Id) && (z.Amount == amount) && (z.Type == EntryType.Debit)))))).Return(null);

            var controller = GetController();
            controller.Edit(model);

            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Edit_ShouldReturnRedirectToIndex()
        {
            var TransferTransaction = new TransferTransaction();
            var model = new EditModel();
            model.Id = TransferTransaction.Id;
            model.SelectedFromAccountId = Guid.NewGuid();
            model.SelectedToAccountId = Guid.NewGuid();

            _repository.Stub(x => x.Find<TransferTransaction>(TransferTransaction.Id)).Return(TransferTransaction);
            _repository.Stub(x => x.Find<Account>(model.SelectedFromAccountId)).Return(new AssetAccount());
            _repository.Stub(x => x.Find<Account>(model.SelectedToAccountId)).Return(new AssetAccount());

            var controller = GetController();
            var result = (RedirectToRouteResult)controller.Edit(model);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Transaction"));
        }
    }
}
