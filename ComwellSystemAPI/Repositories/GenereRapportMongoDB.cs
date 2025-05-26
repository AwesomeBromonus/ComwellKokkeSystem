// ComwellSystemAPI/Repositories/GenereRapportMongoDB.cs
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
using ClosedXML.Excel; // VIGTIGT: Sørg for denne er her og ikke OfficeOpenXml

namespace ComwellSystemAPI.Repositories
{
    public class GenereRapportMongoDB : IGenereRapport
    {
        private readonly IMongoCollection<Praktikperiode> _praktikperioder;
        private readonly IMongoCollection<Delmål> _delmål;
        private readonly IMongoCollection<UserModel> _brugere;
        private readonly IMongoCollection<Underdelmaal> _underdelmaal;

        public GenereRapportMongoDB()
        {
            var mongoUri = "mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/";
            var client = new MongoClient(mongoUri);
            var database = client.GetDatabase("Comwell");

            _praktikperioder = database.GetCollection<Praktikperiode>("Praktikperioder");
            _delmål = database.GetCollection<Delmål>("Delmål");
            _brugere = database.GetCollection<UserModel>("Brugere");
            _underdelmaal = database.GetCollection<Underdelmaal>("Underdelmaal");
        }

        public async Task<List<Praktikperiode>> GetPraktikPerioderAsync(int year)
        {
            try
            {
                Console.WriteLine($"Attempting to connect to MongoDB for praktikperioder, year: {year}");
                var filter = Builders<Praktikperiode>.Filter.And(
                    Builders<Praktikperiode>.Filter.Exists(p => p.StartDato, true),
                    Builders<Praktikperiode>.Filter.Ne(p => p.StartDato, DateTime.MinValue),
                    Builders<Praktikperiode>.Filter.Eq(p => p.StartDato.Year, year)
                );
                var result = await _praktikperioder.Find(filter).ToListAsync();
                Console.WriteLine($"Found {result.Count} praktikperioder for year {year}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPraktikPerioderAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<List<Delmål>> GetDelmålAsync(int year)
        {
            try
            {
                var result = await _delmål.Find(d => d.Deadline.Year == year).ToListAsync();
                Console.WriteLine($"Found {result.Count} delmål for year {year}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDelmålAsync: {ex.Message}");
                return new List<Delmål>();
            }
        }

        public async Task<List<Delmål>> GetDelmålMånedAsync(int year, int month)
        {
            try
            {
                return await _delmål.Find(d => d.Deadline.Year == year && d.Deadline.Month == month)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDelmålMånedAsync: {ex.Message}");
                return new List<Delmål>();
            }
        }

        public async Task<List<UserModel>> GetBrugereAsync(int year)
        {
            try
            {
                var filter = Builders<UserModel>.Filter.And(
                    Builders<UserModel>.Filter.Exists(u => u.StartDato, true),
                    Builders<UserModel>.Filter.Ne(u => u.StartDato, DateTime.MinValue),
                    Builders<UserModel>.Filter.Eq(u => u.StartDato.Year, year)
                );

                var result = await _brugere.Find(filter).ToListAsync();
                Console.WriteLine($"Found {result.Count} users for year {year}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBrugereAsync: {ex.Message}");
                try
                {
                    return await _brugere.Find(_ => true).ToListAsync();
                }
                catch
                {
                    return new List<UserModel>();
                }
            }
        }

        public async Task<List<Delmål>> GetFuldførteDelmålAsync(int year)
        {
            try
            {
                return await _delmål.Find(d => d.Deadline.Year == year && d.Status == "Fuldført")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFuldførteDelmålAsync: {ex.Message}");
                return new List<Delmål>();
            }
        }

        public async Task<List<Praktikperiode>> GetPraktikPerioderPerElevAsync(int elevId, int year)
        {
            try
            {
                return await _praktikperioder.Find(p => p.ElevId == elevId && p.StartDato.Year == year)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPraktikPerioderPerElevAsync: {ex.Message}");
                return new List<Praktikperiode>();
            }
        }

        public async Task<int> GetTotalTimerAsync(int year)
        {
            return 0; // Skal implementeres, hvis relevant
        }

        public async Task<List<Delmål>> GetAllDelmaalWithUnderdelmaalAsync(int year)
        {
            try
            {
                Console.WriteLine($"Starting GetAllDelmaalWithUnderdelmaalAsync for year: {year}");

                var databaseNames = await _delmål.Database.ListCollectionNames().ToListAsync();
                Console.WriteLine($"Connected to MongoDB. Collections: {string.Join(", ", databaseNames)}");

                Console.WriteLine($"Fetching delmål for year: {year}");
                var allDelmaal = await _delmål.Find(d => d.Deadline.Year == year).ToListAsync();
                Console.WriteLine($"Found {allDelmaal.Count} delmål");

                Console.WriteLine($"Fetching all underdelmål");
                var allUnderdelmaal = await _underdelmaal.Find(_ => true).ToListAsync();
                Console.WriteLine($"Found {allUnderdelmaal.Count} underdelmål");

                var groupedUnderdelmaal = allUnderdelmaal
                    .GroupBy(ud => ud.DelmålId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var dm in allDelmaal)
                {
                    if (groupedUnderdelmaal.TryGetValue(dm.Id, out var underdelmaalForDelmaal))
                    {
                        dm.UnderdelmaalList = underdelmaalForDelmaal;
                    }
                    else
                    {
                        dm.UnderdelmaalList = new List<Underdelmaal>();
                    }
                }

                return allDelmaal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error in GetAllDelmaalWithUnderdelmaalAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<byte[]> ExportToExcelAsync(int year)
        {
            try
            {
                Console.WriteLine($"Starting Excel export for year: {year} using ClosedXML.");

                var allDelmaal = await GetAllDelmaalWithUnderdelmaalAsync(year);
                Console.WriteLine($"Retrieved {allDelmaal?.Count ?? 0} delmål");

                var userModels = await GetBrugereAsync(year);
                Console.WriteLine($"Retrieved {userModels?.Count ?? 0} users");

                var praktikperioder = await GetPraktikPerioderAsync(year);
                Console.WriteLine($"Retrieved {praktikperioder?.Count ?? 0} praktikperioder");

                allDelmaal ??= new List<Delmål>();
                userModels ??= new List<UserModel>();
                praktikperioder ??= new List<Praktikperiode>();

                // Opret en liste af din ViewModel for at matche den data, du vil eksportere
                var rapportData = new List<RapportElevDelmålViewModel>();

                foreach (var delmaal in allDelmaal)
                {
                    try
                    {
                        var user = userModels.FirstOrDefault(u => u.Id == delmaal.ElevId);
                        var praktikperiode = praktikperioder.FirstOrDefault(pp => pp.Id == delmaal.PraktikperiodeId);

                        string progressText = "Ingen underdelmål";
                        double progressPercent = 0;

                        if (delmaal.UnderdelmaalList != null && delmaal.UnderdelmaalList.Any())
                        {
                            int done = delmaal.UnderdelmaalList.Count(ud => ud.Status == "Fuldført");
                            int total = delmaal.UnderdelmaalList.Count;
                            progressPercent = (total > 0) ? Math.Round((double)done / total * 100, 0) : 0;
                            progressText = $"{done}/{total} ({progressPercent}%)";
                        }

                        // Opret en instans af din ViewModel og fyld den
                        rapportData.Add(new RapportElevDelmålViewModel
                        {
                            ElevNavn = user?.Navn ?? "Ukendt elev",
                            Username = user?.Username ?? "Ukendt",
                            HotelNavn = user?.HotelNavn ?? "Ukendt hotel",
                            Rolle = user?.Role ?? "Ukendt rolle",
                            PraktikperiodeNavn = praktikperiode?.Navn ?? "Ukendt praktikperiode",
                            DelmålBeskrivelse = delmaal.Beskrivelse ?? "Ingen beskrivelse",
                            DelmålAnsvarlig = delmaal.Ansvarlig ?? "Ukendt",
                            DelmålCalculatedStatus = delmaal.CalculatedStatus ?? delmaal.Status ?? "Ukendt", // Brug CalculatedStatus
                            DelmålProgressText = progressText, // Brug den beregnede tekst
                            DelmålDeadline = delmaal.Deadline != default ? delmaal.Deadline : DateTime.MinValue // Sørg for at deadline er gyldig
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing delmål {delmaal.Id} for export: {ex.Message}\nStackTrace: {ex.StackTrace}");
                    }
                }

                Console.WriteLine($"Processed {rapportData.Count} rows for ClosedXML export.");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("HR Rapport");

                    // Tilføj kolonnehoveder
                    worksheet.Cell(1, 1).Value = "Elev Navn";
                    worksheet.Cell(1, 2).Value = "Username";
                    worksheet.Cell(1, 3).Value = "Hotel";
                    worksheet.Cell(1, 4).Value = "Rolle";
                    worksheet.Cell(1, 5).Value = "Praktikperiode";
                    worksheet.Cell(1, 6).Value = "Delmål Beskrivelse";
                    worksheet.Cell(1, 7).Value = "Ansvarlig";
                    worksheet.Cell(1, 8).Value = "Delmål Status";
                    worksheet.Cell(1, 9).Value = "Progress";
                    worksheet.Cell(1, 10).Value = "Deadline";

                    // Indsæt data
                    worksheet.Cell(2, 1).InsertData(rapportData);

                    // Tilpas kolonnernes bredde
                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var result = stream.ToArray();
                        Console.WriteLine($"Excel file generated successfully with ClosedXML, size: {result.Length} bytes");
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error in ExportToExcelAsync (ClosedXML): {ex.Message}\nStackTrace: {ex.StackTrace}");
                throw; // Kast exception for at få den fanget i controlleren
            }
        }
    }
}