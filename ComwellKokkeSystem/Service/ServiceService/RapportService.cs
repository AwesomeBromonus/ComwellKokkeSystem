using System.Net.Http;
using System.Threading.Tasks;

namespace ComwellKokkeSystem.Service
{
    // @* KLASSE: Serviceklasse der implementerer IRapportService og håndterer rapportgenerering via HTTP-requests *@
    public class RapportService : IRapportService
    {
        private readonly HttpClient _http;

        public RapportService(HttpClient http)
        {
            _http = http;
        }

        // @* Henter Excel-rapport som byte-array via GET *@
        public async Task<byte[]> HentExcelRapportAsync()
        {
            return await _http.GetByteArrayAsync("api/rapport/excel");
        }
    }
}
