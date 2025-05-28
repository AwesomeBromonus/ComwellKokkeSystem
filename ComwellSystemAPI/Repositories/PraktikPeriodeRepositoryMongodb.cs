namespace ComwellSystemAPI.Repositories;
using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Bson;
using MongoDB.Driver;

public class PraktikperiodeRepository : IPraktikperiode
{
    private readonly IMongoCollection<Praktikperiode> _collection;

    public PraktikperiodeRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Praktikperiode>("Praktikperioder");
    }

    public async Task<List<Praktikperiode>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<Praktikperiode?> GetByIdAsync(int id) =>
        await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task AddAsync(Praktikperiode periode)
    {
        periode.Id = await GetNextIdAsync();
        await _collection.InsertOneAsync(periode);
    }

    public async Task UpdateAsync(Praktikperiode periode)
    {
        var filter = Builders<Praktikperiode>.Filter.Eq(p => p.Id, periode.Id);
        await _collection.ReplaceOneAsync(filter, periode);
    }

    public async Task DeleteAsync(int id)
    {
        var filter = Builders<Praktikperiode>.Filter.Eq(p => p.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    private async Task<int> GetNextIdAsync()
    {
        var sort = Builders<Praktikperiode>.Sort.Descending(p => p.Id);
        var last = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return last == null ? 1 : last.Id + 1;
    }

    public async Task<List<Praktikperiode>> GetByElevplanIdAsync(int elevplanId)
    {
        return await _collection.Find(p => p.ElevplanId == elevplanId).ToListAsync();
    }
    public async Task<List<Praktikperiode>> GetByElevIdAsync(int elevId)
    {
        var filter = Builders<Praktikperiode>.Filter.Eq(p => p.ElevId, elevId);
        return await _collection.Find(filter).ToListAsync();
    }
    
    





}
