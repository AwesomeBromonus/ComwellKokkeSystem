using Modeller;
using MongoDB.Driver;
using ComwellSystemAPI.Interfaces;

public class ElevplanRepository : IElevplan
{
    private readonly IMongoCollection<Elevplan> _elevplanCollection;

    public ElevplanRepository(IMongoDatabase database)
    {
        _elevplanCollection = database.GetCollection<Elevplan>("Elevplaner");
    }
    // Hent alle elevplaner
    public async Task<List<Elevplan>> GetAllAsync()
    {
        return await _elevplanCollection.Find(_ => true).ToListAsync();
    }


    // Hent én plan ud fra int-ID
    public async Task<Elevplan?> GetByIdAsync(int id)
    {
        return await _elevplanCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

    }

    // Hent alle elevplaner for en specifik elev
    public async Task<List<Elevplan>> GetByElevIdAsync(int elevId)
    {
        return await _elevplanCollection.Find(p => p.ElevId == elevId).ToListAsync();
    }


    // Opret ny plan med manuelt tildelt ID (næste ledige heltal)
    public async Task AddAsync(Elevplan plan)
    {
        plan.Id = await GetNextIdAsync();
        await _elevplanCollection.InsertOneAsync(plan);
    }

    // Opdater eksisterende plan
    public async Task UpdateAsync(Elevplan plan)
    {
        var filter = Builders<Elevplan>.Filter.Eq(p => p.Id, plan.Id);
        await _elevplanCollection.ReplaceOneAsync(filter, plan);
    }

    // Slet plan ud fra ID
    public async Task DeleteAsync(int id)
    {
        var filter = Builders<Elevplan>.Filter.Eq(p => p.Id, id);
        await _elevplanCollection.DeleteOneAsync(filter);
    }

    // Hent næste ledige ID ved at finde max eksisterende ID og lægge 1 til
    private async Task<int> GetNextIdAsync()
    {
        var sort = Builders<Elevplan>.Sort.Descending(p => p.Id);
        var lastPlan = await _elevplanCollection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return lastPlan == null ? 1 : lastPlan.Id + 1;
    }

   
}
