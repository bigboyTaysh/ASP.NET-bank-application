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
    public class AcquirersController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Acquirers
        public async Task<ActionResult> Index()
        {
            return View(await db.Acquirers.ToListAsync());
        }

        // GET: Acquirers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acquirer acquirer = await db.Acquirers.FindAsync(id);
            if (acquirer == null)
            {
                return HttpNotFound();
            }
            return View(acquirer);
        }

        // GET: Acquirers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Acquirers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,URL,OrderDetailsPath,UpdateOrderStatusPath,OrderSummaryPath,Description")] Acquirer acquirer)
        {
            if (ModelState.IsValid)
            {
                db.Acquirers.Add(acquirer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(acquirer);
        }

        // GET: Acquirers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acquirer acquirer = await db.Acquirers.FindAsync(id);
            if (acquirer == null)
            {
                return HttpNotFound();
            }
            return View(acquirer);
        }

        // POST: Acquirers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,URL,OrderDetailsPath,UpdateOrderStatusPath,OrderSummaryPath,Description")] Acquirer acquirer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acquirer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(acquirer);
        }

        // GET: Acquirers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acquirer acquirer = await db.Acquirers.FindAsync(id);
            if (acquirer == null)
            {
                return HttpNotFound();
            }
            return View(acquirer);
        }

        // POST: Acquirers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Acquirer acquirer = await db.Acquirers.FindAsync(id);
            db.Acquirers.Remove(acquirer);
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
