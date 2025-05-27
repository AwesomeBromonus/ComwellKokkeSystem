using ComwellSystemAPI.Interfaces;
using ComwellSystemAPI.Repositories;
using ComwellSystemAPI.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using Modeller;
using MongoDB.Driver;

namespace ComwellSystemAPI.Repositories
{
    public class UnderdelmaalRepository : IUnderdelmaal
    {
        private readonly IMongoCollection<Underdelmaal> _collection;

        public UnderdelmaalRepository()
        {
            var client = new MongoClient("mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/");
            var db = client.GetDatabase("Comwell");
            _collection = db.GetCollection<Underdelmaal>("Underdelmaal");
        }

        public async Task AddAsync(Underdelmaal underdelmaal)
        {
            underdelmaal.Id = await GetNextIdAsync();
            await _collection.InsertOneAsync(underdelmaal);
        }

        private async Task<int> GetNextIdAsync()
        {
            var sort = Builders<Underdelmaal>.Sort.Descending(u => u.Id);
            var last = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
            return last == null ? 1 : last.Id + 1;
        }

        public async Task UpdateAsync(Underdelmaal underdelmaal)
        {
            var filter = Builders<Underdelmaal>.Filter.Eq(u => u.Id, underdelmaal.Id);
            await _collection.ReplaceOneAsync(filter, underdelmaal);
        }

        public async Task<List<Underdelmaal>> GetByDelmaalIdAsync(int delmaalId)
        {
            return await _collection.Find(u => u.DelmaalId == delmaalId).ToListAsync();
        }

        public async Task<Underdelmaal?> GetByIdAsync(int id)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateStatusAsync(int underdelmaalId, string nyStatus)
        {
            var filter = Builders<Underdelmaal>.Filter.Eq(u => u.Id, underdelmaalId);
            var update = Builders<Underdelmaal>.Update.Set(u => u.Status, nyStatus);
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(int id)
        {
            var filter = Builders<Underdelmaal>.Filter.Eq(u => u.Id, id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}
