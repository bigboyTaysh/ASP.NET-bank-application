using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankApplication.Data;
using BankApplication.Models;
using PagedList;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BankApplication.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /*
        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }
        */

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsAsync(string accountNumber, int? size, int? page)
        {
            Profile profile;
            List<Transaction> transactions = new List<Transaction>();
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);


            if (roles.Any(r => r == "Admin"))
            {
                transactions = _context.Transactions
                    .Include(t => t.TransactionType)
                    .Include(t => t.CurrencyTo)
                    .OrderByDescending(t => t.Date).ThenByDescending(t => t.ID).ToList();
            }
            else
            {
                profile = _context.Profiles.Include(p => p.BankAccounts).Single(p => p.Email == user.Email);
                if (profile.BankAccounts.Any(b => b.BankAccountNumber == accountNumber))
                {
                    transactions = _context.Transactions
                    .Where(t => accountNumber == t.FromBankAccountNumber || accountNumber == t.ToBankAccountNumber)
                    .Include(t => t.TransactionType)
                    .Include(t => t.CurrencyTo)
                    .OrderByDescending(t => t.Date).ThenByDescending(t => t.ID).ToList();
                }
            }


            int pageSize = (size ?? 10);
            int pageNumber = (page ?? 1);

            return transactions.ToPagedList(pageNumber, pageSize).ToList();
        }

        // GET: api/Transactions/5
        [HttpGet("byId/{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.ID)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.ID }, transaction);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.ID == id);
        }
    }
}
