using Modeller;
using System.Net.Http;
using System.Net.Http.Json;

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

    public async Task<List<Praktikperiode>> GetByElevIdAsync(int elevId) =>
        await _http.GetFromJsonAsync<List<Praktikperiode>>($"api/praktikperiode/elev/{elevId}");

    public async Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId) =>
        await GetByElevIdAsync(elevId); // Samme funktionalite
    // ✅ Den du skal bruge i din delmål-side
    public async Task<List<Praktikperiode>> GetByElevplanIdAsync(int elevplanId) =>
        await _http.GetFromJsonAsync<List<Praktikperiode>>($"api/praktikperiode/elevplan/{elevplanId}");
    public async Task UpdateAsync(Praktikperiode periode)
    {
        await _http.PutAsJsonAsync($"api/praktikperiode/{periode.Id}", periode);
    }
    public async Task AddAsync(Praktikperiode periode)
    {
        await _http.PostAsJsonAsync("api/praktikperiode", periode);
    }


}
