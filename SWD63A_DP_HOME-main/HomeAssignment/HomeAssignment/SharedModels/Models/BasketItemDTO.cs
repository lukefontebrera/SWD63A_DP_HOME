using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedModels.Models
{
    public class BasketItemDTO
    {
        public string? Id { get; set; }

        public string? Title { get; set; }

        public decimal UnitPrice { get; set; }

        public string? PictureUri { get; set; }

        public string? MovieId { get; set; }
    }
}
