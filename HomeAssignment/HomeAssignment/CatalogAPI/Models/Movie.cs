using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CatalogAPI.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
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
