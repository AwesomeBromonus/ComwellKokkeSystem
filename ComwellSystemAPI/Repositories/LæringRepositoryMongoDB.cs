using Modeller;
using Interface;
using MongoDB.Driver;

namespace ComwellSystemAPI.Repositories
{
    public class LæringRepositoryMongoDB : ILæring
    {
        private readonly IMongoCollection<Læring> _collection;

        public LæringRepositoryMongoDB(IMongoDatabase database)
        {
            _collection = database.GetCollection<Læring>("Laering");
        }
        public async Task<List<Læring>> GetAllAsync() => await _collection.Find(_ => true).ToListAsync();

        public async Task<Læring?> GetByIdAsync(int id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Læring læring)
        {
            læring.Id = await GetNextIdAsync();
            await _collection.InsertOneAsync(læring);
        }

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
