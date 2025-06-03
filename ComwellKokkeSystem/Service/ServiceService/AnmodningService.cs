using Modeller;
using System.Net.Http.Json;

namespace ComwellKokkeSystem.Service
{
    // @* KLASSE: Service klasse der implementerer IAnmodningService for håndtering af anmodninger via HTTP API *@
    public class AnmodningService : IAnmodningService
    {
        private readonly HttpClient _http;

        //konstruktør
        public AnmodningService(HttpClient http)
        {
            _http = http;
        }

        // @* Opretter en ny anmodning via POST *@
        public async Task OpretAsync(Anmodning anmodning)
        {
            await _http.PostAsJsonAsync("api/anmodning", anmodning);
        }

        // @* Henter en anmodning efter ID via GET *@
        public async Task<Anmodning?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Anmodning>($"api/anmodning/{id}");
        }

        // @* Henter alle anmodninger til en given modtager via GET *@
        public async Task<List<Anmodning>> GetTilModtagerAsync(int modtagerId)
        {
            return await _http.GetFromJsonAsync<List<Anmodning>>($"api/anmodning/modtager/{modtagerId}");
        }

        // @* Opdaterer en anmodning via PUT *@
        public async Task UpdateAsync(Anmodning anmodning)
        {
            await _http.PutAsJsonAsync($"api/anmodning/{anmodning.Id}", anmodning);
        }

        // @* Henter alle anmodninger via GET *@
        public async Task<List<Anmodning>> GetAlleAsync()
        {
            return await _http.GetFromJsonAsync<List<Anmodning>>("api/anmodning");
        }

        // @* Behandler (accepterer/afviser) en anmodning via PUT *@
        public async Task BehandlAsync(int anmodningId, bool accepteret)
        {
            var response = await _http.PutAsJsonAsync($"api/anmodning/behandl/{anmodningId}", accepteret);
            response.EnsureSuccessStatusCode();
        }
    }
}
