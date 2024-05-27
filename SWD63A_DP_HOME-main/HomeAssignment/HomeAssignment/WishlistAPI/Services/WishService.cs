using WishlistAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WishlistAPI.Services
{
    public class WishService
    {
        private readonly IMongoCollection<WishedMovie> _wishedMovieCollection;

        public WishService(
            IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _wishedMovieCollection = mongoDatabase.GetCollection<WishedMovie>(
                databaseSettings.Value.CatalogCollectionName);
        }

        public async Task<List<WishedMovie>> GetAsync() =>
            await _wishedMovieCollection.Find(_ => true).ToListAsync();

        public List<WishedMovie> Get() => _wishedMovieCollection.Find(_ => true).ToList();

        public async Task<WishedMovie?> GetAsync(string id) =>
            await _wishedMovieCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(WishedMovie newProduct) =>
            await _wishedMovieCollection.InsertOneAsync(newProduct);

        public void Create(WishedMovie newProduct) => _wishedMovieCollection.InsertOne(newProduct);

        public async Task UpdateAsync(string id, WishedMovie updatedProduct) =>
            await _wishedMovieCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _wishedMovieCollection.DeleteOneAsync(x => x.Id == id);
    }
}
