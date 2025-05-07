using Modeller;
using System.Net.Http.Json;
namespace Service;

public class ElevplanService : IElevplanService
{
    private readonly HttpClient _http;

    // HttpClient injiceres fra Blazor og bruges til at kalde API'et
    public ElevplanService(HttpClient http)
    {
        _http = http;
    }

    // Hent alle elevplaner via GET-anmodning
    public async Task<List<Elevplan>> GetElevplanerAsync() =>
        await _http.GetFromJsonAsync<List<Elevplan>>("api/elevplan");

    // Hent én elevplan via GET /api/elevplan/{id}
    public async Task<Elevplan?> GetElevplanByIdAsync(int id) =>
        await _http.GetFromJsonAsync<Elevplan>($"api/elevplan/{id}");

    // Tilføj ny elevplan via POST
    public async Task AddElevplanAsync(Elevplan plan) =>
        await _http.PostAsJsonAsync("api/elevplan", plan);

    // Opdater eksisterende elevplan via PUT
    public async Task UpdateElevplanAsync(Elevplan plan) =>
        await _http.PutAsJsonAsync($"api/elevplan/{plan.Id}", plan);

    // Slet elevplan via DELETE
    public async Task DeleteElevplanAsync(int id) =>
        await _http.DeleteAsync($"api/elevplan/{id}");
}
