using DirectoryServer.Data;
using DirectoryServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DirectoryServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        static HttpClient client = new HttpClient();

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<PaymentController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("cardSecure")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CardSecure([FromBody] SecureCardJson json)
        {
            if (!_context.Acquirers.Any(a => a.ApiKey == json.apiKey))
            {
                return StatusCode(403);
            }

            var bank = _context.Banks.FirstOrDefault();
            json.apiKey = bank.ApiKey;

            HttpResponseMessage response = await client.PostAsJsonAsync(bank.URL + "paymentCards/cardSecured", json);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            } 
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        /*
        // GET api/<PaymentController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PaymentController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PaymentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PaymentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
