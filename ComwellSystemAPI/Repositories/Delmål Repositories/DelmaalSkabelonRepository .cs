using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;

namespace ComwellSystemAPI.Repositories
{
    public class DelmaalSkabelonRepository : IDelmaalSkabelon
    {
        private readonly IMongoCollection<DelmaalSkabelon> _collection;

        public DelmaalSkabelonRepository()
        {
            var client = new MongoClient("mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/");
            var db = client.GetDatabase("Comwell");
            _collection = db.GetCollection<DelmaalSkabelon>("DelmålSkabeloner");
        }

        public async Task<List<DelmaalSkabelon>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<List<DelmaalSkabelon>> GetByPraktikperiodeNrAsync(int praktikperiodeNummer) =>
            await _collection.Find(x => x.PraktikperiodeNummer == praktikperiodeNummer).ToListAsync();

        public async Task<DelmaalSkabelon?> GetByIdAsync(int id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(DelmaalSkabelon model)
        {
            model.Id = await GetNextIdAsync();
            await _collection.InsertOneAsync(model);
        }

        public async Task UpdateAsync(DelmaalSkabelon model)
        {
            var filter = Builders<DelmaalSkabelon>.Filter.Eq(x => x.Id, model.Id);
            await _collection.ReplaceOneAsync(filter, model);
        }

        public async Task DeleteAsync(int id)
        {
            var filter = Builders<DelmaalSkabelon>.Filter.Eq(x => x.Id, id);
            await _collection.DeleteOneAsync(filter);
        }

        private async Task<int> GetNextIdAsync()
        {
            var sort = Builders<DelmaalSkabelon>.Sort.Descending(x => x.Id);
            var last = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
            return last?.Id + 1 ?? 1;
        }
    }
}
