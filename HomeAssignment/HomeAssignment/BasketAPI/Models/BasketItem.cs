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

        public string? MovieName { get; set; }

        public decimal UnitPrice { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? MovieId { get; set; }

        [Range(1, 999)]
        public long Quantity { get; set; }
    }
}
