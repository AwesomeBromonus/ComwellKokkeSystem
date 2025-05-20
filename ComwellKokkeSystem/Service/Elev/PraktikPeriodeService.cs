using Modeller;
using System.Net.Http.Json;

namespace ComwellKokkeSystem.Service
{
    public class PraktikperiodeService : IPraktikperiodeService
    {
        private readonly HttpClient _http;

        public PraktikperiodeService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Praktikperiode>> GetAllAsync() =>
            await _http.GetFromJsonAsync<List<Praktikperiode>>("api/praktikperiode");

        public async Task<Praktikperiode?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<Praktikperiode>($"api/praktikperiode/{id}");

        public async Task UpdateDelmålAsync(int praktikperiodeId, int delmålId, string nyStatus)
        {
            var content = JsonContent.Create(nyStatus);
            await _http.PutAsync($"api/praktikperiode/{praktikperiodeId}/delmål/{delmålId}", content);
        }
        public async Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId)
        {
            return await _http.GetFromJsonAsync<List<Praktikperiode>>($"api/praktikperiode/elev/{elevId}");
        }

        public async Task<List<Praktikperiode>> GetByElevIdAsync(int elevId)
        {
            return await _http.GetFromJsonAsync<List<Praktikperiode>>($"api/praktikperiode/elev/{elevId}");
        }
    }

}