using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using PaymentAPI.Models;
using Publisher.Services;
using SharedModels.Models;
using System.Threading.Tasks;

namespace PaymentAPI.Services
{
    public class PaymentService
    {
        private readonly IMongoCollection<Payment> _paymentCollection;

        public PaymentService(
            IOptions<DatabaseSettings> paymentDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                paymentDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                paymentDatabaseSettings.Value.DatabaseName);

            _paymentCollection = mongoDatabase.GetCollection<Payment>(
                paymentDatabaseSettings.Value.BasketCollectionName);
        }

        public async Task<List<Payment>> GetAsync() =>
            await _paymentCollection.Find(_ => true).ToListAsync();

        public async Task<Payment?> GetAsync(string id) =>
            await _paymentCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Payment newPayment) =>
            await _paymentCollection.InsertOneAsync(newPayment);

        public async Task UpdateAsync(string id, Payment updatedPayment) =>
            await _paymentCollection.ReplaceOneAsync(x => x.Id == id, updatedPayment);

        public async Task RemoveAsync(string id) =>
            await _paymentCollection.DeleteOneAsync(x => x.Id == id);
    }
}
