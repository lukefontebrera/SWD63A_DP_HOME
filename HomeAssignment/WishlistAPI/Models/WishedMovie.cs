using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WishlistAPI.Models
{
    public class WishedMovie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Title { get; set; }

        public string? PictureUri { get; set; }

        public string? MovieId { get; set; }

        public string? User { get; set; }
    }
}
