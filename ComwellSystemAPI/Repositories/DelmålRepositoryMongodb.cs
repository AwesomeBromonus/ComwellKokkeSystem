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
        delmaal.Id = await GetNextIdAsync();
        await _collection.InsertOneAsync(delmaal);

        var db = _collection.Database;
        var underdelSkabelonCollection = db.GetCollection<UnderdelmaalSkabelon>("UnderdelmaalSkabelon");
        var underdelCollection = db.GetCollection<Underdelmaal>("Underdelmaal");

        if (delmaal.DelmaalSkabelonId == null)
        {
            Console.WriteLine("⚠️ DelmaalSkabelonId er null. Underdelmål bliver ikke oprettet.");
            return;
        }

        var underSkabeloner = await underdelSkabelonCollection
            .Find(u => u.DelmaalSkabelonId == delmaal.DelmaalSkabelonId)
            .ToListAsync();

        if (!underSkabeloner.Any())
        {
            Console.WriteLine($"ℹ️ Ingen underdelmålsskabeloner fundet for skabelon ID {delmaal.DelmaalSkabelonId}");
            return;
        }

        var sidste = await underdelCollection.Find(_ => true)
            .SortByDescending(x => x.Id)
            .Limit(1)
            .FirstOrDefaultAsync();

        int næsteId = sidste?.Id + 1 ?? 1;

        var underdelmaal = underSkabeloner.Select(s => new Underdelmaal
        {
            Id = næsteId++,
            DelmaalId = delmaal.Id,
            Beskrivelse = s.Beskrivelse,
            DeadlineOffsetDage = s.DeadlineOffsetDage,
            Status = "Ikke fuldført"
        }).ToList();

        await underdelCollection.InsertManyAsync(underdelmaal);
        Console.WriteLine($"✅ {underdelmaal.Count} underdelmål oprettet for delmål ID {delmaal.Id}");
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

    public async Task UpdateStatusAsync(int delmaalId, string nyStatus)
    {
        var filter = Builders<Delmål>.Filter.Eq(d => d.Id, delmaalId);
        var update = Builders<Delmål>.Update.Set(d => d.Status, nyStatus);
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task DeleteDelmaalAsync(int id)
    {
        var filter = Builders<Delmål>.Filter.Eq(d => d.Id, id);
        await _collection.DeleteOneAsync(filter);
    }
}
