namespace ComwellSystemAPI.Repositories;
using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;

public class PraktikperiodeRepository : IPraktikperiode
{
    private readonly IMongoCollection<Praktikperiode> _collection;

    // Konstruktor initialiserer MongoDB-samling for praktikperioder
    public PraktikperiodeRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Praktikperiode>("Praktikperioder");
    }

    // Henter alle praktikperioder som en liste
    public async Task<List<Praktikperiode>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    // Henter en enkelt praktikperiode baseret på dens unikke id
    public async Task<Praktikperiode?> GetByIdAsync(int id) =>
        await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

    // Tilføjer en ny praktikperiode med unikt genereret id
    public async Task AddAsync(Praktikperiode periode)
    {
        periode.Id = await GetNextIdAsync();
        await _collection.InsertOneAsync(periode);
    }

    // Opdaterer en eksisterende praktikperiode
    public async Task UpdateAsync(Praktikperiode periode)
    {
        var filter = Builders<Praktikperiode>.Filter.Eq(p => p.Id, periode.Id);
        await _collection.ReplaceOneAsync(filter, periode);
    }

    // Sletter en praktikperiode baseret på id
    public async Task DeleteAsync(int id)
    {
        var filter = Builders<Praktikperiode>.Filter.Eq(p => p.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    // Hjælpefunktion der finder næste ledige id ved at finde højeste eksisterende id og lægge 1 til
    private async Task<int> GetNextIdAsync()
    {
        var sort = Builders<Praktikperiode>.Sort.Descending(p => p.Id);
        var last = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return last == null ? 1 : last.Id + 1;
    }

    // Henter alle praktikperioder tilknyttet en specifik elevplan
    public async Task<List<Praktikperiode>> GetByElevplanIdAsync(int elevplanId)
    {
        return await _collection.Find(p => p.ElevplanId == elevplanId).ToListAsync();
    }

    // Henter alle praktikperioder tilknyttet en specifik elev
    public async Task<List<Praktikperiode>> GetByElevIdAsync(int elevId)
    {
        var filter = Builders<Praktikperiode>.Filter.Eq(p => p.ElevId, elevId);
        return await _collection.Find(filter).ToListAsync();
    }
}
