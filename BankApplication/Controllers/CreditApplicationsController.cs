using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankApplication.DAL;
using BankApplication.Models;

namespace BankApplication.Controllers
{
    public class CreditApplicationsController : Controller
    {
        private BankContext db = new BankContext();

        // GET: CreditApplications
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Worker"))
            {
                return View(db.CreditApplications
                    .OrderByDescending(c => c.DateOfSubmission)
                    .ThenByDescending(c => c.ID).ToList());
            }
            else
            {
                return View(db.Profiles.Single(p => p.Login == User.Identity.Name).CreditApplications
                    .OrderByDescending(c => c.DateOfSubmission)
                    .ThenByDescending(c => c.ID).ToList());
            }
        }

        // GET: CreditApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditApplication creditApplication = db.CreditApplications.Find(id);
            if (creditApplication == null)
            {
                return HttpNotFound();
            }
            return View(creditApplication);
        }

        // GET: CreditApplications/Create
        public ActionResult Create()
        {
            ViewBag.Types = db.CreditTypes.ToList();
            return View();
        }

        // POST: CreditApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreditAmount,NumberOfMonths,TypeID")] CreditApplication creditApplication)
        {
            if (ModelState.IsValid)
            {
                creditApplication.TotalRepayment = GetMonthRepayment(creditApplication.CreditAmount, creditApplication.NumberOfMonths, creditApplication.TypeID) * creditApplication.NumberOfMonths;
                creditApplication.MonthRepayment = GetMonthRepayment(creditApplication.CreditAmount, creditApplication.NumberOfMonths, creditApplication.TypeID);
                creditApplication.DateOfSubmission = DateTime.Now;
                creditApplication.State = null;
                db.CreditApplications.Add(creditApplication);
                db.Profiles.Single(p => p.Email == User.Identity.Name).CreditApplications.Add(creditApplication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Types = db.CreditTypes.ToList();
            return View(creditApplication);
        }

        // GET: CreditApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditApplication creditApplication = db.CreditApplications.Find(id);
            if (creditApplication == null)
            {
                return HttpNotFound();
            }
            return View(creditApplication);
        }

        // POST: CreditApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreditAmount,State")] CreditApplication creditApplication)
        {
            if (ModelState.IsValid)
            {
                var creditApplicationEdit = db.CreditApplications.Single(c => c.ID == creditApplication.ID);
                creditApplicationEdit.State = creditApplication.State;
                db.Entry(creditApplicationEdit).State = EntityState.Modified;

                if (creditApplicationEdit.State == true)
                {

                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(creditApplication);
        }

        // GET: CreditApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditApplication creditApplication = db.CreditApplications.Find(id);
            if (creditApplication == null)
            {
                return HttpNotFound();
            }
            return View(creditApplication);
        }

        // POST: CreditApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CreditApplication creditApplication = db.CreditApplications.Find(id);
            db.CreditApplications.Remove(creditApplication);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private decimal GetMonthRepayment(decimal value, int months, int typeID)
        {
            var sum = 0.0m;
            var numberOfInstallmentsPaidDuringTheYear = 12m;
            var type = db.CreditTypes.Single(t => t.ID == typeID);

            var creditAmount = value + (value * (0.01m * type.Commission));

            for (int i = 1; i <= months; i++)
            {
                sum = sum + (decimal)Math.Pow((double)(1m + ((0.01m * type.Rates) / numberOfInstallmentsPaidDuringTheYear)), 0 - i);
            }

            return creditAmount / sum;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
