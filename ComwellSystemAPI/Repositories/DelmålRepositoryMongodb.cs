using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;

public class DelmålRepository : IDelmål
{
    private readonly IMongoCollection<Delmål> _collection;
    private readonly IMongoCollection<Underdelmaal> _underdelmaalCollection;
    private readonly IMongoCollection<UnderdelmaalSkabelon> _underdelSkabelonCollection;

    // Konstruktor initialiserer MongoDB-samlinger for delmål, underdelmål og skabeloner
    public DelmålRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Delmål>("Delmål");
        _underdelmaalCollection = database.GetCollection<Underdelmaal>("Underdelmaal");
        _underdelSkabelonCollection = database.GetCollection<UnderdelmaalSkabelon>("UnderdelmaalSkabelon");
    }

    // Tilføjer et nyt delmål med automatisk genereret ID
    // Opretter tilhørende underdelmål baseret på skabeloner knyttet til delmålet
    public async Task AddAsync(Delmål delmaal)
    {
        delmaal.Id = await GetNextIdAsync();
        await _collection.InsertOneAsync(delmaal);

        if (delmaal.DelmaalSkabelonId == null)
        {
            Console.WriteLine("⚠️ DelmaalSkabelonId er null. Underdelmål bliver ikke oprettet.");
            return;
        }

        // Henter underdelmålsskabeloner tilknyttet delmålsskabelonen
        var underSkabeloner = await _underdelSkabelonCollection
            .Find(u => u.DelmaalSkabelonId == delmaal.DelmaalSkabelonId)
            .ToListAsync();

        if (!underSkabeloner.Any())
        {
            Console.WriteLine($"ℹ️ Ingen underdelmålsskabeloner fundet for skabelon ID {delmaal.DelmaalSkabelonId}");
            return;
        }

        // Finder højeste nuværende ID for underdelmål for at sætte nye unikke ID'er
        var sidste = await _underdelmaalCollection.Find(_ => true)
            .SortByDescending(x => x.Id)
            .Limit(1)
            .FirstOrDefaultAsync();

        int næsteId = sidste?.Id + 1 ?? 1;

        // Opretter nye underdelmål ud fra skabelonerne med status "Ikke fuldført"
        var underdelmaal = underSkabeloner.Select(s => new Underdelmaal
        {
            Id = næsteId++,
            DelmaalId = delmaal.Id,
            Beskrivelse = s.Beskrivelse,
            Deadline = s.Deadline,
            Status = "Ikke fuldført"
        }).ToList();

        await _underdelmaalCollection.InsertManyAsync(underdelmaal);
        Console.WriteLine($"✅ {underdelmaal.Count} underdelmål oprettet for delmål ID {delmaal.Id}");
    }

    // Hjælpefunktion der finder det næste ledige unikke ID for delmål
    private async Task<int> GetNextIdAsync()
    {
        var sort = Builders<Delmål>.Sort.Descending(d => d.Id);
        var last = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return last == null ? 1 : last.Id + 1;
    }

    // Opdaterer et delmål helt
    public async Task UpdateDelmaalAsync(Delmål delmaal)
    {
        var filter = Builders<Delmål>.Filter.Eq(d => d.Id, delmaal.Id);
        await _collection.ReplaceOneAsync(filter, delmaal);
    }

    // Henter delmål baseret på praktikperiode-id
    public async Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId)
    {
        return await _collection.Find(d => d.PraktikperiodeId == praktikperiodeId).ToListAsync();
    }

   

    // Henter et enkelt delmål baseret på id
    public async Task<Delmål?> GetByIdAsync(int id)
    {
        return await _collection.Find(d => d.Id == id).FirstOrDefaultAsync();
    }

    // Opdaterer kun statusfeltet for et delmål
    public async Task UpdateStatusAsync(int delmaalId, string nyStatus)
    {
        var filter = Builders<Delmål>.Filter.Eq(d => d.Id, delmaalId);
        var update = Builders<Delmål>.Update.Set(d => d.Status, nyStatus);
        await _collection.UpdateOneAsync(filter, update);
    }

    // Sletter et delmål baseret på id
    public async Task DeleteDelmaalAsync(int id)
    {
        var filter = Builders<Delmål>.Filter.Eq(d => d.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    // Henter alle delmål
    public async Task<List<Delmål>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    // Henter delmål med deadline inden for et bestemt antal dage fra nu
    public async Task<List<Delmål>> GetWithDeadlineWithinDaysAsync(int antalDage)
    {
        var nu = DateTime.Now;
        var grænse = nu.AddDays(antalDage);

        var filter = Builders<Delmål>.Filter.And(
            Builders<Delmål>.Filter.Gte(d => d.Deadline, nu),
            Builders<Delmål>.Filter.Lte(d => d.Deadline, grænse)
        );

        return await _collection.Find(filter).ToListAsync();
    }
}
