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

    public async Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId)
    {
        return await _collection.Find(d => d.PraktikperiodeId == praktikperiodeId).ToListAsync();
    }
    public async Task UpdateDelmaalAsync(Delmål delmaal)
    {
        var filter = Builders<Delmål>.Filter.Eq(d => d.Id, delmaal.Id);
        await _collection.ReplaceOneAsync(filter, delmaal);
    }
    public async Task<List<Modeller.Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId)
    {
        var filter = Builders<Modeller.Delmål>.Filter.And(
            Builders<Modeller.Delmål>.Filter.Eq(d => d.ElevplanId, elevplanId),
            Builders<Modeller.Delmål>.Filter.Eq(d => d.PraktikperiodeId, praktikperiodeId)
        );

        return await _collection.Find(filter).ToListAsync();
    }


}
