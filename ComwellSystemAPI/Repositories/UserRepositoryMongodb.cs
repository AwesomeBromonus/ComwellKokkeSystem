using MongoDB.Driver;
using Modeller;

namespace ComwellSystemAPI.Repositories
{
    // Repository-klasse til håndtering af brugerdata i MongoDB
    public class UserRepositoryMongodb : IUserRepository
    {
        private readonly IMongoCollection<UserModel> _userCollection;

        // Konstruktor initialiserer MongoDB-samling til brugere
        public UserRepositoryMongodb(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<UserModel>("Brugere");
        }

        // Tilføjer en ny bruger med unikt genereret id
        public async Task AddAsync(UserModel user)
        {
            user.Id = await GetNextIdAsync();
            await _userCollection.InsertOneAsync(user);
        }

        // Henter alle brugere som liste
        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _userCollection.Find(_ => true).ToListAsync();
        }

        // Henter en bruger baseret på brugernavn
        public async Task<UserModel?> GetByUsernameAsync(string username)
        {
            return await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        // Henter en bruger baseret på id
        public async Task<UserModel?> GetByIdAsync(int id)
        {
            return await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        // Sletter en bruger baseret på id
        public async Task DeleteAsync(int id)
        {
            await _userCollection.DeleteOneAsync(u => u.Id == id);
        }

        // Validerer login ved at tjekke brugernavn og adgangskode
        public async Task<bool> ValidateLogin(string username, string password)
        {
            var user = await GetByUsernameAsync(username);
            return user != null && user.Password == password;
        }

        // Opdaterer brugerdata i databasen
        public async Task UpdateUserAsync(UserModel bruger)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Id, bruger.Id);

            var update = Builders<UserModel>.Update
                .Set(u => u.Username, bruger.Username)
                .Set(u => u.Password, bruger.Password)
                .Set(u => u.Role, bruger.Role)
                .Set(u => u.Email, bruger.Email)
                .Set(u => u.Navn, bruger.Navn)
                .Set(u => u.Tlf, bruger.Tlf)
                .Set(u => u.Adresse, bruger.Adresse)
                .Set(u => u.StartDato, bruger.StartDato)
                .Set(u => u.SlutDato, bruger.SlutDato)
                .Set(u => u.HotelId, bruger.HotelId)
                .Set(u => u.HotelNavn, bruger.HotelNavn)
                .Set(u => u.ElevplanId, bruger.ElevplanId);

            await _userCollection.UpdateOneAsync(filter, update);
        }

        // Tildeler elevplan til bruger ved at opdatere elevplanId
        public async Task AssignElevplanToUserAsync(int userId, int elevplanId)
        {
            var filter = Builders<UserModel>.Filter.Eq(u => u.Id, userId);
            var update = Builders<UserModel>.Update.Set(u => u.ElevplanId, elevplanId);
            await _userCollection.UpdateOneAsync(filter, update);
        }

        // Hjælpefunktion til at finde næste ledige id baseret på eksisterende brugere
        private async Task<int> GetNextIdAsync()
        {
            var allUsers = await _userCollection.Find(_ => true).ToListAsync();
            return allUsers.Count == 0 ? 1 : allUsers.Max(u => u.Id) + 1;
        }

        // Henter alle brugere med rollerne admin eller kok
        public async Task<List<UserModel>> GetAdminsOgKokkeAsync()
        {
            var filter = Builders<UserModel>.Filter.In(u => u.Role, new[] { "admin", "kok" });
            return await _userCollection.Find(filter).ToListAsync();
        }
    }
}
