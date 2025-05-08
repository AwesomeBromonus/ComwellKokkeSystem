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
        }

    
}
