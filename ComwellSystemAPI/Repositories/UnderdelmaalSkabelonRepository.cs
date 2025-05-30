using MongoDB.Driver;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Repositories
{
    // Repository-klasse til håndtering af underdelmålsskabeloner i MongoDB
    public class UnderdelmaalSkabelonRepository : IUnderdelmaalSkabelon
    {
        private readonly IMongoCollection<UnderdelmaalSkabelon> _collection;

        // Konstruktor initialiserer MongoDB-samlingen til underdelmålsskabeloner
        public UnderdelmaalSkabelonRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<UnderdelmaalSkabelon>("UnderdelmaalSkabelon");
        }

        // Henter alle underdelmålsskabeloner tilknyttet et bestemt delmålsskabelon-id
        public async Task<List<UnderdelmaalSkabelon>> GetByDelmaalSkabelonIdAsync(int delmaalSkabelonId)
        {
            return await _collection.Find(u => u.DelmaalSkabelonId == delmaalSkabelonId).ToListAsync();
        }

        // Tilføjer en ny underdelmålsskabelon med automatisk genereret unikt ID
        public async Task AddAsync(UnderdelmaalSkabelon item)
        {
            item.Id = await GetNextIdAsync();
            await _collection.InsertOneAsync(item);
        }

        // Sletter en underdelmålsskabelon baseret på ID
        public async Task DeleteAsync(int id)
        {
            await _collection.DeleteOneAsync(u => u.Id == id);
        }

        // Opdaterer en eksisterende underdelmålsskabelon
        public async Task UpdateAsync(UnderdelmaalSkabelon model)
        {
            var filter = Builders<UnderdelmaalSkabelon>.Filter.Eq(x => x.Id, model.Id);
            await _collection.ReplaceOneAsync(filter, model);
        }

        // Hjælpefunktion til at finde næste ledige ID ved at finde højeste eksisterende ID og lægge 1 til
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
