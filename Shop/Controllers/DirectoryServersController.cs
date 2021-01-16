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
    public class DirectoryServersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DirectoryServersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DirectoryServers
        [HttpGet]
        public async Task<ActionResult<DirectoryServer>> GetDirectoryServers()
        {
            return await _context.DirectoryServers.FirstOrDefaultAsync();
        }

        // GET: api/DirectoryServers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DirectoryServer>> GetDirectoryServer(int id)
        {
            var directoryServer = await _context.DirectoryServers.FindAsync(id);

            if (directoryServer == null)
            {
                return NotFound();
            }

            return directoryServer;
        }

        // PUT: api/DirectoryServers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirectoryServer(int id, DirectoryServer directoryServer)
        {
            if (id != directoryServer.ID)
            {
                return BadRequest();
            }

            _context.Entry(directoryServer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectoryServerExists(id))
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

        // POST: api/DirectoryServers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DirectoryServer>> PostDirectoryServer(DirectoryServer directoryServer)
        {
            _context.DirectoryServers.Add(directoryServer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDirectoryServer", new { id = directoryServer.ID }, directoryServer);
        }

        // DELETE: api/DirectoryServers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirectoryServer(int id)
        {
            var directoryServer = await _context.DirectoryServers.FindAsync(id);
            if (directoryServer == null)
            {
                return NotFound();
            }

            _context.DirectoryServers.Remove(directoryServer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DirectoryServerExists(int id)
        {
            return _context.DirectoryServers.Any(e => e.ID == id);
        }
    }
}
