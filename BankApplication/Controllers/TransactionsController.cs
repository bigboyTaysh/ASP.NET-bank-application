using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BankApplication.DAL;
using BankApplication.Models;
using Microsoft.AspNet.Identity;

namespace BankApplication.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Transactions
        public ActionResult Index()
        {
            Profile user;
            IEnumerable<string> bankAccounts;
            IQueryable<Transaction> transactions;

            if (User.IsInRole("Admin"))
            {
                transactions = db.Transactions.Include(t => t.TransactionType);
            } 
            else
            {
                user = db.Profiles.Single(p => p.Login == User.Identity.Name);
                bankAccounts = user.BankAccounts.Select(b => b.BankAccountNumber);
                transactions = db.Transactions
                    .Where(t => bankAccounts
                            .Any(b => b == t.FromBankAccountNumber || b == t.ToBankAccountNumber))
                    .Include(t => t.TransactionType);
            }

            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Transfer
        public ActionResult Transfer()
        {
            ViewBag.TransactionTypeID = new SelectList(db.TransactionTypes, "ID", "Type");
            return View("Transfer");
        }

        // POST: Transactions/Transfer
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer([Bind(Include = "ID,Value,BalanceAfterTransactionUserFrom,BalanceAfterTransactionUserTo,FromBankAccountNumber,ToBankAccountNumber,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.TransactionTypeID = db.TransactionTypes.Single(t => t.Type == "TRANSFER").ID;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TransactionTypeID = new SelectList(db.TransactionTypes, "ID", "Type", transaction.TransactionTypeID);
            return View("Transfer", transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.TransactionTypeID = new SelectList(db.TransactionTypes, "ID", "Type", transaction.TransactionTypeID);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Value,BalanceAfterTransactionUserFrom,BalanceAfterTransactionUserTo,FromBankAccountNumber,ToBankAccountNumber,Date,TransactionTypeID")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TransactionTypeID = new SelectList(db.TransactionTypes, "ID", "Type", transaction.TransactionTypeID);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
