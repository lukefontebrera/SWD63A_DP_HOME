using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BasketAPI.Models;

namespace BasketAPI.Services
{
    public class BasketService
    {
        private readonly IMongoCollection<BasketItem> _basketCollection;

        public BasketService(
            IOptions<BasketDatabaseSettings> basketDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                basketDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                basketDatabaseSettings.Value.DatabaseName);

            _basketCollection = mongoDatabase.GetCollection<BasketItem>(
                basketDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<BasketItem>> GetAsync() =>
            await _basketCollection.Find(_ => true).ToListAsync();

        public async Task<BasketItem?> GetAsync(string id) =>
            await _basketCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(BasketItem newProduct) =>
            await _basketCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, BasketItem updatedProduct) =>
            await _basketCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _basketCollection.DeleteOneAsync(x => x.Id == id);
    }
}
