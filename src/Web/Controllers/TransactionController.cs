using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Budgomatic.Core.Domain;
using Budgomatic.Core.DataAccess;

namespace Budgomatic.Web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IRepository _repository;

        public TransactionController(IRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /Transaction/

        public ActionResult Index()
        {
            return View("Index", _repository.Get<Transaction>());
        }        

        //
        // GET: /Transaction/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            var transaction = _repository.Find<Transaction>(id);
            _repository.Delete(transaction);

            return RedirectToAction("Index");
        }

        //
        // GET: /Transaction/Edit/5

        public ActionResult Edit(Guid id, TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Income:
                    return RedirectToAction("Edit", "IncomeTransaction", new { id = id });
                case TransactionType.Expense:
                    return RedirectToAction("Edit", "ExpenseTransaction", new { id = id });
                case TransactionType.Transfer:
                    return RedirectToAction("Edit", "TransferTransaction", new { id = id });
            }

            throw new Exception("Unhandled transaction type for edit: " + type);
        }
    }
}
