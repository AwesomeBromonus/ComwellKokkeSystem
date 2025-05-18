using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Linq;
using ClosedXML.Excel; // You'll need to add this NuGet package

namespace ComwellSystemAPI.Repositories
{
    public class GenereRapportMongoDB : IGenereRapport
    {
        // MongoDB-kollektioner
        private readonly IMongoCollection<Praktikperiode> _praktikperioder;
        private readonly IMongoCollection<Delmål> _delmål;
        private readonly IMongoCollection<UserModel> _brugere;

        public GenereRapportMongoDB()
        {
            var mongoUri = "mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/";
            var client = new MongoClient(mongoUri);
            var database = client.GetDatabase("Comwell");
            _praktikperioder = database.GetCollection<Praktikperiode>("Praktikperioder");
            _delmål = database.GetCollection<Delmål>("Delmål");
            _brugere = database.GetCollection<UserModel>("Brugere");
        }

        public async Task<List<Praktikperiode>> GetPraktikPerioderAsync(int year)
        {
            return await _praktikperioder.Find(p => p.StartDato.Year == year)
                .ToListAsync();
        }

        public async Task<List<Delmål>> GetDelmålAsync(int year)
        {
            return await _delmål.Find(d => d.Deadline.Year == year)
                .ToListAsync();
        }

        public async Task<List<Delmål>> GetDelmålMånedAsync(int year, int month)
        {
            return await _delmål.Find(d => d.Deadline.Year == year && d.Deadline.Month == month)
                .ToListAsync();
        }

        public async Task<List<UserModel>> GetBrugereAsync(int year)
        {
            return await _brugere.Find(b => b.StartDato.Year == year)
                .ToListAsync();
        }

        public async Task<List<Delmål>> GetFuldførteDelmålAsync(int year)
        {
            return await _delmål.Find(d => d.Deadline.Year == year && d.Status == "Fuldført")
                .ToListAsync();
        }

        public async Task<List<Praktikperiode>> GetPraktikPerioderPerElevAsync(int elevId, int year)
        {
            return await _praktikperioder.Find(p => p.ElevId == elevId && p.StartDato.Year == year)
                .ToListAsync();
        }

        public async Task<int> GetTotalTimerAsync(int year)
        {
            // Placeholder - skal implementeres, hvis timer registreres
            return 0;
        }

        public async Task<byte[]> ExportToCsvAsync(int year)
        {
            // Hent alle data
            var praktikperioder = await GetPraktikPerioderAsync(year);
            var delmål = await GetDelmålAsync(year);
            var brugere = await GetBrugereAsync(year);

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            
            // Praktikperioder
            writer.WriteLine("PRAKTIKPERIODER");
            writer.WriteLine("Navn,StartDato,SlutDato");
            foreach (var periode in praktikperioder)
            {
                writer.WriteLine($"{periode.Navn},{periode.StartDato:dd/MM/yyyy},{periode.SlutDato:dd/MM/yyyy}");
            }
            
            writer.WriteLine(); // Tom linje mellem sektioner
            
            // Delmål
            writer.WriteLine("DELMÅL");
            writer.WriteLine("Beskrivelse,Deadline,Status");
            foreach (var dm in delmål)
            {
                writer.WriteLine($"{dm.Beskrivelse},{dm.Deadline:dd/MM/yyyy},{dm.Status}");
            }
            
            writer.WriteLine(); // Tom linje mellem sektioner
            
            // Brugere
            writer.WriteLine("NYE BRUGERE");
            writer.WriteLine("Navn,StartDato");
            foreach (var bruger in brugere)
            {
                writer.WriteLine($"{bruger.Navn},{bruger.StartDato:dd/MM/yyyy}");
            }
            
            writer.Flush();
            return memoryStream.ToArray();
        }

        public async Task<byte[]> ExportToExcelAsync(int year)
        {
            // Hent alle data
            var praktikperioder = await GetPraktikPerioderAsync(year);
            var delmål = await GetDelmålAsync(year);
            var brugere = await GetBrugereAsync(year);

            using var workbook = new XLWorkbook();
            
            // Praktikperioder worksheet
            var worksheetPerioder = workbook.Worksheets.Add("Praktikperioder");
            worksheetPerioder.Cell(1, 1).Value = "Navn";
            worksheetPerioder.Cell(1, 2).Value = "Start Dato";
            worksheetPerioder.Cell(1, 3).Value = "Slut Dato";
            
            int row = 2;
            foreach (var periode in praktikperioder)
            {
                worksheetPerioder.Cell(row, 1).Value = periode.Navn;
                worksheetPerioder.Cell(row, 2).Value = periode.StartDato.ToString("dd/MM/yyyy") ?? "Ingen dato";
                worksheetPerioder.Cell(row, 3).Value = periode.SlutDato.ToString("dd/MM/yyyy") ?? "Ingen dato";
                row++;
            }
            
            // Style header
            var headerRange = worksheetPerioder.Range(1, 1, 1, 3);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            
            worksheetPerioder.Columns().AdjustToContents();
            
            // Delmål worksheet
            var worksheetDelmål = workbook.Worksheets.Add("Delmål");
            worksheetDelmål.Cell(1, 1).Value = "Beskrivelse";
            worksheetDelmål.Cell(1, 2).Value = "Deadline";
            worksheetDelmål.Cell(1, 3).Value = "Status";
            
            row = 2;
            foreach (var dm in delmål)
            {
                worksheetDelmål.Cell(row, 1).Value = dm.Beskrivelse;
                worksheetDelmål.Cell(row, 2).Value = dm.Deadline.ToString("dd/MM/yyyy") ?? "Ingen dato";
                worksheetDelmål.Cell(row, 3).Value = dm.Status;
                row++;
            }
            
            // Style header
            headerRange = worksheetDelmål.Range(1, 1, 1, 3);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            
            worksheetDelmål.Columns().AdjustToContents();
            
            // Brugere worksheet
            var worksheetBrugere = workbook.Worksheets.Add("Nye Brugere");
            worksheetBrugere.Cell(1, 1).Value = "Navn";
            worksheetBrugere.Cell(1, 2).Value = "Start Dato";
            
            row = 2;
            foreach (var bruger in brugere)
            {
                worksheetBrugere.Cell(row, 1).Value = bruger.Navn;
                worksheetBrugere.Cell(row, 2).Value = bruger.StartDato.ToString("dd/MM/yyyy") ?? "Ingen dato";
                row++;
            }
            
            // Style header
            headerRange = worksheetBrugere.Range(1, 1, 1, 2);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            
            worksheetBrugere.Columns().AdjustToContents();
            
            // Save to memory stream
            using var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }
    }
}