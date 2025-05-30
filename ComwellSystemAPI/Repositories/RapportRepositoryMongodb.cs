using ClosedXML.Excel;
using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class RapportRepository : IRapportRepository
{
    private readonly IMongoCollection<UserModel> _userCollection;
    private readonly IMongoCollection<Delmål> _delmaalCollection;
    private readonly IMongoCollection<Underdelmaal> _underdelmaalCollection;
    private readonly IMongoCollection<Praktikperiode> _praktikperiodeCollection;

    // Konstruktor hvor MongoDB-samlinger injiceres til at hente data
    public RapportRepository(IMongoDatabase database)
    {
        _userCollection = database.GetCollection<UserModel>("Brugere");
        _delmaalCollection = database.GetCollection<Delmål>("Delmål");
        _underdelmaalCollection = database.GetCollection<Underdelmaal>("Underdelmaal");
        _praktikperiodeCollection = database.GetCollection<Praktikperiode>("Praktikperioder");
    }

    // Metode til at generere Excel-rapport med elevers delmål og underdelmål
    public async Task<byte[]> GenererElevDelmaalExcelAsync()
    {
        // Hent alle brugere og filtrer på elever
        var brugere = await _userCollection.Find(_ => true).ToListAsync();
        var elever = brugere.Where(b => b.Role?.ToLower() == "elev").ToList();

        // Hent alle delmål og praktikperioder
        var delmaalListe = await _delmaalCollection.Find(_ => true).ToListAsync();
        var praktikperioder = await _praktikperiodeCollection.Find(_ => true).ToListAsync();

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Brugerrapport");

        // Opsæt header række med kolonnenavne og styling
        ws.Cell(1, 1).Value = "Elevnavn";
        ws.Cell(1, 2).Value = "Email";
        ws.Cell(1, 3).Value = "Hotel";
        ws.Cell(1, 4).Value = "Delmål";
        ws.Cell(1, 5).Value = "Delmål Status";
        ws.Cell(1, 6).Value = "Delmål Deadline";
        ws.Cell(1, 7).Value = "Underdelmål";
        ws.Cell(1, 8).Value = "Underdelmål Status";
        ws.Cell(1, 9).Value = "Underdelmål Deadline";
        ws.Cell(1, 10).Value = "Praktikperiode";
        ws.Cell(1, 11).Value = "Periode Status";

        ws.Range("A1:K1").Style.Font.Bold = true;
        ws.Range("A1:K1").Style.Fill.BackgroundColor = XLColor.LightGray;

        int row = 2;

        // Loop igennem hver elev og deres delmål
        foreach (var elev in elever)
        {
            var elevensDelmaal = delmaalListe.Where(d => d.ElevId == elev.Id).ToList();

            foreach (var d in elevensDelmaal)
            {
                var praktikperiode = praktikperioder.FirstOrDefault(p => p.Id == d.PraktikperiodeId);

                // Hent underdelmål til det aktuelle delmål
                var underdelmaalListe = await _underdelmaalCollection
                    .Find(u => u.DelmaalId == d.Id)
                    .ToListAsync();

                if (underdelmaalListe.Any())
                {
                    // Hvis der findes underdelmål, tilføj én række per underdelmål
                    foreach (var u in underdelmaalListe)
                    {
                        ws.Cell(row, 1).Value = elev?.Navn ?? "";
                        ws.Cell(row, 2).Value = elev?.Email ?? "";
                        ws.Cell(row, 3).Value = elev?.HotelNavn ?? "Ukendt";
                        ws.Cell(row, 4).Value = d?.Beskrivelse ?? "";
                        ws.Cell(row, 5).Value = d?.Status ?? "";
                        ws.Cell(row, 6).Value = d?.Deadline.ToShortDateString() ?? "";
                        ws.Cell(row, 7).Value = u?.Beskrivelse ?? "";
                        ws.Cell(row, 8).Value = u?.Status ?? "";
                        ws.Cell(row, 9).Value = u?.Deadline.ToShortDateString() ?? "";
                        ws.Cell(row, 10).Value = praktikperiode?.Navn ?? "Ukendt";
                        ws.Cell(row, 11).Value = praktikperiode?.Status ?? "Ukendt";
                        row++;
                    }
                }
                else
                {
                    // Hvis ingen underdelmål, skriv "-" i de relevante kolonner
                    ws.Cell(row, 1).Value = elev?.Navn ?? "";
                    ws.Cell(row, 2).Value = elev?.Email ?? "";
                    ws.Cell(row, 3).Value = elev?.HotelNavn ?? "Ukendt";
                    ws.Cell(row, 4).Value = d?.Beskrivelse ?? "";
                    ws.Cell(row, 5).Value = d?.Status ?? "";
                    ws.Cell(row, 6).Value = d?.Deadline.ToShortDateString() ?? "";
                    ws.Cell(row, 7).Value = "-";
                    ws.Cell(row, 8).Value = "-";
                    ws.Cell(row, 9).Value = "-";
                    ws.Cell(row, 10).Value = praktikperiode?.Navn ?? "Ukendt";
                    ws.Cell(row, 11).Value = praktikperiode?.Status ?? "Ukendt";
                    row++;
                }
            }
        }

        // Gemmer regnearket til en memory stream og returnerer som byte-array
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
