using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CatalogAPI.Models
{
    public class Movie
    {
        [BsonId]
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Caption { get; set; } = null!;

        public decimal Price { get; set; }

        public string? PictureUri { get; set; }

        public int ReleaseYear { get; set; }

        public DateTime? ReleaseDate { get; set; }
    }
}
