using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Budgomatic.Core.Domain;
using Budgomatic.Core.DataAccess;
using Budgomatic.Web.Controllers.Commands;

namespace Budgomatic.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetAccountBalancesCommand _getAccountBalancesCommand;

        public HomeController(IGetAccountBalancesCommand getAccountBalancesCommand)
        {
            _getAccountBalancesCommand = getAccountBalancesCommand;
        }

        public PartialViewResult Balances(DateTime date)
        {
            return PartialView("_balanceResults", _getAccountBalancesCommand.Execute(date));
        }

        public ActionResult Index()
        {
            var model = new Models.Home.IndexModel();
            model.Date = DateTime.Today;

            return Index(model);
        }

        [HttpPost]
        public ActionResult Index(Models.Home.IndexModel model)
        {
            model.AccountBalances = _getAccountBalancesCommand.Execute(model.Date);

            return View(model);
        }

        public ActionResult About()
        {
            return View("About");
        }
    }
}
