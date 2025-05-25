using Modeller;
using System.Net.Http.Json;

namespace ComwellKokkeSystem.Service.Elev;

public class ElevplanService : IElevplanService
{
    private readonly HttpClient _http;

    public ElevplanService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Elevplan>?> GetElevplanerAsync() =>
        await _http.GetFromJsonAsync<List<Elevplan>>("api/elevplan");

    public async Task<Elevplan?> GetElevplanByIdAsync(int id) =>
        await _http.GetFromJsonAsync<Elevplan>($"api/elevplan/{id}");

    public async Task<List<Elevplan>?> GetElevplanerForElevAsync(int elevId) =>
        await _http.GetFromJsonAsync<List<Elevplan>>($"api/elevplan/elev/{elevId}");

    public async Task AddElevplanAsync(Elevplan plan) =>
        await _http.PostAsJsonAsync("api/elevplan", plan);

    public async Task UpdateElevplanAsync(Elevplan plan) =>
        await _http.PutAsJsonAsync($"api/elevplan/{plan.Id}", plan);

    public async Task DeleteElevplanAsync(int id) =>
        await _http.DeleteAsync($"api/elevplan/{id}");
}


