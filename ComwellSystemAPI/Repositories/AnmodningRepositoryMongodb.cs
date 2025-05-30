using MongoDB.Driver;
using Modeller;
using ComwellSystemAPI.Interfaces;
using ComwellSystemAPI.Repositories;

namespace ComwellSystemAPI.Repositories
{
    // Repository-klasse der håndterer CRUD-operationer på "Anmodning" i MongoDB
    public class AnmodningRepositoryMongo : IAnmodningRepository
    {
        private readonly IMongoCollection<Anmodning> _collection;
        private readonly IMongoDatabase _database;

        // Konstruktor hvor Mongo-database injiceres, og samling initialiseres
        public AnmodningRepositoryMongo(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<Anmodning>("Anmodninger");
        }

        // Opretter en ny anmodning med automatisk genereret unikt ID og default status
        public async Task OpretAsync(Anmodning anmodning)
        {
            anmodning.Id = await GetNextIdAsync(); // Genererer næste ledige ID baseret på max i databasen
            anmodning.Status = "Afventer";          // Sætter standardstatus ved oprettelse
            await _collection.InsertOneAsync(anmodning);
        }

        // Henter en anmodning baseret på dens id
        public async Task<Anmodning?> GetByIdAsync(int id)
        {
            return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        // Henter alle anmodninger der er til en specifik modtager og som stadig er i "Afventer" status
        public async Task<List<Anmodning>> GetTilModtagerAsync(int modtagerId)
        {
            return await _collection
                .Find(a => a.ModtagerId == modtagerId && a.Status == "Afventer")
                .ToListAsync();
        }

        // Opdaterer en eksisterende anmodning i databasen
        public async Task UpdateAsync(Anmodning anmodning)
        {
            await _collection.ReplaceOneAsync(a => a.Id == anmodning.Id, anmodning);
        }

        // Henter alle anmodninger i systemet
        public async Task<List<Anmodning>> GetAlleAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        // Hjælpefunktion til at finde det næste ledige id baseret på den højeste eksisterende id i samlingen
        private async Task<int> GetNextIdAsync()
        {
            var all = await _collection.Find(_ => true).ToListAsync();
            return all.Count == 0 ? 1 : all.Max(a => a.Id) + 1;
        }

        // Behandler en anmodning ved enten at acceptere eller afvise den
        public async Task BehandlAsync(int id, bool accepteret)
        {
            var anmodning = await GetByIdAsync(id);
            if (anmodning == null) return;

            var db = _collection.Database;

            if (accepteret)
            {
                // Hvis anmodningen er knyttet til et underdelmål, opdateres dets status
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
                // Hvis anmodningen er knyttet til et delmål, opdateres dets status
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
                // Opdater anmodningens status til "Godkendt"
                anmodning.Status = "Godkendt";
            }
            else
            {
                // Hvis ikke accepteret, sættes status til "Afvist"
                anmodning.Status = "Afvist";
            }

            // Opdater anmodningens status i databasen
            await UpdateAsync(anmodning);
        }
    }
}
