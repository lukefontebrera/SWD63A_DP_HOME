using CatalogAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogAPI.Services
{
    public class UpcomingService
    {
        private readonly IMongoCollection<Movie> _movieCollection;

        public UpcomingService(
            IOptions<DatabaseSettings> orderDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                orderDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                orderDatabaseSettings.Value.DatabaseName);

            _movieCollection = mongoDatabase.GetCollection<Movie>(
                orderDatabaseSettings.Value.CatalogCollectionName);
        }

        public async Task<List<Movie>> GetAsync() =>
            await _movieCollection.Find(_ => true).ToListAsync();

        public async Task<Movie?> GetAsync(string id) =>
            await _movieCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Movie newProduct) =>
            await _movieCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Movie updatedProduct) =>
            await _movieCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _movieCollection.DeleteOneAsync(x => x.Id == id);
    }
}
