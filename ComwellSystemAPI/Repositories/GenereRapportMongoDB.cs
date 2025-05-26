using ComwellSystemAPI.Interfaces;
using Modeller; // Sørg for at denne using er her
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Linq; // Vigtig for GroupBy og ToDictionary
using ClosedXML.Excel;

namespace ComwellSystemAPI.Repositories
{
    public class GenereRapportMongoDB : IGenereRapport
    {
        private readonly IMongoCollection<Praktikperiode> _praktikperioder;
        private readonly IMongoCollection<Delmål> _delmål;
        private readonly IMongoCollection<UserModel> _brugere;
        
        // Injicer IUnderdelmaal repo'et her
        private readonly IUnderdelmaal _underdelmaalRepository; // Denne bruges nu til at hente Underdelmaal

        // Konstruktør: Modtag IMongoDatabase og IUnderdelmaal via Dependency Injection
        public GenereRapportMongoDB(IMongoDatabase database, IUnderdelmaal underdelmaalRepository)
        {
            _praktikperioder = database.GetCollection<Praktikperiode>("Praktikperioder");
            _delmål = database.GetCollection<Delmål>("Delmål");
            _brugere = database.GetCollection<UserModel>("Brugere");
            
            _underdelmaalRepository = underdelmaalRepository; // Tildel det injicerede repo
        }

        // --- Grå metoder (implementeret, men ikke kaldt af Blazor UI lige nu) ---
        // Disse metoder er fine, ingen ændringer nødvendige her.
        public async Task<List<Delmål>> GetDelmålMånedAsync(int year, int month)
        {
            try
            {
                return await _delmål.Find(d => d.Deadline != default(DateTime) && d.Deadline.Year == year && d.Deadline.Month == month)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDelmålMånedAsync: {ex.Message}");
                return new List<Delmål>();
            }
        }

        public async Task<List<Delmål>> GetFuldførteDelmålAsync(int year)
        {
            try
            {
                return await _delmål.Find(d => d.Deadline != default(DateTime) && d.Deadline.Year == year && d.Status == "Fuldført")
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
                return await _praktikperioder.Find(p => p.ElevId == elevId && p.StartDato != default(DateTime) && p.StartDato.Year == year)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPraktikPerioderPerElevAsync: {ex.Message}");
                return new List<Praktikperiode>();
            }
        }
        // --- Slut på grå metoder ---

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
                var result = await _delmål.Find(d => d.Deadline != default(DateTime) && d.Deadline.Year == year).ToListAsync();
                Console.WriteLine($"Found {result.Count} delmål for year {year}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDelmålAsync: {ex.Message}");
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
                    // Fallback to fetch all users if year filter fails, or return empty
                    return await _brugere.Find(_ => true).ToListAsync();
                }
                catch
                {
                    return new List<UserModel>();
                }
            }
        }

        public async Task<int> GetTotalTimerAsync(int year)
        {
            return 0; // Skal implementeres, hvis relevant
        }

        // **ÆNDRET LOGIK HER:** GetAllDelmaalWithUnderdelmaalAsync
        public async Task<List<Delmål>> GetAllDelmaalWithUnderdelmaalAsync(int year)
        {
            try
            {
                Console.WriteLine($"Starting GetAllDelmaalWithUnderdelmaalAsync for year: {year}");

                Console.WriteLine($"Fetching delmål for year: {year}");
                var allDelmaal = await _delmål.Find(d => d.Deadline != default(DateTime) && d.Deadline.Year == year).ToListAsync();
                Console.WriteLine($"Found {allDelmaal.Count} delmål");

                // Hent praktikperioder. Disse skal bruges til at slå navne op senere,
                // men IKKE til at ændre Delmål-objektet.
                var praktikperioder = await _praktikperioder.Find(_ => true).ToListAsync();
                var praktikperiodeMap = praktikperioder.ToDictionary(p => p.Id, p => p.Navn); // Dette map er klar til brug.

                // Hent underdelmål for hvert delmål
                foreach (var dm in allDelmaal)
                {
                    // Brug den fungerende Underdelmaal repository til at hente underdelmål
                    // for hvert delmål individuelt.
                    dm.UnderdelmaalList = await _underdelmaalRepository.GetByDelmaalIdAsync(dm.Id) ?? new List<Underdelmaal>();

                    // VIGTIGT: Fjernet linjerne der forsøgte at sætte PraktikperiodeNavn
                    // på Delmål-objektet, da den property ikke eksisterer og ikke bør eksistere der.
                }

                // Returnerer den berigede liste af Delmål, hvor hvert Delmål har sin UnderdelmaalList udfyldt.
                // PraktikperiodeNavn vil blive håndteret, når du bygger din RapportElevDelmålViewModel.
                return allDelmaal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error in GetAllDelmaalWithUnderdelmaalAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                throw;
            }
        }

        // Denne metode implementerer det, der forventes af IGenereRapport
        // Bemærk: Denne metode modtager allerede 'RapportElevDelmålViewModel',
        // som bør have PraktikperiodeNavn udfyldt fra en tidligere step.
        public async Task<byte[]> ExportToExcelAsync(List<Modeller.RapportElevDelmålViewModel> dataToExport)
        {
            try
            {
                Console.WriteLine($"Starting Excel export for {dataToExport.Count} rows using ClosedXML.");

                if (!dataToExport.Any())
                {
                    Console.WriteLine("No data provided for export. Returning empty byte array.");
                    return Array.Empty<byte>();
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("HR Rapport");

                    // Opsæt overskrifter for Excel-arket
                    worksheet.Cell(1, 1).Value = "Elev Navn";
                    worksheet.Cell(1, 2).Value = "Username";
                    worksheet.Cell(1, 3).Value = "Hotel";
                    worksheet.Cell(1, 4).Value = "Rolle";
                    worksheet.Cell(1, 5).Value = "Praktikperiode"; // Denne overskrift er fin!
                    worksheet.Cell(1, 6).Value = "Delmål Beskrivelse";
                    worksheet.Cell(1, 7).Value = "Ansvarlig";
                    worksheet.Cell(1, 8).Value = "Delmål Status";
                    worksheet.Cell(1, 9).Value = "Progress";
                    worksheet.Cell(1, 10).Value = "Deadline";

                    int row = 2;
                    foreach (var item in dataToExport)
                    {
                        worksheet.Cell(row, 1).Value = item.ElevNavn;
                        worksheet.Cell(row, 2).Value = item.Username;
                        worksheet.Cell(row, 3).Value = item.HotelNavn;
                        worksheet.Cell(row, 4).Value = item.Rolle;
                        worksheet.Cell(row, 5).Value = item.PraktikperiodeNavn; // Denne property skal findes i RapportElevDelmålViewModel
                        worksheet.Cell(row, 6).Value = item.DelmålBeskrivelse;
                        worksheet.Cell(row, 7).Value = item.DelmålAnsvarlig;
                        worksheet.Cell(row, 8).Value = item.DelmålCalculatedStatus;
                        worksheet.Cell(row, 9).Value = item.DelmålProgressText;
                        worksheet.Cell(row, 10).Value = item.DelmålDeadline != DateTime.MinValue ? item.DelmålDeadline.ToShortDateString() : "Ukendt";
                        row++;
                    }

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
                throw;
            }
        }
    }
}