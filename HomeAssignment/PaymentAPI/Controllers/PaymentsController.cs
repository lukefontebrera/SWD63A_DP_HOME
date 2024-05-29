using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Models;
using PaymentAPI.Services;
using System.Text;
using BasketAPI.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;


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
                    var response = await httpClient.GetAsync($"http://localhost:5003/gateway/Payments");
                    Console.WriteLine($"Received response: {(int)response.StatusCode} - {response.ReasonPhrase}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Process the payment
                        payment.Id = GenerateId();
                        await _service.CreateAsync(payment);

                        // Delete of BasketItems is handled in Order instead

                        return Ok();
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

        private string GenerateId()
        {
            Random random = new Random();
            StringBuilder hexString = new StringBuilder(24);

            for (int i = 0; i < 24; i++)
            {
                int randomNumber = random.Next(16); 
                hexString.Append(randomNumber.ToString("X"));
            }

            return hexString.ToString();
        }
    }
}
