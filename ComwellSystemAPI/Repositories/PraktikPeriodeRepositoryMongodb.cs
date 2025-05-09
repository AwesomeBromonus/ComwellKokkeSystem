namespace ComwellSystemAPI.Repositories;
using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Bson;
using MongoDB.Driver;

public class PraktikperiodeRepository : IPraktikperiode
{
    private readonly IMongoCollection<Praktikperiode> _collection;

    public PraktikperiodeRepository()
    {
        var mongoUri = "mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/"; // your real connection string
        var client = new MongoClient(mongoUri);
        var db = client.GetDatabase("Comwell");
        _collection = db.GetCollection<Praktikperiode>("Praktikperioder");
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
    public async Task UpdateDelmålAsync(int praktikPeriodeId, int delmålId, string status)
    {
        var filter = Builders<Praktikperiode>.Filter.Eq(p => p.Id, praktikPeriodeId);

        var update = Builders<Praktikperiode>.Update
            .Set("Delmål.$[d].Status", status);

        var arrayFilter = new List<ArrayFilterDefinition>
    {
        new BsonDocumentArrayFilterDefinition<BsonDocument>(
            new BsonDocument("d._id", delmålId))
    };

        var options = new UpdateOptions { ArrayFilters = arrayFilter };

        var result = await _collection.UpdateOneAsync(filter, update, options);

        if (result.ModifiedCount == 0)
        {
            throw new Exception("Delmålet blev ikke opdateret. Tjek om ID'er matcher.");
        }
    }





}
