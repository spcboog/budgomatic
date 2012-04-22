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
using Budgomatic.Web.Models.Transaction;

namespace Budgomatic.Web.Tests.Controllers
{
    [TestFixture]
    public class TransactionControllerFixture
    {
        private IRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = MockRepository.GenerateMock<IRepository>();
        }

        private TransactionController GetController()
        {
            return new TransactionController(_repository);
        }

        [Test]
        public void Index_ShouldReturnViewWithModel()
        {
            var transactions = new List<Transaction>();

            _repository.Stub(x => x.Get<Transaction>()).Return(transactions);

            var controller = GetController();
            var result = (ViewResult)controller.Index();

            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(result.Model, Is.SameAs(transactions));            
        }

        [Test]
        public void Delete_ShouldDeleteTransactionFromRepository()
        {
            var id = Guid.NewGuid();
            var transaction = new IncomeTransaction();

            _repository.Stub(x => x.Find<Transaction>(id)).Return(transaction);
            _repository.Expect(x => x.Delete(transaction));

            var controller = GetController();
            controller.Delete(id);

            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Delete_ShouldReturnRedirectToIndex()
        {
            var controller = GetController();
            var result = (RedirectToRouteResult)controller.Delete(Guid.NewGuid());

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [TestCase(TransactionType.Income, "IncomeTransaction")]
        [TestCase(TransactionType.Expense, "ExpenseTransaction")]
        [TestCase(TransactionType.Transfer, "TransferTransaction")]
        public void Edit_ShouldReturnRedirectToEditActionOnExpectedController(TransactionType transactionType, string expectedController)
        {
            var controller = GetController();
            var result = (RedirectToRouteResult)controller.Edit(Guid.NewGuid(), transactionType);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Edit"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo(expectedController));
        }
    }
}
