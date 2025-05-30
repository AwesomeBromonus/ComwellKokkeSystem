using Modeller;
using Interface;
using MongoDB.Driver;

namespace ComwellSystemAPI.Repositories
{
    // Repository-klasse til håndtering af læringsmaterialer i MongoDB
    public class LæringRepositoryMongoDB : ILæring
    {
        private readonly IMongoCollection<Læring> _collection;

        // Konstruktor initialiserer MongoDB-samlingen til læringsmaterialer
        public LæringRepositoryMongoDB(IMongoDatabase database)
        {
            _collection = database.GetCollection<Læring>("Laering");
        }

        // Henter alle læringsmaterialer som en liste
        public async Task<List<Læring>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        // Henter et enkelt læringsmateriale baseret på dets unikke id
        public async Task<Læring?> GetByIdAsync(int id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Tilføjer et nyt læringsmateriale med automatisk genereret unikt id
        public async Task AddAsync(Læring læring)
        {
            læring.Id = await GetNextIdAsync();
            await _collection.InsertOneAsync(læring);
        }

        // Hjælpefunktion til at finde næste ledige id ved at finde den højeste eksisterende id og lægge 1 til
        private async Task<int> GetNextIdAsync()
        {
            var last = await _collection.Find(_ => true)
                .SortByDescending(x => x.Id)
                .Limit(1)
                .FirstOrDefaultAsync();

            return last == null ? 1 : last.Id + 1;
        }
    }
}
