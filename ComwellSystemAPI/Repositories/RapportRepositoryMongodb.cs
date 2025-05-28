using ClosedXML.Excel;
using MongoDB.Driver;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Repositories
{
    public class RapportRepository : IRapportRepository
    {
        private readonly IMongoCollection<UserModel> _userCollection;
        private readonly IMongoCollection<Delmål> _delmaalCollection;
        private readonly IMongoCollection<Praktikperiode> _praktikperiodeCollection;

        public RapportRepository(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<UserModel>("Brugere");
            _delmaalCollection = database.GetCollection<Delmål>("Delmål");
            _praktikperiodeCollection = database.GetCollection<Praktikperiode>("Praktikperioder");
        }
        public async Task<byte[]> GenererElevDelmaalExcelAsync()
        {
            var brugere = await _userCollection.Find(_ => true).ToListAsync();
            var elever = brugere.Where(b => b.Role.ToLower() == "elev").ToList();

            var delmaalListe = await _delmaalCollection.Find(_ => true).ToListAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Brugerrapport");

            ws.Cell(1, 1).Value = "Elevnavn";
            ws.Cell(1, 2).Value = "Email";
            ws.Cell(1, 3).Value = "Hotel";
            ws.Cell(1, 4).Value = "Delmål";
            ws.Cell(1, 5).Value = "Status";
            ws.Cell(1, 6).Value = "Kommentar";
            ws.Cell(1, 7).Value = "Deadline";
            ws.Cell(1, 8).Value = "Praktikperiode";
            ws.Cell(1, 9).Value = "Status";

            // 🎨 Styling af header-rækken (række 1)
            ws.Range("A1:I1").Style.Font.Bold = true;
            ws.Range("A1:I1").Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;

            foreach (var elev in elever)
            {
                var elevensDelmaal = delmaalListe.Where(d => d.ElevId == elev.Id).ToList();
                var praktikperioder = await _praktikperiodeCollection.Find(_ => true).ToListAsync();


                foreach (var d in elevensDelmaal)
                {
                    var praktikperiode = praktikperioder.FirstOrDefault(p => p.Id == d.PraktikperiodeId);

                    ws.Cell(row, 1).Value = elev?.Navn ?? "";
                    ws.Cell(row, 2).Value = elev?.Email ?? "";
                    ws.Cell(row, 3).Value = elev?.HotelNavn ?? "Ukendt";
                    ws.Cell(row, 4).Value = d?.Beskrivelse ?? "";
                    ws.Cell(row, 5).Value = d?.Status ?? "";
                    ws.Cell(row, 6).Value = d?.Kommentar ?? "";
                    ws.Cell(row, 7).Value = d?.Deadline.ToShortDateString();
                    ws.Cell(row, 8).Value = praktikperiode?.Navn ?? "Ukendt";
                    ws.Cell(row, 9).Value = praktikperiode?.Status ?? "Ukendt";

                    row++;
                }

            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
