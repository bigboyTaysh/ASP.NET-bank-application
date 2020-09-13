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
    public class CreditsController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Credits
        public ActionResult Index()
        {
            ViewBag.Message = "";
            if (User.IsInRole("Admin") || User.IsInRole("Worker"))
            {
                return View(db.Credits
                    .OrderByDescending(c => c.StartDate)
                    .ThenByDescending(c => c.ID).ToList());
            }
            else
            {
                return View(db.Profiles.Single(p => p.Login == User.Identity.Name).Credits
                    .OrderByDescending(c => c.StartDate)
                    .ThenByDescending(c => c.ID).ToList());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayOffOneInstallment(int ID)
        {
            var profile = db.Profiles
                        .Single(p => p.Credits
                        .Any(c => c.ID == ID));

            var bankAccount = profile.BankAccounts
                .First(b => b.BankAccountType.Type == "PAY_ACC_FOR_YOUNG" || b.BankAccountType.Type == "PAY_ACC_FOR_ADULT");

            var credit = profile.Credits.Single(c => c.ID == ID);

            if (bankAccount.AvailableFounds >= credit.MonthRepayment)
            {
                bankAccount.Balance -= credit.MonthRepayment;
                bankAccount.AvailableFounds -= credit.MonthRepayment;

                Transaction transaction = new Transaction
                {
                    ValueTo = credit.MonthRepayment,
                    ValueFrom = credit.MonthRepayment,
                    BalanceAfterTransactionUserTo = bankAccount.Balance,
                    CurrencyTo = bankAccount.Currency,
                    ToBankAccountNumber = bankAccount.BankAccountNumber,
                    FromBankAccountNumber = bankAccount.BankAccountNumber,

                    TransactionTypeID = db.TransactionTypes.Single(t => t.Type == "CREDIT_TRANSFER").ID,
                    Description = $"Spłata raty {credit.NumberOfMonths - credit.NumberOfMonthsToEnd} kredytu {credit.ID}",
                    ReceiverName = profile.FullName,

                    OperationDate = DateTime.Now,
                    Date = DateTime.Now
                };
                db.Transactions.Add(transaction);

                credit.CurrentRepayment += credit.MonthRepayment;
                credit.NumberOfMonthsToEnd--;
                credit.LastPayment = DateTime.Now;

                if (credit.NumberOfMonthsToEnd == 0)
                {
                    credit.IsPaidOff = true;
                    credit.EndDate = DateTime.Now;
                }

                db.SaveChanges();

                TempData["Message"] = "Pomyślnie spłacono ratę";
            } 
            else
            {
                TempData["Message"] = "Brak wystarczających środków";
            }

            return RedirectToAction("Details", new { id = ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayOffCountOfNumberInstallments(int ID, int numberOfMonths)
        {
            var profile = db.Profiles
                        .Single(p => p.Credits
                        .Any(c => c.ID == ID));

            var bankAccount = profile.BankAccounts
                .First(b => b.BankAccountType.Type == "PAY_ACC_FOR_YOUNG" || b.BankAccountType.Type == "PAY_ACC_FOR_ADULT");

            var credit = profile.Credits.Single(c => c.ID == ID);

            var value = credit.MonthRepayment * numberOfMonths;

            if (bankAccount.AvailableFounds >= value)
            {
                bankAccount.Balance -= value;
                bankAccount.AvailableFounds -= value;

                Transaction transaction = new Transaction
                {
                    ValueTo = value,
                    ValueFrom = value,
                    BalanceAfterTransactionUserTo = bankAccount.Balance,
                    CurrencyTo = bankAccount.Currency,
                    ToBankAccountNumber = bankAccount.BankAccountNumber,
                    FromBankAccountNumber = bankAccount.BankAccountNumber,

                    TransactionTypeID = db.TransactionTypes.Single(t => t.Type == "CREDIT_TRANSFER").ID,
                    Description = $"Spłata kredytu {credit.ID}, rat: {numberOfMonths}",
                    ReceiverName = profile.FullName,

                    OperationDate = DateTime.Now,
                    Date = DateTime.Now
                };
                db.Transactions.Add(transaction);

                credit.CurrentRepayment += value;
                credit.NumberOfMonthsToEnd -= numberOfMonths;
                credit.LastPayment = DateTime.Now;

                if (credit.NumberOfMonthsToEnd == 0)
                {
                    credit.IsPaidOff = true;
                    credit.EndDate = DateTime.Now;
                }

                db.SaveChanges();

                TempData["Message"] = "Pomyślnie spłacono ratę";
            }
            else
            {
                TempData["Message"] = "Brak wystarczających środków";
            }

            return RedirectToAction("Details", new { id = ID });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayOffAll(int ID)
        {
            var profile = db.Profiles
                        .Single(p => p.Credits
                        .Any(c => c.ID == ID));

            var bankAccount = profile.BankAccounts
                .First(b => b.BankAccountType.Type == "PAY_ACC_FOR_YOUNG" || b.BankAccountType.Type == "PAY_ACC_FOR_ADULT");

            var credit = profile.Credits.Single(c => c.ID == ID);

            var value = credit.TotalRepayment - credit.CurrentRepayment;

            if (bankAccount.AvailableFounds >= value)
            {
                bankAccount.Balance -= value;
                bankAccount.AvailableFounds -= value;

                Transaction transaction = new Transaction
                {
                    ValueTo = value,
                    ValueFrom = value,
                    BalanceAfterTransactionUserTo = bankAccount.Balance,
                    CurrencyTo = bankAccount.Currency,
                    ToBankAccountNumber = bankAccount.BankAccountNumber,
                    FromBankAccountNumber = bankAccount.BankAccountNumber,

                    TransactionTypeID = db.TransactionTypes.Single(t => t.Type == "CREDIT_TRANSFER").ID,
                    Description = $"Całkowita spłata kredytu {credit.ID}",
                    ReceiverName = profile.FullName,

                    OperationDate = DateTime.Now,
                    Date = DateTime.Now
                };
                db.Transactions.Add(transaction);

                credit.CurrentRepayment += value;
                credit.NumberOfMonthsToEnd = 0;
                credit.LastPayment = DateTime.Now;
                credit.EndDate = DateTime.Now;
                credit.IsPaidOff = true;

                db.SaveChanges();

                TempData["Message"] = "Pomyślnie spłacono kredyt";
            }
            else
            {
                TempData["Message"] = "Brak wystarczających środków";
            }

            return RedirectToAction("Details", new { id = ID });
        }

        // GET: Credits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credit credit = db.Credits.Find(id);
            if (credit == null)
            {
                return HttpNotFound();
            }

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            else
            {
                ViewBag.Message = "";
            }
            return View(credit);
        }

        // GET: Credits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Credits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreditAmount,TotalRepayment,CurrentRepayment,MonthRepayment,NumberOfMonths,NumberOfMonthsToEnd,StartDate,EndDate,LastPayment,IsPaidOff")] Credit credit)
        {
            if (ModelState.IsValid)
            {
                db.Credits.Add(credit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(credit);
        }

        // GET: Credits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credit credit = db.Credits.Find(id);
            if (credit == null)
            {
                return HttpNotFound();
            }
            return View(credit);
        }

        // POST: Credits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreditAmount,TotalRepayment,CurrentRepayment,MonthRepayment,NumberOfMonths,NumberOfMonthsToEnd,StartDate,EndDate,LastPayment,IsPaidOff")] Credit credit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(credit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(credit);
        }

        // GET: Credits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credit credit = db.Credits.Find(id);
            if (credit == null)
            {
                return HttpNotFound();
            }
            return View(credit);
        }

        // POST: Credits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Credit credit = db.Credits.Find(id);
            db.Credits.Remove(credit);
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
