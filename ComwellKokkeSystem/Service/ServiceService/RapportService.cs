
using System.Net.Http;
using System.Threading.Tasks;

namespace ComwellKokkeSystem.Service
{
    public class RapportService : IRapportService
    {
        private readonly HttpClient _http;

        public RapportService(HttpClient http)
        {
            _http = http;
        }

        public async Task<byte[]> HentExcelRapportAsync()
        {
            return await _http.GetByteArrayAsync("api/rapport/excel");
        }
    }
}
