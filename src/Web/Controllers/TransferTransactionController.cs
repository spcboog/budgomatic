using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Budgomatic.Core.DataAccess;
using Budgomatic.Core.Domain;

namespace Budgomatic.Web.Controllers
{
    public class TransferTransactionController : Controller
    {
        private readonly IRepository _repository;

        public TransferTransactionController(IRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /TransferTransaction/Create

        public ActionResult Create()
        {
            var assetAndLiabilityAccounts = _repository.Get<Account>().Where(x => (x.AccountType == AccountType.Asset) || (x.AccountType == AccountType.Liability));

            var model = new Models.TransferTransaction.CreateModel
            {
                AssetAndLiabilityAccounts = assetAndLiabilityAccounts,
                SelectedFromAccountId = assetAndLiabilityAccounts.ElementAt(0).Id,
                SelectedToAccountId = assetAndLiabilityAccounts.ElementAt(1).Id,
                Date = DateTime.Today
            };

            return View("Create", model);
        } 

        //
        // POST: /TransferTransaction/Create

        [HttpPost]
        public ActionResult Create(Models.TransferTransaction.CreateModel model)
        {
            var transferTransaction = new TransferTransaction();
            transferTransaction.Configure(model.Amount)
                .ForDate(model.Date)
                .WithComments(model.Comments)
                .FromAccount(_repository.Find<Account>(model.SelectedFromAccountId))
                .ToAccount(_repository.Find<Account>(model.SelectedToAccountId));

            _repository.Save(transferTransaction);

            return RedirectToAction("Index", "Transaction");
        }
        
        //
        // GET: /TransferTransaction/Edit/5

        public ActionResult Edit(Guid id)
        {
            var transferTransaction = _repository.Find<TransferTransaction>(id);
            var assetAndLiabilityAccounts = _repository.Get<Account>().Where(x => (x.AccountType == AccountType.Asset) || (x.AccountType == AccountType.Liability));

            var model = new Models.TransferTransaction.EditModel
            {
                Id = id,
                AssetAndLiabilityAccounts = assetAndLiabilityAccounts,
                SelectedFromAccountId = transferTransaction.Entries.Single(x => x.Type == EntryType.Credit).Account.Id,
                SelectedToAccountId = transferTransaction.Entries.Single(x => x.Type == EntryType.Debit).Account.Id,
                Date = transferTransaction.Date,
                Amount = transferTransaction.Entries.First().Amount,
                Comments = transferTransaction.Comments
            };

            return View("Edit", model);
        }

        //
        // POST: /TransferTransaction/Edit/5

        [HttpPost]
        public ActionResult Edit(Web.Models.TransferTransaction.EditModel model)
        {
            var transferTransaction = _repository.Find<TransferTransaction>(model.Id);
            transferTransaction.Configure(model.Amount)
                .ForDate(model.Date)
                .WithComments(model.Comments)
                .FromAccount(_repository.Find<Account>(model.SelectedFromAccountId))
                .ToAccount(_repository.Find<Account>(model.SelectedToAccountId));

            _repository.Save(transferTransaction);

            return RedirectToAction("Index", "Transaction");
        }
    }
}
