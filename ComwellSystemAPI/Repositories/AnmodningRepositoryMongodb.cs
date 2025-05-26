using MongoDB.Driver;
using Modeller;
using ComwellSystemAPI.Interfaces;
using ComwellSystemAPI.Repositories;

namespace ComwellSystemAPI.Repositories
{
    public class AnmodningRepositoryMongo : IAnmodningRepository
    {
        private readonly IMongoCollection<Anmodning> _collection;


        public AnmodningRepositoryMongo()
        {
            var client = new MongoClient("mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/");
            var database = client.GetDatabase("Comwell");
            _collection = database.GetCollection<Anmodning>("Anmodninger");
        }

        public async Task OpretAsync(Anmodning anmodning)
        {
            anmodning.Id = await GetNextIdAsync(); // Generer unikt int-ID
            anmodning.Status = "Afventer";          // Sæt default status
            await _collection.InsertOneAsync(anmodning);
        }

        public async Task<Anmodning?> GetByIdAsync(int id)
        {
            return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Anmodning>> GetTilModtagerAsync(int modtagerId)
        {
            return await _collection
                .Find(a => a.ModtagerId == modtagerId && a.Status == "Afventer")
                .ToListAsync();
        }

        public async Task UpdateAsync(Anmodning anmodning)
        {
            await _collection.ReplaceOneAsync(a => a.Id == anmodning.Id, anmodning);
        }

        public async Task<List<Anmodning>> GetAlleAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        //En metode som finder den største id af id´erne i databasen og plusser den anmodning man tilføjer med 1
        private async Task<int> GetNextIdAsync()
        {
            var all = await _collection.Find(_ => true).ToListAsync();
            return all.Count == 0 ? 1 : all.Max(a => a.Id) + 1;
        }

        // Metode som behandler anmodningen for under- og delmål 
        public async Task BehandlAsync(int id, bool accepteret)
        {
            var anmodning = await GetByIdAsync(id);
            if (anmodning == null) return;

            var db = _collection.Database;

            if (accepteret)
            {
                if (anmodning.UnderdelmaalId.HasValue)
                {
                    var underdelmaalCollection = db.GetCollection<Underdelmaal>("Underdelmaal");
                    var filter = Builders<Underdelmaal>.Filter.Eq(u => u.Id, anmodning.UnderdelmaalId.Value);
                    var underdelmaal = await underdelmaalCollection.Find(filter).FirstOrDefaultAsync();

                    if (underdelmaal != null)
                    {
                        underdelmaal.Status = anmodning.ØnsketStatus;
                        await underdelmaalCollection.ReplaceOneAsync(filter, underdelmaal);
                    }
                }
                else if (anmodning.DelmaalId != 0)
                {
                    var delmaalCollection = db.GetCollection<Delmål>("Delmål");
                    var filter = Builders<Delmål>.Filter.Eq(d => d.Id, anmodning.DelmaalId);
                    var delmaal = await delmaalCollection.Find(filter).FirstOrDefaultAsync();

                    if (delmaal != null)
                    {
                        delmaal.Status = anmodning.ØnsketStatus;
                        await delmaalCollection.ReplaceOneAsync(filter, delmaal);
                    }
                }

                anmodning.Status = "Godkendt";
            }
            else
            {
                anmodning.Status = "Afvist";
            }

            await UpdateAsync(anmodning);
        }



    }
}
