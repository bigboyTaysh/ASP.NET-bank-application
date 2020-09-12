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
            return View(db.CreditApplications.ToList());
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
            return View();
        }

        // POST: CreditApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreditAmount,TotalRepayment,MonthRepayment,NumberOfMonths,DateOfSubmission,State")] CreditApplication creditApplication)
        {
            if (ModelState.IsValid)
            {
                db.CreditApplications.Add(creditApplication);
                db.Profiles.Single(p => p.Email == User.Identity.Name).CreditApplications.Add(creditApplication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
        public ActionResult Edit([Bind(Include = "ID,CreditAmount,TotalRepayment,MonthRepayment,NumberOfMonths,DateOfSubmission,State")] CreditApplication creditApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(creditApplication).State = EntityState.Modified;
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
