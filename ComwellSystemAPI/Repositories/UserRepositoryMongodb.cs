using MongoDB.Driver;
using Modeller;

namespace ComwellSystemAPI.Repositories
{
    public class UserRepositoryMongodb : IUserRepository
    {
        private readonly IMongoCollection<Bruger> _userCollection;

        public UserRepositoryMongodb()
        {
            var mongoUri = "mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/";
            var client = new MongoClient(mongoUri);
            var database = client.GetDatabase("Comwell");
            _userCollection = database.GetCollection<Bruger>("Brugere");
        }

        public async Task<Bruger?> GetByUsernameAsync(string username)
        {
            return await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Bruger bruger)
        {
            bruger.Id = await GetNextIdAsync();
            await _userCollection.InsertOneAsync(bruger);
        }

        public async Task<bool> ValidateLogin(string username, string password)
        {
            var bruger = await GetByUsernameAsync(username);
            return bruger != null && bruger.Password == password;
        }

        private async Task<int> GetNextIdAsync()
        {
            var sort = Builders<Bruger>.Sort.Descending(u => u.Id);
            var last = await _userCollection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
            return last == null ? 1 : last.Id + 1;
        }

        public async Task<List<Bruger>> GetAllAsync()
        {
            return await _userCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Bruger?> GetByIdAsync(int id)
        {
            return await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _userCollection.DeleteOneAsync(u => u.Id == id);
        }
    }
}
