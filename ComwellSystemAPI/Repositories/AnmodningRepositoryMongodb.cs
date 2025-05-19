using MongoDB.Driver;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Repositories
{
    public class AnmodningRepositoryMongo : IAnmodningRepository
    {
        private readonly IMongoCollection<Anmodning> _collection;

        public AnmodningRepositoryMongo()
        {
            var client = new MongoClient("mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/");
            var database = client.GetDatabase("Comwell");
            _collection = database.GetCollection<Anmodning>("Anmodninger");
        }

        public async Task OpretAsync(Anmodning anmodning)
        {
            anmodning.Id = await GetNextIdAsync(); // Generer unikt int-ID
            anmodning.Status = "Afventer";          // Sæt default status
            await _collection.InsertOneAsync(anmodning);
        }

        public async Task<Anmodning?> GetByIdAsync(int id)
        {
            return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Anmodning>> GetTilModtagerAsync(int modtagerId)
        {
            return await _collection
                .Find(a => a.ModtagerId == modtagerId && a.Status == "Afventer")
                .ToListAsync();
        }

        public async Task UpdateAsync(Anmodning anmodning)
        {
            await _collection.ReplaceOneAsync(a => a.Id == anmodning.Id, anmodning);
        }

        public async Task<List<Anmodning>> GetAlleAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        private async Task<int> GetNextIdAsync()
        {
            var all = await _collection.Find(_ => true).ToListAsync();
            return all.Count == 0 ? 1 : all.Max(a => a.Id) + 1;
        }
        public async Task BehandlAsync(int id, bool accepteret)
        {
            var anmodning = await GetByIdAsync(id);
            if (anmodning == null) return;

            anmodning.Status = accepteret ? "Godkendt" : "Afvist";
            await UpdateAsync(anmodning);
        }

    }
}
