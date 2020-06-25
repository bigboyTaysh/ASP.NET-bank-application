﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankApplication.DAL;
using BankApplication.Models;
using BankApplication.ViewModels;

namespace BankApplication.Controllers
{
    public class HomeController : Controller
    {
        private BankContext db = new BankContext();
        public ActionResult Index()
        {
            return View();
        }

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