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
                    .SingleOrDefaultAsync(p => p.Login == User.Identity.Name);

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
            if (orderId == null || String.IsNullOrEmpty(apiKey) || String.IsNullOrEmpty(cardNumber))
            {
                return RedirectToAction("Index", "Home");
            }

            var acquirer = db.Acquirers.SingleOrDefault(a => a.ApiKey == apiKey);

            if (acquirer == null)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpResponseMessage response = await client.GetAsync(acquirer.URL + "api/orders/" + orderId);

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            var content = JsonConvert.DeserializeObject<AcquirerOrderJson>(response.Content.ReadAsStringAsync().Result);

            if (content.Status.Status != "Awaiting Payment")
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["orderId"] = orderId;
            TempData["cardNumber"] = cardNumber;
            TempData["price"] = content.Price;
            TempData["acquirer"] = acquirer;

            return RedirectToAction("CardPaymentConfirmation");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", Location = OutputCacheLocation.None)]
        [HttpGet]
        public ActionResult CardPaymentConfirmation()
        {
            if (TempData["orderId"] == null || TempData["cardNumber"] == null || TempData["price"] == null || TempData["acquirer"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int orderId = (int)TempData["orderId"];
            string cardNumber = TempData["cardNumber"].ToString();
            decimal price = (decimal)TempData["price"];
            Acquirer acquirer = (Acquirer)TempData["acquirer"];

            ViewBag.orderId = orderId;
            ViewBag.cardNumber = cardNumber;
            ViewBag.price = price;
            ViewBag.acquirer = acquirer;

            TempData["statusOrderId"] = orderId;
            TempData["statusCardNumber"] = cardNumber;
            TempData["statusPrice"] = price;
            TempData["statusAcquirer"] = acquirer;

            TempData.Remove("orderId");
            TempData.Remove("cardNumber");
            TempData.Remove("price");
            TempData.Remove("acquirer");

            return View("Payment");
        }

        [HttpGet]
        public ActionResult Status(bool status)
        {
            if (TempData["statusOrderId"] == null ||
                TempData["statusCardNumber"] == null ||
                TempData["statusPrice"] == null ||
                TempData["statusAcquirer"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int orderId = (int)TempData["statusOrderId"];
            string cardNumber = TempData["statusCardNumber"].ToString();
            decimal price = (decimal)TempData["statusPrice"];
            Acquirer acquirer = (Acquirer)TempData["statusAcquirer"];

            TempData.Remove("statusOrderId");
            TempData.Remove("statusCardNumber");
            TempData.Remove("statusPrice");
            TempData.Remove("statusAcquirer");

            client.PostAsJsonAsync(acquirer.URL + "api/orders/updateStatus", new { id = orderId, status = status, apiKey = acquirer.ApiKey});
            
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return Redirect(acquirer.URL + "summary/" + orderId);
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
