using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BasketAPI.Models
{
    public class BasketItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Title { get; set; }

        public string? PictureUri { get; set; }

        public decimal UnitPrice { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? MovieId { get; set; }
    }
}
