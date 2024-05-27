using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models
{
	public class MovieDTO
	{
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string? PictureUri { get; set; }

        public int ReleaseYear { get; set; }

        public string Genre { get; set; } = null!;

        public DateTime? ReleaseDate { get; set; }
    }
}
