using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Budgomatic.Core.Domain;
using Budgomatic.Core.DataAccess;

namespace Budgomatic.Web.Controllers
{
    public class ExpenseTransactionController : Controller
    {
        private readonly IRepository _repository;

        public ExpenseTransactionController(IRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /ExpenseTransaction/Create

        public ActionResult Create()
        {
            var expenseAccounts = _repository.Get<Account>().Where(x => x.AccountType == AccountType.Expense);
            var assetAndLiabilityAccounts = _repository.Get<Account>().Where(x => (x.AccountType == AccountType.Asset) || (x.AccountType == AccountType.Liability));

            var model = new Models.ExpenseTransaction.CreateModel
            {
                ExpenseAccounts = expenseAccounts,
                AssetAndLiabilityAccounts = assetAndLiabilityAccounts,
                SelectedExpenseAccountId = expenseAccounts.First().Id,
                SelectedAssetOrLiabilityAccountId = assetAndLiabilityAccounts.First().Id,
                Date = DateTime.Today
            };

            return View("Create", model);
        } 

        //
        // POST: /ExpenseTransaction/Create

        [HttpPost]
        public ActionResult Create(Models.ExpenseTransaction.CreateModel model)
        {
            var expenseTransaction = new ExpenseTransaction();
            expenseTransaction.Configure(model.Amount)
                .ForDate(model.Date)
                .WithComments(model.Comments)
                .ExpenseAccount(_repository.Find<Account>(model.SelectedExpenseAccountId))
                .AccountToCredit(_repository.Find<Account>(model.SelectedAssetOrLiabilityAccountId));

            _repository.Save(expenseTransaction);

            return RedirectToAction("Index", "Transaction");
        }
        
        //
        // GET: /ExpenseTransaction/Edit/5

        public ActionResult Edit(Guid id)
        {
            var expenseTransaction = _repository.Find<ExpenseTransaction>(id);
            var expenseAccounts = _repository.Get<Account>().Where(x => x.AccountType == AccountType.Expense);
            var assetAndLiabilityAccounts = _repository.Get<Account>().Where(x => (x.AccountType == AccountType.Asset) || (x.AccountType == AccountType.Liability));

            var model = new Models.ExpenseTransaction.EditModel
            {
                Id = id,
                ExpenseAccounts = expenseAccounts,
                AssetAndLiabilityAccounts = assetAndLiabilityAccounts,
                SelectedExpenseAccountId = expenseTransaction.Entries.Single(x => x.Account is ExpenseAccount).Account.Id,
                SelectedAssetOrLiabilityAccountId = expenseTransaction.Entries.Single(x => (x.Account is AssetAccount) || (x.Account is LiabilityAccount)).Account.Id,
                Date = expenseTransaction.Date,
                Amount = expenseTransaction.Entries.Single(x => x.Account is ExpenseAccount).Amount,
                Comments = expenseTransaction.Comments
            };

            return View("Edit", model);
        }

        //
        // POST: /ExpenseTransaction/Edit/5

        [HttpPost]
        public ActionResult Edit(Web.Models.ExpenseTransaction.EditModel model)
        {
            var expenseTransaction = _repository.Find<ExpenseTransaction>(model.Id);
            expenseTransaction.Configure(model.Amount)
                .ForDate(model.Date)
                .WithComments(model.Comments)
                .ExpenseAccount(_repository.Find<Account>(model.SelectedExpenseAccountId))
                .AccountToCredit(_repository.Find<Account>(model.SelectedAssetOrLiabilityAccountId));

            _repository.Save(expenseTransaction);

            return RedirectToAction("Index", "Transaction");
        }
    }
}
