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
                transactions = db.Transactions
                    .Include(t => t.TransactionType)
                    .Include(t => t.Currency);
            } 
            else
            {
                user = db.Profiles.Single(p => p.Email == User.Identity.Name);
                bankAccounts = user.BankAccounts.Select(b => b.BankAccountNumber);
                transactions = db.Transactions
                    .Where(t => bankAccounts
                            .Any(b => b == t.FromBankAccountNumber || b == t.ToBankAccountNumber))
                    .Include(t => t.TransactionType)
                    .Include(t => t.Currency);
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
            ViewBag.CurrencyToID = new SelectList(db.Currencies, "ID", "Code");
            ViewBag.BankAccounts = db.Profiles.Single(p => p.Login == User.Identity.Name).BankAccounts;
            return View("Transfer");
        }

        // POST: Transactions/Transfer
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer([Bind(Include = "ReceiverName,ToBankAccountNumber,Description,ValueTo,Date,CurrencyToID")] Transaction transaction, int BankAccountID)
        {
            var bankAccounts = db.Profiles.Single(p => p.Login == User.Identity.Name).BankAccounts;
            var bankAccount = db.BankAccounts.SingleOrDefault(b => b.ID == BankAccountID);
            var toBankAccount = db.BankAccounts.SingleOrDefault(b => b.BankAccountNumber == transaction.ToBankAccountNumber);
            var currency = db.Currencies.Single(c => c.ID == transaction.CurrencyToID);

            if (!bankAccounts.Any(b => b.ID == BankAccountID))
            {
                ModelState.AddModelError("FromBankAccountNumber", "Nie znaleziono takiego konta bankowego");
            } else if (bankAccount != null) 
            {
                if (bankAccount.AvailableFounds < transaction.ValueTo)
                {
                    ModelState.AddModelError("ValueTo", "Za mało środków");
                }

                if (bankAccount.Currency.Code != currency.Code && bankAccount.BankAccountType.Type != "FOR_CUR_ACC")
                {
                    ModelState.AddModelError("CurrencyToID", "Podaj poprawną walutę");
                    
                }

                if (bankAccount.BankAccountNumber == transaction.ToBankAccountNumber)
                {
                    ModelState.AddModelError("ToBankAccountNumber", "Podaj prawidłowy numer konta");
                }
            }

            if (ModelState.IsValid)
            {
                var user = db.Profiles.Single(p => p.Email == User.Identity.Name);

                bankAccount.AvailableFounds -= transaction.ValueTo;
                transaction.BalanceAfterTransactionUserFrom = bankAccount.Balance;

                if (toBankAccount != null)
                {
                    toBankAccount.Balance += transaction.ValueTo;
                    toBankAccount.AvailableFounds += transaction.ValueTo;
                    transaction.BalanceAfterTransactionUserTo = toBankAccount.Balance;
                }

                transaction.FromBankAccountNumber = bankAccount.BankAccountNumber;
                transaction.SenderName = user.FullName;
                transaction.TransactionTypeID = db.TransactionTypes.Single(t => t.Type == "TRANSFER").ID;
                
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TransactionTypeID = new SelectList(db.TransactionTypes, "ID", "Type", transaction.TransactionTypeID);
            ViewBag.CurrencyToID = new SelectList(db.Currencies, "ID", "Code", transaction.CurrencyToID);
            ViewBag.BankAccounts = db.Profiles.Single(p => p.Login == User.Identity.Name).BankAccounts;

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
            ViewBag.CurrencyToID = new SelectList(db.Currencies, "ID", "Code", transaction.CurrencyToID);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ValueTo,BalanceAfterTransactionUserFrom,BalanceAfterTransactionUserTo,FromBankAccountNumber,ToBankAccountNumber,Date,TransactionTypeID")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TransactionTypeID = new SelectList(db.TransactionTypes, "ID", "Type", transaction.TransactionTypeID);
            ViewBag.CurrencyToID = new SelectList(db.Currencies, "ID", "Code", transaction.CurrencyToID);
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

        private decimal ExchangeCurrency(Currency from, Currency to, decimal value)
        {
            return value;
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
