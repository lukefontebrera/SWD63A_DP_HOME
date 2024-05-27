using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WishlistAPI.Models
{
    public class WishedMovie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; } = null!;
    }
}
