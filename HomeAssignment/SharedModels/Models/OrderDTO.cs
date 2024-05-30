using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models
{
    public class OrderDTO
    {
        public string? Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; }

        public BasketItemDTO[] Movies { get; set; }

        public string? User { get; set; }
    }
}
