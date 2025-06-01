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
        // Genererer næste ledige id for praktikperiode
        periode.Id = await GetNextIdAsync();
        // Indsætter ny praktikperiode i samlingen
        await _collection.InsertOneAsync(periode);
    }

    // Opdaterer en eksisterende praktikperiode
    public async Task UpdateAsync(Praktikperiode periode)
    {
        // Finder dokument baseret på id og erstatter det med den nye data
        var filter = Builders<Praktikperiode>.Filter.Eq(p => p.Id, periode.Id);
        await _collection.ReplaceOneAsync(filter, periode);
    }

    // Sletter en praktikperiode baseret på id
    public async Task DeleteAsync(int id)
    {
        // Finder dokument baseret på id og sletter det
        var filter = Builders<Praktikperiode>.Filter.Eq(p => p.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    // Hjælpefunktion der finder næste ledige id ved at finde højeste eksisterende id og lægge 1 til
    private async Task<int> GetNextIdAsync()
    {
        // Sorterer praktikperioder efter id i faldende orden og henter den første
        var sort = Builders<Praktikperiode>.Sort.Descending(p => p.Id);
        var last = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        // Returnerer 1 hvis samlingen er tom, ellers sidste id + 1
        return last == null ? 1 : last.Id + 1;
    }

    // Henter alle praktikperioder tilknyttet en specifik elevplan
    public async Task<List<Praktikperiode>> GetByElevplanIdAsync(int elevplanId)
    {
        // Finder alle praktikperioder hvor ElevplanId matcher
        return await _collection.Find(p => p.ElevplanId == elevplanId).ToListAsync();
    }

   
    
    





}
