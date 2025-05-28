using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;

namespace ComwellSystemAPI.Repositories
{
    public class KommentarRepository : IKommentar
    {
        private readonly IMongoCollection<Kommentar> _collection;

        public KommentarRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Kommentar>("Kommentarer");
        }
        // Tilføjer en ny kommentar til databasen
        public async Task AddAsync(Kommentar kommentar)
        {
            kommentar.Id = await GetNextIdAsync(); // automatisk ID-generering
            await _collection.InsertOneAsync(kommentar);
        }

        // Henter alle kommentarer knyttet til et bestemt delmål
        public async Task<List<Kommentar>> GetByDelmålIdAsync(int delmålId)
        {
            return await _collection.Find(k => k.DelmålId == delmålId).ToListAsync();
        }

        // Hjælper med at finde næste ID (auto-increment)
        private async Task<int> GetNextIdAsync()
        {
            var sort = Builders<Kommentar>.Sort.Descending(k => k.Id);
            var sidste = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
            return sidste == null ? 1 : sidste.Id + 1;
        }
    }
}
