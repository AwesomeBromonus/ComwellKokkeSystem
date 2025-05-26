using Modeller;
using System.Net.Http.Json;

namespace ComwellKokkeSystem.Service
{
    public class AnmodningService : IAnmodningService
    {
        private readonly HttpClient _http;

        public AnmodningService(HttpClient http)
        {
            _http = http;
        }

        public async Task OpretAsync(Anmodning anmodning)
        {
            await _http.PostAsJsonAsync("api/anmodning", anmodning);
        }

        public async Task<Anmodning?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Anmodning>($"api/anmodning/{id}");
        }

        public async Task<List<Anmodning>> GetTilModtagerAsync(int modtagerId)
        {
            return await _http.GetFromJsonAsync<List<Anmodning>>($"api/anmodning/modtager/{modtagerId}");
        }

        public async Task UpdateAsync(Anmodning anmodning)
        {
            await _http.PutAsJsonAsync($"api/anmodning/{anmodning.Id}", anmodning);
        }

        public async Task<List<Anmodning>> GetAlleAsync()
        {
            return await _http.GetFromJsonAsync<List<Anmodning>>("api/anmodning");
        }
        public async Task BehandlAsync(int anmodningId, bool accepteret)
        {
            var response = await _http.PutAsJsonAsync($"api/anmodning/behandl/{anmodningId}", accepteret);
            response.EnsureSuccessStatusCode();
        }

    }
}
