using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Budgomatic.Core.Domain;
using Budgomatic.Core.DataAccess;

namespace Budgomatic.Web.Controllers
{
    public class IncomeTransactionController : Controller
    {
        private readonly IRepository _repository;

        public IncomeTransactionController(IRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /IncomeTransaction/Create

        public ActionResult Create()
        {
            var assetAccounts = _repository.Get<Account>().Where(x => x.AccountType == AccountType.Asset);
            var incomeAccounts = _repository.Get<Account>().Where(x => x.AccountType == AccountType.Income);

            var model = new Models.IncomeTransaction.CreateModel
            {
                AssetAccounts = assetAccounts,
                IncomeAccounts = incomeAccounts,
                SelectedAssetAccountId = assetAccounts.First().Id,
                SelectedIncomeAccountId = incomeAccounts.First().Id,
                Date = DateTime.Today
            };

            return View("Create", model);
        } 

        //
        // POST: /IncomeTransaction/Create

        [HttpPost]
        public ActionResult Create(Models.IncomeTransaction.CreateModel model)
        {
            var incomeTransaction = new IncomeTransaction();
            incomeTransaction.Configure(model.Amount)
                .ForDate(model.Date)
                .WithComments(model.Comments)
                .IncomeAccount(_repository.Find<Account>(model.SelectedIncomeAccountId))
                .AccountToDebit(_repository.Find<Account>(model.SelectedAssetAccountId));

            _repository.Save(incomeTransaction);

            return RedirectToAction("Index", "Transaction");
        }
        
        //
        // GET: /IncomeTransaction/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            var incomeTransaction = _repository.Find<IncomeTransaction>(id);
            var assetAccounts = _repository.Get<Account>().Where(x => x.AccountType == AccountType.Asset);
            var incomeAccounts = _repository.Get<Account>().Where(x => x.AccountType == AccountType.Income);

            var model = new Models.IncomeTransaction.EditModel
            {
                Id = id,
                AssetAccounts = assetAccounts,
                IncomeAccounts = incomeAccounts,
                SelectedIncomeAccountId = incomeTransaction.Entries.Single(x => x.Account is IncomeAccount).Account.Id,
                SelectedAssetAccountId = incomeTransaction.Entries.Single(x => x.Account is AssetAccount).Account.Id,
                Date = incomeTransaction.Date,
                Amount = incomeTransaction.Entries.Single(x => x.Account is IncomeAccount).Amount,
                Comments = incomeTransaction.Comments
            };

            return View("Edit", model);
        }

        //
        // POST: /IncomeTransaction/Edit/5

        [HttpPost]
        public ActionResult Edit(Web.Models.IncomeTransaction.EditModel model)
        {
            var incomeTransaction = _repository.Find<IncomeTransaction>(model.Id);
            incomeTransaction.Configure(model.Amount)
                .ForDate(model.Date)
                .WithComments(model.Comments)
                .IncomeAccount(_repository.Find<Account>(model.SelectedIncomeAccountId))
                .AccountToDebit(_repository.Find<Account>(model.SelectedAssetAccountId));

            _repository.Save(incomeTransaction);

            return RedirectToAction("Index", "Transaction");
        }
    }
}
