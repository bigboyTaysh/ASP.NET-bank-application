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
    public class CreditTypesController : Controller
    {
        private BankContext db = new BankContext();

        // GET: CreditTypes
        public ActionResult Index()
        {
            return View(db.CreditTypes.ToList());
        }

        // GET: CreditTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditType creditType = db.CreditTypes.Find(id);
            if (creditType == null)
            {
                return HttpNotFound();
            }
            return View(creditType);
        }

        // GET: CreditTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreditTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Rates,Commission")] CreditType creditType)
        {
            if (ModelState.IsValid)
            {
                db.CreditTypes.Add(creditType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(creditType);
        }

        // GET: CreditTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditType creditType = db.CreditTypes.Find(id);
            if (creditType == null)
            {
                return HttpNotFound();
            }
            return View(creditType);
        }

        // POST: CreditTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Rates,Commission")] CreditType creditType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(creditType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(creditType);
        }

        // GET: CreditTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditType creditType = db.CreditTypes.Find(id);
            if (creditType == null)
            {
                return HttpNotFound();
            }
            return View(creditType);
        }

        // POST: CreditTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CreditType creditType = db.CreditTypes.Find(id);
            db.CreditTypes.Remove(creditType);
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
