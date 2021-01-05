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

namespace BankApplication.Controllers
{
    public class PaymentCardsController : Controller
    {
        private BankContext db = new BankContext();

        [Authorize]
        // GET: PaymentCards
        public async Task<ActionResult> Index()
        {
            var profile = await db.Profiles
                .SingleOrDefaultAsync(p => p.Login == User.Identity.Name);

            var paymentCards = profile.BankAccounts.Select(b => b.PaymentCard).Where(p => p != null).ToList();

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
