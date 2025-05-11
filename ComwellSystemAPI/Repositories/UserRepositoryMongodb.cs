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

        public async Task<UserModel?> GetByUsernameAsync(string username)
        {
            return await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task AddAsync(UserModel user)
        {
            user.Id = await GetNextIdAsync();
            await _userCollection.InsertOneAsync(user);
        }

        public async Task<bool> ValidateLogin(string username, string password)
        {
            var user = await GetByUsernameAsync(username);
            return user != null && user.Password == password;
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _userCollection.Find(_ => true).ToListAsync();
        }

        public async Task<UserModel?> GetByIdAsync(int id)
        {
            return await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _userCollection.DeleteOneAsync(u => u.Id == id);
        }

        private async Task<int> GetNextIdAsync()
        {
            var sort = Builders<UserModel>.Sort.Descending(u => u.Id);
            var last = await _userCollection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
            return last == null ? 1 : last.Id + 1;
        }
    }
}
