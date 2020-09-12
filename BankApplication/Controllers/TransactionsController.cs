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
        public ActionResult Index(string bankAccountNumber)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Worker"))
            {
                var bankAccount = db.BankAccounts.SingleOrDefault(b => b.BankAccountNumber == bankAccountNumber);
                if (bankAccount != null)
                {
                    return View(bankAccount);
                }
                else
                {
                    return View(bankAccount);
                }
            }
            else
            {
                var bankAccounts = db.Profiles.Single(p => p.Login == User.Identity.Name).BankAccounts;
                var bankAccount = db.BankAccounts.SingleOrDefault(b => b.BankAccountNumber == bankAccountNumber);

                if (bankAccount != null && bankAccounts.Any(b => b.BankAccountNumber == bankAccount.BankAccountNumber))
                {
                    return View(bankAccount);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
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
                    .Where(t => bankAccountNumber == t.FromBankAccountNumber || bankAccountNumber == t.ToBankAccountNumber)
                    .Include(t => t.TransactionType)
                    .Include(t => t.CurrencyTo)
                    .OrderByDescending(t => t.Date).ThenByDescending(t => t.ID).ToList();
            }
            else
            {
                user = db.Profiles.Single(p => p.Email == User.Identity.Name);
                if (user.BankAccounts.Any(b => b.BankAccountNumber == bankAccountNumber)) 
                {
                    transactions = db.Transactions
                    .Where(t => bankAccountNumber == t.FromBankAccountNumber || bankAccountNumber == t.ToBankAccountNumber)
                    .Include(t => t.TransactionType)
                    .Include(t => t.CurrencyTo)
                    .OrderByDescending(t => t.Date).ThenByDescending(t => t.ID).ToList();
                } 
            }


            int pageSize = (size ?? 10);
            int pageNumber = (page ?? 1);

            ViewBag.Count = transactions.Count;

            return PartialView("TransfersList", transactions.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public PartialViewResult IndexTransactions(string bankAccountNumber, int? page, int? size)
        {
            Profile user;
            List<Transaction> transactions = new List<Transaction>();
            ViewBag.BankAccountNumber = bankAccountNumber;


            if (User.IsInRole("Admin"))
            {
                transactions = db.Transactions
                    .Include(t => t.TransactionType)
                    .Include(t => t.CurrencyTo)
                    .OrderByDescending(t => t.Date).ThenByDescending(t => t.ID).ToList();
            }
            else
            {
                user = db.Profiles.Single(p => p.Email == User.Identity.Name);
                if (user.BankAccounts.Any(b => b.BankAccountNumber == bankAccountNumber))
                {
                    transactions = db.Transactions
                    .Where(t => bankAccountNumber == t.FromBankAccountNumber || bankAccountNumber == t.ToBankAccountNumber)
                    .Include(t => t.TransactionType)
                    .Include(t => t.CurrencyTo)
                    .OrderByDescending(t => t.Date).ThenByDescending(t => t.ID).ToList();
                }
            }


            int pageSize = (size ?? 10);
            int pageNumber = (page ?? 1);

            ViewBag.Count = transactions.Count;

            return PartialView("IndexTransfersList", transactions.ToPagedList(pageNumber, pageSize));
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
                var exchangedCurrency = CurrenciesController.ExchangeCurrencyBid(bankAccount.Currency.Code, currencyTo.Code, transaction.ValueTo);
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

                if (value == 0)
                {
                    ModelState.AddModelError("ValueTo", "Kwota jest zbyt niska");
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
                transaction.CurrencyFrom = bankAccount.Currency;

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

        [Authorize(Roles = "Admin,Worker")]
        [HttpPost]
        public string CashDeposit(string bankAccountNumber, string value)
        {
            var bankAccount = db.BankAccounts.SingleOrDefault(b => b.BankAccountNumber == bankAccountNumber);
            decimal.TryParse(value, out decimal deciamlValue);

            bankAccount.AvailableFounds += deciamlValue;
            bankAccount.Balance += deciamlValue;

            Transaction transaction = new Transaction
            {
                ValueFrom = deciamlValue,
                BalanceAfterTransactionUserFrom = bankAccount.Balance,

                FromBankAccountNumber = bankAccount.BankAccountNumber,
                ToBankAccountNumber = bankAccount.BankAccountNumber,
                ReceiverName = db.Profiles.Single(p => p.BankAccounts.Any(b => b.BankAccountNumber == bankAccountNumber)).FullName,
                Description = "Wpłana na konto",
                CurrencyFrom = bankAccount.Currency,

                TransactionTypeID = db.TransactionTypes.Single(t => t.Type == "CASH_DEPOSIT").ID,

                OperationDate = DateTime.Now,
                Date = DateTime.Now
            };

            db.Transactions.Add(transaction);
            db.SaveChanges();
            return "true";
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpPost]
        public string CashWithdrawal(string bankAccountNumber, string value)
        {
            var bankAccount = db.BankAccounts.SingleOrDefault(b => b.BankAccountNumber == bankAccountNumber);
            decimal.TryParse(value, out decimal deciamlValue);
            if (bankAccount.AvailableFounds >= deciamlValue)
            {
                bankAccount.AvailableFounds -= deciamlValue;
                bankAccount.Balance -= deciamlValue;

                Transaction transaction = new Transaction
                {
                    ValueFrom = deciamlValue,
                    BalanceAfterTransactionUserFrom = bankAccount.Balance,

                    FromBankAccountNumber = bankAccount.BankAccountNumber,
                    ToBankAccountNumber = bankAccount.BankAccountNumber,
                    ReceiverName = db.Profiles.Single(p => p.BankAccounts.Any(b => b.BankAccountNumber == bankAccountNumber)).FullName,
                    Description = "Wypłata z konta",
                    CurrencyFrom = bankAccount.Currency,

                    TransactionTypeID = db.TransactionTypes.Single(t => t.Type == "CASH_WITHDRAWAL").ID,

                    OperationDate = DateTime.Now,
                    Date = DateTime.Now
                };

                db.Transactions.Add(transaction);

                db.SaveChanges();
                return "true";
            } 
            else
            {
                return "false";
            }  
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
