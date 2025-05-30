using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;

namespace ComwellSystemAPI.Repositories
{
    // Repository-klasse til håndtering af delmålsskabeloner i MongoDB
    public class DelmaalSkabelonRepository : IDelmaalSkabelon
    {
        private readonly IMongoCollection<DelmaalSkabelon> _collection;

        // Konstruktor initialiserer MongoDB-samlingen til delmålsskabeloner
        public DelmaalSkabelonRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<DelmaalSkabelon>("DelmålSkabeloner");
        }

        // Henter alle delmålsskabeloner som en liste
        public async Task<List<DelmaalSkabelon>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        // Henter delmålsskabeloner der matcher et specifikt praktikperiode-nummer
        public async Task<List<DelmaalSkabelon>> GetByPraktikperiodeNrAsync(int praktikperiodeNummer) =>
            await _collection.Find(x => x.PraktikperiodeNummer == praktikperiodeNummer).ToListAsync();

        // Henter en enkelt delmålsskabelon baseret på id
        public async Task<DelmaalSkabelon?> GetByIdAsync(int id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Tilføjer en ny delmålsskabelon og genererer automatisk et unikt id
        public async Task AddAsync(DelmaalSkabelon model)
        {
            model.Id = await GetNextIdAsync();
            await _collection.InsertOneAsync(model);
        }

        // Opdaterer en eksisterende delmålsskabelon baseret på id
        public async Task UpdateAsync(DelmaalSkabelon model)
        {
            var filter = Builders<DelmaalSkabelon>.Filter.Eq(x => x.Id, model.Id);
            await _collection.ReplaceOneAsync(filter, model);
        }

        // Sletter en delmålsskabelon baseret på id
        public async Task DeleteAsync(int id)
        {
            var filter = Builders<DelmaalSkabelon>.Filter.Eq(x => x.Id, id);
            await _collection.DeleteOneAsync(filter);
        }

        // Hjælpefunktion der finder næste ledige id ved at finde højeste eksisterende id og lægge 1 til
        private async Task<int> GetNextIdAsync()
        {
            var sort = Builders<DelmaalSkabelon>.Sort.Descending(x => x.Id);
            var last = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
            return last?.Id + 1 ?? 1;
        }
    }
}
