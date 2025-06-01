using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;

namespace ComwellSystemAPI.Repositories
{
    // Repository-klasse til håndtering af kommentarer i MongoDB
    public class KommentarRepository : IKommentar
    {
        private readonly IMongoCollection<Kommentar> _collection;

        // Konstruktor initialiserer MongoDB-samlingen til kommentarer
        public KommentarRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Kommentar>("Kommentarer");
        }

        // Tilføjer en ny kommentar med automatisk genereret unikt ID
        public async Task AddAsync(Kommentar kommentar)
        {
            kommentar.Id = await GetNextIdAsync(); // Genererer næste unikke ID
            await _collection.InsertOneAsync(kommentar);
        }

        // Henter alle kommentarer knyttet til et specifikt delmål
        public async Task<List<Kommentar>> GetByDelmålIdAsync(int delmålId)
        {
            return await _collection.Find(k => k.DelmålId == delmålId).ToListAsync();
        }

        // Hjælpefunktion der finder næste ledige ID ved at finde højeste eksisterende ID og lægge 1 til
        private async Task<int> GetNextIdAsync()
        {
            var sort = Builders<Kommentar>.Sort.Descending(k => k.Id);
            var sidste = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
            return sidste == null ? 1 : sidste.Id + 1;
        }
    }
}
