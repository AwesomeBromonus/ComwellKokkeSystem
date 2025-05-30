using Modeller;
using MongoDB.Driver;
using ComwellSystemAPI.Interfaces;

public class ElevplanRepository : IElevplan
{
    private readonly IMongoCollection<Elevplan> _elevplanCollection;

    // Konstruktor initialiserer MongoDB-samling for elevplaner
    public ElevplanRepository(IMongoDatabase database)
    {
        _elevplanCollection = database.GetCollection<Elevplan>("Elevplaner");
    }

    // Henter alle elevplaner som en liste
    public async Task<List<Elevplan>> GetAllAsync()
    {
        return await _elevplanCollection.Find(_ => true).ToListAsync();
    }

    // Henter en enkelt elevplan baseret på dens unikke id
    public async Task<Elevplan?> GetByIdAsync(int id)
    {
        return await _elevplanCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    // Hent En elevplan for en specifik elev
    public async Task<Elevplan?> GetByElevIdAsync(int elevId)
    {
        return await _elevplanCollection.Find(p => p.ElevId == elevId)
                                .SortByDescending(p => p.OprettetDato)
                                .Limit(1)
                                .FirstOrDefaultAsync();
    }

    // Tilføjer en ny elevplan med manuelt genereret unikt id
    public async Task AddAsync(Elevplan plan)
    {
        plan.Id = await GetNextIdAsync();
        await _elevplanCollection.InsertOneAsync(plan);
    }

    // Opdaterer en eksisterende elevplan fuldstændigt
    public async Task UpdateAsync(Elevplan plan)
    {
        var filter = Builders<Elevplan>.Filter.Eq(p => p.Id, plan.Id);
        await _elevplanCollection.ReplaceOneAsync(filter, plan);
    }

    // Sletter en elevplan baseret på id
    public async Task DeleteAsync(int id)
    {
        var filter = Builders<Elevplan>.Filter.Eq(p => p.Id, id);
        await _elevplanCollection.DeleteOneAsync(filter);
    }

    // Hjælpefunktion der finder næste ledige id ved at finde den højeste eksisterende id og lægge 1 til
    private async Task<int> GetNextIdAsync()
    {
        var sort = Builders<Elevplan>.Sort.Descending(p => p.Id);
        var lastPlan = await _elevplanCollection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return lastPlan == null ? 1 : lastPlan.Id + 1;
    }
}
