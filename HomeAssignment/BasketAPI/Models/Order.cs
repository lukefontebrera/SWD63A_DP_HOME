using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BasketAPI.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; }

        public string[] MovieIds { get; set; }
    }
}
