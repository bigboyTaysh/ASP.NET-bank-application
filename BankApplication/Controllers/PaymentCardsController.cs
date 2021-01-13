using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankApplication.DAL;
using BankApplication.Models;
using System.Net.Http;
using System.Web.Http.Description;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using BankApplication.Helper;

namespace BankApplication.Controllers

{
    [Authorize]
    public class PaymentCardsController : Controller
    {
        private BankContext db = new BankContext();
        static HttpClient client = new HttpClient();

        // GET: PaymentCards
        public async Task<ActionResult> Index()
        {
            List<PaymentCard> paymentCards;

            if (User.IsInRole("Admin") || User.IsInRole("Worker"))
            {
                paymentCards = db.PaymentCards
                    .Include(p => p.BankAccount).ToList();
            }
            else
            {
                var profile = await db.Profiles
                    .Include(p => p.BankAccounts)
                    .SingleOrDefaultAsync(p => p.Email == User.Identity.Name);

                var bankAccounts = profile.BankAccounts.Select(b => b.ID);

                paymentCards = db.PaymentCards
                    .Include(p => p.BankAccount)
                    .Where(b => bankAccounts.Any(p => p == b.ID))
                    .ToList();
            }    

            return View(paymentCards);
        }

        // GET: PaymentCards/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentCard paymentCard = await db.PaymentCards.FindAsync(id);
            if (paymentCard == null)
            {
                return HttpNotFound();
            }
            return View(paymentCard);
        }

        // GET: PaymentCards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,PaymentCardNumber,Code,Blocked,SecureCard")] PaymentCard paymentCard)
        {
            if (ModelState.IsValid)
            {
                db.PaymentCards.Add(paymentCard);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(paymentCard);
        }

        // GET: PaymentCards/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentCard paymentCard = await db.PaymentCards.FindAsync(id);
            if (paymentCard == null)
            {
                return HttpNotFound();
            }
            return View(paymentCard);
        }

        // POST: PaymentCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,PaymentCardNumber,Code,Blocked,SecureCard")] PaymentCard paymentCard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentCard).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(paymentCard);
        }

        // GET: PaymentCards/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentCard paymentCard = await db.PaymentCards.FindAsync(id);
            if (paymentCard == null)
            {
                return HttpNotFound();
            }
            return View(paymentCard);
        }

        // POST: PaymentCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PaymentCard paymentCard = await db.PaymentCards.FindAsync(id);
            db.PaymentCards.Remove(paymentCard);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: PaymentCards/CardSecured/5
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CardSecured(string apiKey, string cardNumber, string code)
        {
            if (db.DirectoryServers.Any(d => d.ApiKey == apiKey))
            {
                var paymentCard = db.PaymentCards.SingleOrDefaultAsync(p => p.PaymentCardNumber == cardNumber && p.Code == code).Result;
                if (paymentCard != null)
                {
                    if (paymentCard.SecureCard)
                    {
                        return Json(new { status = true });
                    }
                    else
                    {
                        return Json(new { status = false });
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            } 
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        [HttpGet]
        public async Task<ActionResult> CardPayment(int? orderId, string apiKey, string cardNumber)
        {
            if (orderId == null || String.IsNullOrEmpty(apiKey) || String.IsNullOrEmpty(cardNumber)   )
            {
                return RedirectToAction("Index", "Home");
            }

            var acquirer = db.Acquirers.SingleOrDefault(a => a.ApiKey == apiKey);

            if (acquirer == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var profile = await db.Profiles
                    .Include(p => p.BankAccounts)
                    .SingleOrDefaultAsync(p => p.Email == User.Identity.Name);

            var bankAccounts = profile.BankAccounts.Select(b => b.ID);

            var paymentCards = db.PaymentCards
                .Include(p => p.BankAccount)
                .Where(b => bankAccounts.Any(p => p == b.ID))
                .ToList();

            if (!paymentCards.Any(p => p.PaymentCardNumber == cardNumber))
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return Redirect(acquirer.URL);
            }

            HttpResponseMessage response = await client.GetAsync(acquirer.URL + "api/orders/" + orderId);

            if (!response.IsSuccessStatusCode)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return Redirect(acquirer.URL);
            }

            var content = JsonConvert.DeserializeObject<AcquirerOrderJson>(response.Content.ReadAsStringAsync().Result);

            if (content.Status.Status != "Awaiting Payment")
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return Redirect(acquirer.URL);
            }

            TempData["orderId"] = orderId;
            TempData["cardNumber"] = cardNumber;
            TempData["price"] = content.Price;
            TempData["currency"] = content.Currency;
            TempData["acquirer"] = acquirer;

            return RedirectToAction("CardPaymentConfirmation");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", Location = OutputCacheLocation.None)]
        [HttpGet]
        public ActionResult CardPaymentConfirmation()
        {
            if (TempData["orderId"] == null ||
                TempData["cardNumber"] == null ||
                TempData["price"] == null ||
                TempData["acquirer"] == null ||
                TempData["currency"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int orderId = (int)TempData["orderId"];
            string cardNumber = TempData["cardNumber"].ToString();
            decimal price = (decimal)TempData["price"];
            string currency = TempData["currency"].ToString();
            Acquirer acquirer = (Acquirer)TempData["acquirer"];

            ViewBag.orderId = orderId;
            ViewBag.cardNumber = cardNumber;
            ViewBag.price = price;
            ViewBag.currency = currency;
            ViewBag.acquirer = acquirer;

            TempData["orderId"] = orderId;
            TempData["cardNumber"] = cardNumber;
            TempData["price"] = price;
            TempData["acquirer"] = acquirer;
            TempData["currency"] = currency;

            return View("Payment");
        }

        [HttpGet]
        public ActionResult Status(bool status)
        {
            if (TempData["orderId"] == null ||
                TempData["cardNumber"] == null ||
                TempData["price"] == null ||
                TempData["acquirer"] == null ||
                TempData["currency"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int orderId = (int)TempData["orderId"];
            string cardNumber = TempData["cardNumber"].ToString();
            decimal price = (decimal)TempData["price"];
            string currency = TempData["currency"].ToString();
            Acquirer acquirer = (Acquirer)TempData["acquirer"];

            TempData.Remove("orderId");
            TempData.Remove("cardNumber");
            TempData.Remove("price");
            TempData.Remove("acquirer");
            TempData.Remove("currency");

            if (status)
            {
                var bankAccount = db.PaymentCards
                    .Include(p => p.BankAccount)
                    .Single(b => b.PaymentCardNumber == cardNumber).BankAccount;

                var currencyTo = db.Currencies.SingleOrDefault(c => c.Code == currency);

                if (currencyTo != null)
                {
                    var commision = bankAccount.BankAccountType.Commission / 100;
                    decimal value;

                    if (bankAccount.Currency.Code == currencyTo.Code)
                    {
                        value = price + (price * commision);
                    }
                    else
                    {
                        var exchangedCurrency = CurrenciesController.ExchangeCurrencyBid(bankAccount.Currency.Code, currencyTo.Code, price);
                        value = exchangedCurrency + (exchangedCurrency * commision);
                    }

                    if (bankAccount.AvailableFounds >= value)
                    {
                        bankAccount.AvailableFounds -= value;
                        bankAccount.Balance -= value;

                        Transaction transaction = new Transaction
                        {
                            ValueTo = price,
                            BalanceAfterTransactionUserTo = bankAccount.Balance,

                            FromBankAccountNumber = bankAccount.BankAccountNumber,
                            ToBankAccountNumber = acquirer.BankAccountNumebr,
                            ReceiverName = acquirer.Name,
                            Description = "Płatność kartą",
                            CurrencyTo = bankAccount.Currency,

                            TransactionTypeID = db.TransactionTypes.Single(t => t.Type == "CARD_PAYMENT").ID,

                            Date = DateTime.Now
                        };

                        db.Transactions.Add(transaction);
                        db.SaveChanges();
                    }
                    else
                    {
                        status = false;
                    }
                }
                else
                {
                    status = false;
                }
            }

            client.PostAsJsonAsync(acquirer.URL + "api/orders/updateStatus", new { id = orderId, status = status, apiKey = acquirer.ApiKey});
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return Redirect(acquirer.URL + "summary/" + orderId);
        }

        public static string NewPaymentCardNumber()
        {
            BankContext db = new BankContext();
            string last = db.PaymentCards.OrderByDescending(b => b.PaymentCardNumber).First().PaymentCardNumber;
            int parts = int.Parse(last.Split(' ')[2] + last.Split(' ')[3]) + 1;
            string newNumber = last;
            newNumber = newNumber.Replace(newNumber.Split(' ')[2], parts.ToString().Substring(0, 4));
            newNumber = newNumber.Replace(newNumber.Split(' ')[3], parts.ToString().Substring(4, 4));

            return newNumber;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}
