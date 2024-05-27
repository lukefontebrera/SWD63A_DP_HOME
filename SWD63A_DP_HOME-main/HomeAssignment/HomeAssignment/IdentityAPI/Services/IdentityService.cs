using MongoDB.Driver;
using IdentityAPI.Model;
using Microsoft.Extensions.Options;

namespace IdentityAPI.Services
{
	public class IdentityService
	{
		private readonly IMongoCollection<User> _userCollection;

		public IdentityService(
			IOptions<DatabaseSettings> databaseSettings)
		{
			var mongoClient = new MongoClient(
				databaseSettings.Value.ConnectionString);

			var mongoDatabase = mongoClient.GetDatabase(
				databaseSettings.Value.DatabaseName);

			_userCollection = mongoDatabase.GetCollection<User>(
				databaseSettings.Value.UserCollectionName);
		}

		public async Task<User?> GetAsync(string id) =>
			await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task<User?> GetByEmailAsync(string email) =>
			await _userCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

		public async Task CreateAsync(User user) =>
			await _userCollection.InsertOneAsync(user);

		public async Task RemoveAsync(string id) =>
			await _userCollection.DeleteOneAsync( x => x.Id == id);
	}
}
