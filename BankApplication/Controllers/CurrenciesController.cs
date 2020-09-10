using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BankApplication.DAL;
using BankApplication.Helper;
using Newtonsoft.Json;

namespace BankApplication.Models
{
    public class CurrenciesController : Controller
    {
        private BankContext db = new BankContext();
        private HttpClient client = new HttpClient();

        // GET: Currencies
        public ActionResult Index()
        {
            RefreshCurrency.RefreshCurrenciesAsync().ConfigureAwait(false);
            return View(db.Currencies.Where(c => c.Code != "PLN").ToList());
        }

        [HttpPost]
        public PartialViewResult GetBankAccounts(string code)
        {
            var bankAccounts = db.Profiles.Single(p => p.Login == User.Identity.Name).BankAccounts.Where(b => b.Currency.Code == code).ToList();
            return PartialView("BankAccountList", bankAccounts);
        }

        [HttpPost]
        public string GetExchangeResult(string type, decimal value, string from, string to)
        {
            if (type == "bid")
            {
                return decimal.Round(ExchangeCurrencyBid(from, to, value), 2).ToString();
            } else
            {
                return decimal.Round(ExchangeCurrencyAsk(from, to, value), 2).ToString();
            }
            
        }

        [HttpPost]
        public string ExchangeCurrency(string type, decimal value, string fromBankAccountNumber, string toBankAccountNumber)
        {
            Transaction transaction = new Transaction();
            var fromBankAccount = db.BankAccounts.SingleOrDefault(b => b.BankAccountNumber== fromBankAccountNumber);
            var toBankAccount = db.BankAccounts.SingleOrDefault(b => b.BankAccountNumber == toBankAccountNumber);
            decimal valueFrom;

            if (type == "bid")
            {
                valueFrom = decimal.Round(ExchangeCurrencyBid(fromBankAccount.Currency.Code, toBankAccount.Currency.Code, value), 2);
                fromBankAccount.Balance -= valueFrom;
                fromBankAccount.AvailableFounds -= valueFrom;
                transaction.ValueFrom = valueFrom;
                transaction.ValueTo = value;
                toBankAccount.Balance += value;
                toBankAccount.AvailableFounds += value;
                transaction.BalanceAfterTransactionUserFrom = fromBankAccount.Balance;
                transaction.BalanceAfterTransactionUserTo = toBankAccount.Balance;
            }
            else
            {
                valueFrom = decimal.Round(ExchangeCurrencyAsk(fromBankAccount.Currency.Code, toBankAccount.Currency.Code, value), 2);

                fromBankAccount.Balance -= value;
                fromBankAccount.AvailableFounds -= value;
                transaction.ValueFrom = value;

                transaction.ValueTo = valueFrom;
                toBankAccount.Balance += valueFrom;
                toBankAccount.AvailableFounds += valueFrom;
                transaction.BalanceAfterTransactionUserFrom = fromBankAccount.Balance;
                transaction.BalanceAfterTransactionUserTo = toBankAccount.Balance;
            }

            try
            {

                
                transaction.CurrencyTo = toBankAccount.Currency;
                transaction.ToBankAccountNumber = toBankAccount.BankAccountNumber;

                transaction.FromBankAccountNumber = fromBankAccount.BankAccountNumber;
                transaction.CurrencyFrom = fromBankAccount.Currency;

                transaction.TransactionTypeID = db.TransactionTypes.Single(t => t.Type == "CURR_EXCHANGE").ID;
                transaction.Description = "Wymiana waluty";
                transaction.ReceiverName = db.Profiles.Single(p => p.Login == User.Identity.Name).FullName;

                transaction.OperationDate = DateTime.Now;
                transaction.Date = DateTime.Now;
                db.Transactions.Add(transaction);
                db.SaveChanges();

                return "true";
            }
            catch (Exception e)
            {
                return "false";
            } 
        }

        // GET: Currencies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = db.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // GET: Currencies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Currencies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Code,EffectiveDate,Bid,Ask")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                db.Currencies.Add(currency);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(currency);
        }

        // GET: Currencies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = db.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Currencies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Code,EffectiveDate,Bid,Ask")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                db.Entry(currency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(currency);
        }

        // GET: Currencies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = db.Currencies.Find(id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Currency currency = db.Currencies.Find(id);
            db.Currencies.Remove(currency);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public static decimal ExchangeCurrencyBid(string from, string to, decimal value)
        {
            BankContext db = new BankContext();
            return db.Currencies.Single( c => c.Code == to).Ask * value / db.Currencies.Single(c => c.Code == from).Bid;
        }

        public static decimal ExchangeCurrencyAsk(string from, string to, decimal value)
        {
            BankContext db = new BankContext();
            return db.Currencies.Single(c => c.Code == from).Bid * value / db.Currencies.Single(c => c.Code == to).Ask;
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
