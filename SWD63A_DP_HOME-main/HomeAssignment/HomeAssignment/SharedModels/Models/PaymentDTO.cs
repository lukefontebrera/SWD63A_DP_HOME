using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models
{
    public class PaymentDTO
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; }

        public string[] MovieIds { get; set; }
    }
}
