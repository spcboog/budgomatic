using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Budgomatic.Core.DataAccess;
using Budgomatic.Core.Domain;

namespace Budgomatic.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository _repository;
        private readonly IAccountFactory _accountFactory;

        public AccountController(IRepository repository,
            IAccountFactory accountFactory)
        {
            _repository = repository;
            _accountFactory = accountFactory;
        }

        //
        // GET: /Account/

        public ActionResult Index()
        {
            var model = new Models.Account.IndexModel();
            model.Accounts = _repository.Get<Account>();

            return View("Index", model);
        }

        //
        // GET: /Account/Create

        public ActionResult Create()
        {
            return View("Create");
        } 

        //
        // POST: /Account/Create

        [HttpPost]
        public ActionResult Create(Models.Account.CreateModel model)
        {
            var account = _accountFactory.Create(model.AccountType);
            account.Name = model.Name;

            _repository.Save(account);

            return RedirectToAction("Index");
        }
        
        //
        // GET: /Account/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            var account = _repository.Find<Account>(id);

            var model = new Models.Account.EditModel();
            model.Id = id;
            model.Name = account.Name;
            model.AccountType = account.AccountType;

            return View("Edit", model);
        }

        //
        // POST: /Account/Edit/5

        [HttpPost]
        public ActionResult Edit(Models.Account.EditModel model)
        {
            var account = _repository.Find<Account>(model.Id);
            account.Name = model.Name;                
                
            _repository.Save(account);
 
            return RedirectToAction("Index");
        }

        //
        // GET: /Account/Delete/5

        public ActionResult Delete(Guid id)
        {
            var account = _repository.Find<Account>(id);
            _repository.Delete(account);

            return RedirectToAction("Index");
        }
    }
}
