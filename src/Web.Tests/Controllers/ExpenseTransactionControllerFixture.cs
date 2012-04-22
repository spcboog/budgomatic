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
using Budgomatic.Web.Models.ExpenseTransaction;

namespace Budgomatic.Web.Tests.Controllers
{
    [TestFixture]
    public class ExpenseTransactionControllerFixture
    {
        private IRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = MockRepository.GenerateMock<IRepository>();
        }

        private ExpenseTransactionController GetController()
        {
            return new ExpenseTransactionController(_repository);
        }

        [Test]
        public void Create_ShouldReturnViewWithModel()
        {
            var expenseAccount1 = new ExpenseAccount();
            var expenseAccount2 = new ExpenseAccount();
            var assetAccount = new AssetAccount();
            var liabilityAccount = new LiabilityAccount();

            var accounts = new Account[] { expenseAccount1, expenseAccount2, assetAccount, liabilityAccount };
            _repository.Stub(x => x.Get<Account>()).Repeat.Times(2).Return(accounts);

            var controller = GetController();
            var result = (ViewResult)controller.Create();

            Assert.That(result.ViewName, Is.EqualTo("Create"));

            var viewModel = (CreateModel)result.Model;
            Assert.That(viewModel.ExpenseAccounts.Count(), Is.EqualTo(2));
            Assert.That(viewModel.ExpenseAccounts.Any(x => x.Id == expenseAccount1.Id), Is.True);
            Assert.That(viewModel.ExpenseAccounts.Any(x => x.Id == expenseAccount2.Id), Is.True);
            Assert.That(viewModel.SelectedExpenseAccountId, Is.EqualTo(expenseAccount1.Id));
            Assert.That(viewModel.AssetAndLiabilityAccounts.Count(), Is.EqualTo(2));
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == assetAccount.Id), Is.True);
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == liabilityAccount.Id), Is.True);
            Assert.That(viewModel.SelectedAssetOrLiabilityAccountId, Is.EqualTo(assetAccount.Id));
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
            var expenseAccount = new ExpenseAccount();
            var assetAccount = new AssetAccount();

            var model = new CreateModel();
            model.Amount = amount;
            model.Date = date;
            model.Comments = comments;
            model.SelectedExpenseAccountId = expenseAccount.Id;
            model.SelectedAssetOrLiabilityAccountId = assetAccount.Id;

            _repository.Stub(x => x.Find<Account>(expenseAccount.Id)).Return(expenseAccount);
            _repository.Stub(x => x.Find<Account>(assetAccount.Id)).Return(assetAccount);
            _repository.Expect(x => x.Save(Arg<ExpenseTransaction>.Matches(
                y => 
                    (y.Date == date) &&
                    (y.Comments == comments) &&
                    (y.Entries.Count == 2) &&
                    (y.Entries.Any(z => (z.Account.Id == expenseAccount.Id) && (z.Amount == amount) && (z.Type == EntryType.Debit))) &&
                    (y.Entries.Any(z => (z.Account.Id == assetAccount.Id) && (z.Amount == amount) && (z.Type == EntryType.Credit)))))).Return(null);

            var controller = GetController();
            controller.Create(model);

            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Create_ShouldReturnRedirectToIndex()
        {
            var expenseAccount = new ExpenseAccount();
            var assetAccount = new AssetAccount();

            var model = new CreateModel();
            model.SelectedExpenseAccountId = expenseAccount.Id;
            model.SelectedAssetOrLiabilityAccountId = assetAccount.Id;

            _repository.Stub(x => x.Find<Account>(expenseAccount.Id)).Return(expenseAccount);
            _repository.Stub(x => x.Find<Account>(assetAccount.Id)).Return(assetAccount);

            var controller = GetController();
            var result = (RedirectToRouteResult)controller.Create(model);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Transaction"));
        }

        [Test]
        public void Edit_ShouldReturnViewWithModel()
        {
            var expenseAccount1 = new ExpenseAccount();
            var expenseAccount2 = new ExpenseAccount();
            var assetAccount = new AssetAccount();
            var liabilityAccount = new LiabilityAccount();
            var accounts = new Account[] { expenseAccount1, expenseAccount2, assetAccount, liabilityAccount };

            const string comments = "something";
            const decimal amount = 100;
            var date = DateTime.Today.AddDays(1);

            var expenseTransaction = new ExpenseTransaction();
            expenseTransaction.Configure(amount)
                .ForDate(date)
                .WithComments(comments)
                .ExpenseAccount(expenseAccount2)
                .AccountToCredit(liabilityAccount);
            
            _repository.Stub(x => x.Find<ExpenseTransaction>(expenseTransaction.Id)).Return(expenseTransaction);
            _repository.Stub(x => x.Get<Account>()).Repeat.Times(2).Return(accounts);

            var controller = GetController();
            var result = (ViewResult)controller.Edit(expenseTransaction.Id);

            Assert.That(result.ViewName, Is.EqualTo("Edit"));

            var viewModel = (EditModel)result.Model;
            Assert.That(viewModel.Id, Is.EqualTo(expenseTransaction.Id));
            Assert.That(viewModel.ExpenseAccounts.Count(), Is.EqualTo(2));
            Assert.That(viewModel.ExpenseAccounts.Any(x => x.Id == expenseAccount1.Id), Is.True);
            Assert.That(viewModel.ExpenseAccounts.Any(x => x.Id == expenseAccount2.Id), Is.True);
            Assert.That(viewModel.SelectedExpenseAccountId, Is.EqualTo(expenseAccount2.Id));
            Assert.That(viewModel.AssetAndLiabilityAccounts.Count(), Is.EqualTo(2));
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == assetAccount.Id), Is.True);
            Assert.That(viewModel.AssetAndLiabilityAccounts.Any(x => x.Id == liabilityAccount.Id), Is.True);
            Assert.That(viewModel.SelectedAssetOrLiabilityAccountId, Is.EqualTo(liabilityAccount.Id));
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
            var expenseAccount = new ExpenseAccount();
            var assetAccount = new AssetAccount();
            var expenseTransaction = new ExpenseTransaction();

            var model = new EditModel();
            model.Id = expenseTransaction.Id;
            model.Amount = amount;
            model.Date = date;
            model.Comments = comments;
            model.SelectedExpenseAccountId = expenseAccount.Id;
            model.SelectedAssetOrLiabilityAccountId = assetAccount.Id;

            _repository.Stub(x => x.Find<ExpenseTransaction>(expenseTransaction.Id)).Return(expenseTransaction);
            _repository.Stub(x => x.Find<Account>(expenseAccount.Id)).Return(expenseAccount);
            _repository.Stub(x => x.Find<Account>(assetAccount.Id)).Return(assetAccount);
            _repository.Expect(x => x.Save(Arg<ExpenseTransaction>.Matches(
                y =>
                    (y.Date == date) &&
                    (y.Comments == comments) &&
                    (y.Entries.Count == 2) &&
                    (y.Entries.Any(z => (z.Account.Id == expenseAccount.Id) && (z.Amount == amount) && (z.Type == EntryType.Debit))) &&
                    (y.Entries.Any(z => (z.Account.Id == assetAccount.Id) && (z.Amount == amount) && (z.Type == EntryType.Credit)))))).Return(null);
           
            var controller = GetController();
            controller.Edit(model);

            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Edit_ShouldReturnRedirectToIndex()
        {
            var expenseTransaction = new ExpenseTransaction();
            var model = new EditModel();
            model.Id = expenseTransaction.Id;
            model.SelectedExpenseAccountId = Guid.NewGuid();
            model.SelectedAssetOrLiabilityAccountId = Guid.NewGuid();

            _repository.Stub(x => x.Find<ExpenseTransaction>(expenseTransaction.Id)).Return(expenseTransaction);
            _repository.Stub(x => x.Find<Account>(model.SelectedExpenseAccountId)).Return(new ExpenseAccount());
            _repository.Stub(x => x.Find<Account>(model.SelectedAssetOrLiabilityAccountId)).Return(new AssetAccount());

            var controller = GetController();
            var result = (RedirectToRouteResult)controller.Edit(model);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Transaction"));
        }
    }
}
