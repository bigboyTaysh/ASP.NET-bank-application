using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.Status)
                .SingleAsync(o => o.ID == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.ID)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders/PostOrder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("PostOrder")]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            order.Date = DateTime.Now;
            order.Status = _context.OrderStatuses.SingleOrDefault(o => o.Status == "Awaiting Payment");
            order.Currency = "PLN";
            order.Items.ForEach(i => i.Product = _context.Products.Find(i.Product.ID));

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.ID }, order);
        }

        [HttpPost("UpdateStatus")]
        public async Task<ActionResult> UpdateStatus(UpdateOrderStatusJson orderInfo)
        {
            if (_context.Banks.Any(b => b.ApiKey == orderInfo.ApiKey))
            {
                var order = await _context.Orders.SingleOrDefaultAsync(o => o.ID == orderInfo.ID);
                if (order != null)
                {
                    if (orderInfo.Status)
                    {
                        order.Status = await _context.OrderStatuses.SingleOrDefaultAsync(s => s.Status == "Completed");
                    }
                    else
                    {
                        order.Status = await _context.OrderStatuses.SingleOrDefaultAsync(s => s.Status == "Declined");
                    }
                    order.Date = DateTime.Now;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }
    }
}
