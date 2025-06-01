using MongoDB.Driver;
using Modeller;
using ComwellSystemAPI.Interfaces;
namespace ComwellSystemAPI.Repositories
{
  

    public class UnderdelmaalSkabelonRepository : IUnderdelmaalSkabelon
    {
        private readonly IMongoCollection<UnderdelmaalSkabelon> _collection;

        public UnderdelmaalSkabelonRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<UnderdelmaalSkabelon>("UnderdelmaalSkabelon");
        }

        public async Task<List<UnderdelmaalSkabelon>> GetByDelmaalSkabelonIdAsync(int delmaalSkabelonId)
        {
            return await _collection.Find(u => u.DelmaalSkabelonId == delmaalSkabelonId).ToListAsync();
        }

        public async Task AddAsync(UnderdelmaalSkabelon item)
        {
            item.Id = await GetNextIdAsync();
            await _collection.InsertOneAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _collection.DeleteOneAsync(u => u.Id == id);
        }

        private async Task<int> GetNextIdAsync()
        {
            var last = await _collection.Find(_ => true)
                .SortByDescending(x => x.Id)
                .Limit(1)
                .FirstOrDefaultAsync();
            return last == null ? 1 : last.Id + 1;
        }
        public async Task UpdateAsync(UnderdelmaalSkabelon model)
        {
            var filter = Builders<UnderdelmaalSkabelon>.Filter.Eq(x => x.Id, model.Id);
            await _collection.ReplaceOneAsync(filter, model);
        }

    }

}
