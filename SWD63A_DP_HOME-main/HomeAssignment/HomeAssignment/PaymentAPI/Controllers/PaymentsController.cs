using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PaymentAPI.Models;
using PaymentAPI.Services;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _service;

        public PaymentsController(PaymentService service)
        {
            _service = service;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _service.GetAsync();
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(string id)
        {
            var payment = await _service.GetAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }


        // POST: api/Payments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"http://localhost:5003/gateway/Payments/");
                    Console.WriteLine($"Received response: {(int)response.StatusCode} - {response.ReasonPhrase}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Payment details: {content}");

                        var jsonContent = JArray.Parse(content); // Parse the response as a JArray

                        if (jsonContent == null || !jsonContent.Any())
                        {
                            Console.WriteLine("Payment content is null or empty.");
                            return NotFound("Payment content is null or empty.");
                        }

                        var firstItem = jsonContent.First;

                        if (firstItem == null)
                        {
                            Console.WriteLine("No items found in the response.");
                            return NotFound("No items found in the response.");
                        }

                        payment.UserId = firstItem["userId"].Value<string>();
                        payment.Amount = firstItem["amount"].Value<int>();
                        payment.Timestamp = firstItem["timestamp"].Value<DateTime>();
                        payment.MovieIds = firstItem["movieIds"].Value <string[]>();

                        await _service.CreateAsync(payment);

                        return CreatedAtAction("GetBasketItem", new { id = payment.Id }, payment);
                    }
                    else
                    {
                        Console.WriteLine($"Payment not found: {response.ReasonPhrase}");
                        return NotFound("Payment not found.");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"HTTP Request error: {e.Message}");
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, "Error fetching movie data.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
                }
            }
        }

        private async Task<bool> PaymentExists(string id)
        {
            var payment = await _service.GetAsync(id);
            return payment != null;
        }
    }
}
