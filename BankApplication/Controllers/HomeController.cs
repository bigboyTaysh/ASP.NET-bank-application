using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankApplication.DAL;
using BankApplication.Models;
using BankApplication.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;

namespace BankApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly BankContext db = new BankContext();

        
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Worker"))
            {
                return RedirectToAction("Index", "BankAccounts");
            } else
            {

                return View(db.Profiles.Single(p => p.Login == User.Identity.Name).BankAccounts);
            }
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            IQueryable<CreationDateGroup> data = from bankAccount in db.BankAccounts
                                                 group bankAccount by bankAccount.CreationDate
                                                 into dateGroup
                                                 select new CreationDateGroup()
                                                 {
                                                     CreationDate = dateGroup.Key,
                                                     BankAccountsCount = dateGroup.Count()
                                                 };

            return View(data.ToList());
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}