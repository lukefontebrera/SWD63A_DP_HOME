using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using SharedModels.Models;

namespace BasketAPI.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; }

        public BasketItemDTO[] Movies { get; set; }

        public string? User { get; set; }
    }
}
