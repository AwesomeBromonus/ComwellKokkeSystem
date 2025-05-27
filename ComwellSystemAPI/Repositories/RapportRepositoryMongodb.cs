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

        public RapportRepository()
        {
            var client = new MongoClient("mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/"); // <- din connection string
            var database = client.GetDatabase("Comwell");

            _userCollection = database.GetCollection<UserModel>("Brugere");
            _delmaalCollection = database.GetCollection<Delmål>("Delmål");
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
            ws.Cell(1, 3).Value = "Delmålbeskrivelse";
            ws.Cell(1, 4).Value = "Status";
            ws.Cell(1, 5).Value = "Kommentar";
            ws.Cell(1, 6).Value = "Deadline";

            int row = 2;

            foreach (var elev in elever)
            {
                var elevensDelmaal = delmaalListe.Where(d => d.ElevId == elev.Id).ToList();

                foreach (var d in elevensDelmaal)
                {
                    ws.Cell(row, 1).Value = elev.Navn;
                    ws.Cell(row, 2).Value = elev.Email;
                    ws.Cell(row, 3).Value = d.Beskrivelse;
                    ws.Cell(row, 4).Value = d.Status;
                    ws.Cell(row, 5).Value = d.Kommentar;
                    ws.Cell(row, 6).Value = d.Deadline.ToShortDateString();
                    row++;
                }
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
