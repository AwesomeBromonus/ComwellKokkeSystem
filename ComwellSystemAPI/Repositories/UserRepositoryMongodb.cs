using MongoDB.Driver;
using Modeller;

namespace ComwellSystemAPI.Repositories
{
    public class UserRepositoryMongodb : IUserRepository
    {
        private readonly IMongoCollection<UserModel> _userCollection;

        public UserRepositoryMongodb()
        {
            var mongoUri = "mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/";
            var client = new MongoClient(mongoUri);
            var database = client.GetDatabase("Comwell");
            _userCollection = database.GetCollection<UserModel>("Brugere");
        }

        public async Task AddAsync(UserModel user)
        {
            user.Id = await GetNextIdAsync();
            await _userCollection.InsertOneAsync(user);
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _userCollection.Find(_ => true).ToListAsync();
        }

        public async Task<UserModel?> GetByUsernameAsync(string username)
        {
            return await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<UserModel?> GetByIdAsync(int id)
        {
            return await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _userCollection.DeleteOneAsync(u => u.Id == id);
        }

        public async Task<bool> ValidateLogin(string username, string password)
        {
            var user = await GetByUsernameAsync(username);
            return user != null && user.Password == password;
        }

        public async Task UpdateUserAsync(UserModel bruger)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Id, bruger.Id);

            var update = Builders<UserModel>.Update
                .Set(u => u.StartDato, bruger.StartDato)
                .Set(u => u.SlutDato, bruger.SlutDato)
                .Set(u => u.ElevplanId, bruger.ElevplanId);

            await _userCollection.UpdateOneAsync(filter, update);
        }

        public async Task AssignElevplanToUserAsync(int userId, int elevplanId)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Id, userId);
            var update = Builders<UserModel>.Update.Set(u => u.ElevplanId, elevplanId);
            await _userCollection.UpdateOneAsync(filter, update);
        }

        private async Task<int> GetNextIdAsync()
        {
            var allUsers = await _userCollection.Find(_ => true).ToListAsync();
            return allUsers.Count == 0 ? 1 : allUsers.Max(u => u.Id) + 1;
        }
        public async Task<List<UserModel>> GetAdminsOgKokkeAsync()
        {
            var filter = Builders<UserModel>.Filter.In(u => u.Role, new[] { "admin", "kok" });
            return await _userCollection.Find(filter).ToListAsync();
        }

    }
}
