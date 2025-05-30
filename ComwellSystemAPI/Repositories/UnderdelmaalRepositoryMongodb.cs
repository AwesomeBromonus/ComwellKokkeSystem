using ComwellSystemAPI.Interfaces;
using ComwellSystemAPI.Repositories;
using Modeller;
using MongoDB.Driver;

public class UnderdelmaalRepository : IUnderdelmaal
{
    private readonly IMongoCollection<Underdelmaal> _collection;

    // Konstruktor initialiserer MongoDB-samlingen til underdelmål
    public UnderdelmaalRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Underdelmaal>("Underdelmaal");
    }

    // Tilføjer et nyt underdelmål med automatisk genereret unikt ID
    public async Task AddAsync(Underdelmaal underdelmaal)
    {
        underdelmaal.Id = await GetNextIdAsync();
        await _collection.InsertOneAsync(underdelmaal);
    }

    // Hjælpefunktion til at finde næste ledige ID baseret på den højeste eksisterende ID i samlingen
    private async Task<int> GetNextIdAsync()
    {
        var sort = Builders<Underdelmaal>.Sort.Descending(u => u.Id);
        var last = await _collection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return last == null ? 1 : last.Id + 1;
    }

    // Opdaterer et eksisterende underdelmål fuldt ud
    public async Task UpdateAsync(Underdelmaal underdelmaal)
    {
        var filter = Builders<Underdelmaal>.Filter.Eq(u => u.Id, underdelmaal.Id);
        await _collection.ReplaceOneAsync(filter, underdelmaal);
    }

    // Henter alle underdelmål, der tilhører et specifikt delmål
    public async Task<List<Underdelmaal>> GetByDelmaalIdAsync(int delmaalId)
    {
        return await _collection.Find(u => u.DelmaalId == delmaalId).ToListAsync();
    }

    // Henter et enkelt underdelmål baseret på ID
    public async Task<Underdelmaal?> GetByIdAsync(int id)
    {
        return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    // Opdaterer kun statusfeltet for et underdelmål
    public async Task UpdateStatusAsync(int underdelmaalId, string nyStatus)
    {
        var filter = Builders<Underdelmaal>.Filter.Eq(u => u.Id, underdelmaalId);
        var update = Builders<Underdelmaal>.Update.Set(u => u.Status, nyStatus);
        await _collection.UpdateOneAsync(filter, update);
    }

    // Sletter et underdelmål baseret på ID
    public async Task DeleteAsync(int id)
    {
        var filter = Builders<Underdelmaal>.Filter.Eq(u => u.Id, id);
        await _collection.DeleteOneAsync(filter);
    }
}
