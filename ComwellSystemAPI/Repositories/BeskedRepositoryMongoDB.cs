using Modeller;
using MongoDB.Driver;

public class BeskedRepositoryMongoDB : IBesked
{
    private readonly IMongoCollection<Besked> _beskedCollection;

    public BeskedRepositoryMongoDB()
    {
        var mongoUri = "mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/";
        var client = new MongoClient(mongoUri);
        var database = client.GetDatabase("Comwell");
        _beskedCollection = database.GetCollection<Besked>("Beskeder");
    }

    // Hent alle beskeder
    public async Task<List<Besked>> GetAllAsync()
    {
        return await _beskedCollection.Find(_ => true).ToListAsync();
    }

    // Hent en besked ud fra int-ID
    public async Task<Besked?> GetByIdAsync(int id)
    {
        return await _beskedCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    // Opret ny besked med manuelt tildelt ID (næste ledige heltal)
    public async Task AddAsync(Besked besked)
    {
        besked.Id = await GetNextIdAsync();
        await _beskedCollection.InsertOneAsync(besked);
    }

    // Opdater eksisterende besked
    public async Task UpdateAsync(Besked besked)
    {
        var filter = Builders<Besked>.Filter.Eq(p => p.Id, besked.Id);
        await _beskedCollection.ReplaceOneAsync(filter, besked);
    }

    // Slet besked ud fra ID
    public async Task DeleteAsync(int id)
    {
        var filter = Builders<Besked>.Filter.Eq(p => p.Id, id);
        await _beskedCollection.DeleteOneAsync(filter);
    }

    // Hent næste ledige ID ved at finde max eksisterende ID og lægge 1 til
    private async Task<int> GetNextIdAsync()
    {
        var sort = Builders<Besked>.Sort.Descending(p => p.Id);
        var lastMessage = await _beskedCollection.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return lastMessage == null ? 1 : lastMessage.Id + 1;
    }

    // Henter beskeder baseret på brugerens ID (afsender eller modtager)
    public async Task<List<Besked>> GetByUserIdAsync(int userId)
    {
        var filter = Builders<Besked>.Filter.Or(
            Builders<Besked>.Filter.Eq(b => b.AfsenderId, userId),
            Builders<Besked>.Filter.Eq(b => b.ModtagerId, userId)
        );
        return await _beskedCollection.Find(filter).ToListAsync();
    }
}

