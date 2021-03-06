﻿using System;
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
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Include(p => p.ProductCategories).ThenInclude(p => p.Category).Include(p => p.Pictures).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.ProductCategories).ThenInclude(p => p.Category).Include(p => p.Pictures).SingleOrDefaultAsync(p => p.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // GET: api/products/category/5
        [HttpGet("category/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryId(int id)
        {
            var product = await _context.Products
                .Where(p => p.ProductCategories.Any(p => p.CategoryID == id))
                .Include(p => p.ProductCategories)
                .ThenInclude(p => p.Category)
                .Include(p => p.Pictures)
                .ToListAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // GET: api/Products/5
        [HttpGet("{id}/categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetProductCategories(int id)
        {
            var categories = await _context.Categories.Where(p => p.ProductCategories.Any(p => p.ProductID == id)).Include(p => p.ProductCategories).ToListAsync();

            if (categories == null)
            {
                return NotFound();
            }

            return categories;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ID }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
