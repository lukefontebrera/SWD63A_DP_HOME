using BasketAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BasketAPI.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orderCollection;

        public OrderService(
            IOptions<OrderDatabaseSettings> orderDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                orderDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                orderDatabaseSettings.Value.DatabaseName);

            _orderCollection = mongoDatabase.GetCollection<Order>(
                orderDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<Order>> GetAsync() =>
            await _orderCollection.Find(_ => true).ToListAsync();

        public async Task<Order?> GetAsync(string id) =>
            await _orderCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Order newProduct) =>
            await _orderCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Order updatedProduct) =>
            await _orderCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _orderCollection.DeleteOneAsync(x => x.Id == id);
    }
}
