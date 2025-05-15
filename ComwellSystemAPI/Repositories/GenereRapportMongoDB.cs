using Modeller;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Repositories
{
    public class GenereRapportMongoDB : IGenereRapport
    {
        private readonly IMongoCollection<Delmål> _delmålCollection;
        private readonly IMongoCollection<UserModel> _brugerCollection;
        private readonly IMongoCollection<Praktikperiode> _praktikperiodeCollection;

        public GenereRapportMongoDB(IMongoDatabase database)
        {
            _delmålCollection = database.GetCollection<Delmål>("Delmål");
            _brugerCollection = database.GetCollection<UserModel>("Brugere");
            _praktikperiodeCollection = database.GetCollection<Praktikperiode>("Praktikperioder");
        }

        public async Task<List<DelmålDTO>> GetDelmålAsync(int year)
        {
            var praktikperioder = await _praktikperiodeCollection
                .Find(p => p.StartDato.Year == year || p.SlutDato.Year == year)
                .ToListAsync();
            var delmål = await _delmålCollection
                .Find(d => praktikperioder.Any(p => p.Id == d.PraktikperiodeId))
                .ToListAsync();
            var brugere = await _brugerCollection
                .Find(b => true) // Kan optimeres til at filtrere på relevante ElevIds
                .ToListAsync();

            var result = (from d in delmål
                         join p in praktikperioder on d.PraktikperiodeId equals p.Id
                         join b in brugere on d.ElevId equals b.Id
                         select new DelmålDTO
                         {
                             Id = d.Id,
                             Beskrivelse = d.Beskrivelse,
                             Deadline = d.Deadline,
                             Status = d.Status,
                             ElevId = d.ElevId,
                             ElevNavn = b.Navn
                         }).ToList();

            return result.Any() ? result : new List<DelmålDTO>();
        }

        public async Task<List<Praktikperiode>> GetPraktikPerioderAsync(int year)
        {
            var filter = Builders<Praktikperiode>.Filter.Where(p =>
                p.StartDato.Year == year || p.SlutDato.Year == year);
            return await _praktikperiodeCollection.Find(filter).ToListAsync();
        }

        public async Task<List<UserModel>> GetBrugereAsync(int year)
        {
            var filter = Builders<UserModel>.Filter.Where(u =>
                u.StartDato.Year == year && u.Role != null && u.Role.Equals("elev", StringComparison.OrdinalIgnoreCase));
            return await _brugerCollection.Find(filter).ToListAsync();
        }

        public async Task<byte[]> ExportToCsvAsync(int year)
        {
            throw new NotImplementedException(); // Implementér senere
        }

        public async Task<byte[]> ExportToExcelAsync(int year)
        {
            throw new NotImplementedException(); // Implementér senere
        }

        public async Task<List<Praktikperiode>> GetPraktikperioderPerElevAsync(int elevId)
        {
            var filter = Builders<Praktikperiode>.Filter.Eq(p => p.ElevId, elevId);
            return await _praktikperiodeCollection.Find(filter).ToListAsync();
        }

        public async Task<List<Delmål>> GetFuldførteDelmålAsync(int elevId)
        {
            var filter = Builders<Delmål>.Filter.And(
                Builders<Delmål>.Filter.Eq(d => d.ElevId, elevId),
                Builders<Delmål>.Filter.Eq(d => d.Status, "Fuldført"));
            return await _delmålCollection.Find(filter).ToListAsync();
        }
    }
}