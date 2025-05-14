using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;

public class DelmålRepository : IDelmål
{
    private readonly IMongoCollection<Delmål> _collection;

    public DelmålRepository()
    {
        var client = new MongoClient("mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/");
        var db = client.GetDatabase("Comwell");
        _collection = db.GetCollection<Delmål>("Delmål");
    }

    public async Task AddAsync(Delmål delmaal)
    {
        delmaal.Id = await GetNextIdAsync(); // ← Tilføj dette!
        await _collection.InsertOneAsync(delmaal);
    }
    private async Task<int> GetNextIdAsync()
    {
        var sort = Builders<Delmål>.Sort.Descending(d => d.Id);
        var last = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return last == null ? 1 : last.Id + 1;
    }

    public async Task UpdateDelmaalAsync(Delmål delmaal)
    {
        var filter = Builders<Delmål>.Filter.Eq(d => d.Id, delmaal.Id);
        await _collection.ReplaceOneAsync(filter, delmaal);
    }

    public async Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId)
    {
        return await _collection.Find(d => d.PraktikperiodeId == praktikperiodeId).ToListAsync();
    }

    public async Task<List<Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId)
    {
        return await _collection.Find(d => d.ElevplanId == elevplanId && d.PraktikperiodeId == praktikperiodeId).ToListAsync();
    }

    public async Task<List<Delmål>> GetByElevIdAsync(int elevId)
    {
        return await _collection.Find(d => d.ElevId == elevId).ToListAsync();
    }

    public async Task<Delmål?> GetByIdAsync(int id)
    {
        return await _collection.Find(d => d.Id == id).FirstOrDefaultAsync();
    }
    public async Task UpdateStatusAsync(int delmålId, string nyStatus)
    {
        var filter = Builders<Delmål>.Filter.Eq(d => d.Id, delmålId);
        var update = Builders<Delmål>.Update.Set(d => d.Status, nyStatus);
        await _collection.UpdateOneAsync(filter, update);
    }

}
