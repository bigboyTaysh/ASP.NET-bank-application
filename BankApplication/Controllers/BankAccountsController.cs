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
using PagedList;

namespace BankApplication.Controllers
{
    [Authorize]
    public class BankAccountsController : Controller
    {
        private BankContext db = new BankContext();

        [HttpGet]
        // GET: BankAccounts
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? size)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumberSortParm = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var bankAccounts = from b in db.BankAccounts
                               select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                bankAccounts = bankAccounts.Where(s => s.BankAccountNumber.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "number_desc":
                    bankAccounts = bankAccounts.OrderByDescending(s => s.BankAccountNumber);
                    break;
                case "date":
                    bankAccounts = bankAccounts.OrderBy(b => b.CreationDate);
                    break;
                case "date_desc":
                    bankAccounts = bankAccounts.OrderByDescending(b => b.CreationDate);
                    break;
                default:
                    bankAccounts = bankAccounts.OrderBy(b => b.CreationDate);
                    break;
            }

            int pageSize = (size ?? 5);
            int pageNumber = (page ?? 1);

            return View(bankAccounts.Include(b => b.BankAccountType).ToPagedList(pageNumber, pageSize));
        }

        [ActionName("Index")]
        [HttpPost]
        public PartialViewResult IndexPost(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumberSortParm = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var bankAccounts = from b in db.BankAccounts
                               select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                bankAccounts = bankAccounts.Where(s => s.BankAccountNumber.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "number_desc":
                    bankAccounts = bankAccounts.OrderByDescending(s => s.BankAccountNumber);
                    break;
                case "date":
                    bankAccounts = bankAccounts.OrderBy(b => b.CreationDate);
                    break;
                case "date_desc":
                    bankAccounts = bankAccounts.OrderByDescending(b => b.CreationDate);
                    break;
                default:
                    bankAccounts = bankAccounts.OrderBy(b => b.CreationDate);
                    break;
            }

            int pageSize = 3;
            int pageNumber = page ?? 1;

            return PartialView("SearchResults", bankAccounts.Include(b => b.BankAccountType).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Admin")]
        // GET: BankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        [Authorize(Roles = "Admin")]
        // GET: BankAccounts/Create
        public ActionResult Create()
        {
            ViewBag.BankAccountTypeID = new SelectList(db.BankAccountTypes, "ID", "Type");
            ViewData["Message"] = "";
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: BankAccounts/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Balance,AvailableFounds,Lock,BankAccountNumber,CreationDate,BankAccountTypeID")] BankAccount bankAccount)
        {
            //HttpPostedFileBase file = Request.Files["FileName"];

            //if (ModelState.IsValid && file != null && file.ContentLength > 0)
            if (ModelState.IsValid)
            {
                //bankAccount.FileName = file.FileName;
                //file.SaveAs(HttpContext.Server.MapPath("~/Images/") + bankAccount.FileName);

                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BankAccountTypeID = new SelectList(db.BankAccountTypes, "ID", "Type", bankAccount.BankAccountTypeID);
            return View(bankAccount);
        }

        [Authorize(Roles = "Admin")]
        // GET: BankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankAccountTypeID = new SelectList(db.BankAccountTypes, "ID", "Type", bankAccount.BankAccountTypeID);
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "Code");
            return View(bankAccount);
        }

        [Authorize(Roles = "Admin")]
        // POST: BankAccounts/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Balance,AvailableFounds,Lock,BankAccountNumber,CreationDate,BankAccountTypeID,CurrencyToID")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BankAccountTypeID = new SelectList(db.BankAccountTypes, "ID", "Type", bankAccount.BankAccountTypeID);
            ViewBag.CurrencyID = new SelectList(db.Currencies, "ID", "Code");
            return View(bankAccount);
        }

        [Authorize(Roles = "Admin")]
        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        [Authorize(Roles = "Admin")]
        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BankAccounts.Find(id);
            db.BankAccounts.Remove(bankAccount);
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
