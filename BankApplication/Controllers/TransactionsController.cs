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
using PagedList;

namespace BankApplication.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Transactions
        public ActionResult Index()
        {
            ViewBag.BankAccounts = db.Profiles.Single(p => p.Login == User.Identity.Name).BankAccounts;
            return View();
        }

        [HttpPost]
        public PartialViewResult Transactions(string bankAccountNumber, int? page, int? size)
        {
            Profile user;
            List<Transaction> transactions = new List<Transaction>();
            ViewBag.BankAccountNumber = bankAccountNumber;


            if (User.IsInRole("Admin"))
            {
                transactions = db.Transactions
                    .Include(t => t.TransactionType)
                    .Include(t => t.CurrencTo).ToList();
            }
            else
            {
                user = db.Profiles.Single(p => p.Email == User.Identity.Name);
                if (user.BankAccounts.Any(b => b.BankAccountNumber == bankAccountNumber)) 
                {
                    transactions = db.Transactions
                    .Where(t => bankAccountNumber == t.FromBankAccountNumber || bankAccountNumber == t.ToBankAccountNumber)
                    .Include(t => t.TransactionType)
                    .Include(t => t.CurrencTo)
                    .OrderByDescending(t => t.Date).ThenByDescending(t => t.ID).ToList();
                } 
            }


            int pageSize = (size ?? 10);
            int pageNumber = (page ?? 1);

            ViewBag.Count = transactions.Count;

            return PartialView("TransfersList", transactions.ToPagedList(pageNumber, pageSize));
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
            var currencyTo = db.Currencies.Single(c => c.ID == transaction.CurrencyToID);
            var user = db.Profiles.Single(p => p.Email == User.Identity.Name);
            var commision = bankAccount.BankAccountType.Commission / 100;
            decimal value; 

            if(bankAccount.Currency.Code == currencyTo.Code)
            {
                value = transaction.ValueTo + (transaction.ValueTo * commision);
            } 
            else
            {
                var exchangedCurrency = ExchangeCurrency(bankAccount.Currency, currencyTo, transaction.ValueTo);
                value = exchangedCurrency + ( exchangedCurrency * commision);
            }
            

            if (!bankAccounts.Any(b => b.ID == BankAccountID))
            {
                ModelState.AddModelError("FromBankAccountNumber", "Nie znaleziono takiego konta bankowego");
            } else if (bankAccount != null) 
            {
                if (bankAccount.AvailableFounds < value)
                {
                    ModelState.AddModelError("ValueTo", "Za mało środków");
                }

                if (bankAccount.Currency.Code != currencyTo.Code && bankAccount.BankAccountType.Type != "FOR_CUR_ACC")
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
                bankAccount.Balance -= value;
                bankAccount.AvailableFounds -= value;
                transaction.ValueFrom = value;
                transaction.BalanceAfterTransactionUserFrom = bankAccount.Balance;

                if (toBankAccount != null)
                {
                    toBankAccount.Balance += transaction.ValueTo;
                    toBankAccount.AvailableFounds += transaction.ValueTo;
                    transaction.BalanceAfterTransactionUserTo = toBankAccount.Balance;
                }

                transaction.FromBankAccountNumber = bankAccount.BankAccountNumber;
                transaction.SenderName = user.FullName;
                transaction.CurrencFrom = bankAccount.Currency;

                transaction.TransactionTypeID = db.TransactionTypes.Single(t => t.Type == "TRANSFER").ID;

                transaction.OperationDate = DateTime.Now;
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
            return to.Ask * value / from.Bid;
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
