using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentAPI.Models
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; }

        public string[] MovieIds { get; set; } 
    }
}
