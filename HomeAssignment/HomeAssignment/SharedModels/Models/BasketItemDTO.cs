using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedModels.Models
{
    public class BasketItemDTO
    {
        public string? Id { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductId { get; set; }

        [Range(1, 999)]
        public long Quantity { get; set; }
    }
}
