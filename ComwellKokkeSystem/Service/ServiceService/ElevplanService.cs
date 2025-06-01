using Modeller;
using System.Net.Http.Json;

namespace ComwellKokkeSystem.Service.Elev
{
    // @* KLASSE: Serviceklasse der implementerer IElevplanService og håndterer elevplaner via HTTP-requests *@
    public class ElevplanService : IElevplanService
    {
        private readonly HttpClient _http;

    //Konstruktor
    public ElevplanService(HttpClient http)
    {
        _http = http;
    }

        // @* Henter alle elevplaner via GET *@
        public async Task<List<Elevplan>?> GetElevplanerAsync() =>
            await _http.GetFromJsonAsync<List<Elevplan>>("api/elevplan");

        // @* Henter elevplan efter id via GET *@
        public async Task<Elevplan?> GetElevplanByIdAsync(int id) =>
            await _http.GetFromJsonAsync<Elevplan>($"api/elevplan/{id}");

   

    public async Task AddElevplanAsync(Elevplan plan) =>
        await _http.PostAsJsonAsync("api/elevplan", plan);

        // @* Opdaterer en eksisterende elevplan via PUT *@
        public async Task UpdateElevplanAsync(Elevplan plan) =>
            await _http.PutAsJsonAsync($"api/elevplan/{plan.Id}", plan);

    public async Task DeleteElevplanAsync(int id) =>
        await _http.DeleteAsync($"api/elevplan/{id}");

    public async Task<Elevplan?> GetElevplanForElevAsync(int elevId)
    {
        return await _http.GetFromJsonAsync<Elevplan>($"api/elevplan/elev/{elevId}");
    }




}


