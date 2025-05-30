using Modeller;
using System.Net.Http;
using System.Net.Http.Json;

// @* KLASSE: Serviceklasse der implementerer IPraktikperiodeService til håndtering af praktikperioder via HTTP-requests *@
public class PraktikperiodeService : IPraktikperiodeService
{
    private readonly HttpClient _http;

    public PraktikperiodeService(HttpClient http)
    {
        _http = http;
    }

    // @* Henter alle praktikperioder via GET *@
    public async Task<List<Praktikperiode>> GetAllAsync() =>
        await _http.GetFromJsonAsync<List<Praktikperiode>>("api/praktikperiode");

    // @* Henter en praktikperiode efter id via GET *@
    public async Task<Praktikperiode?> GetByIdAsync(int id) =>
        await _http.GetFromJsonAsync<Praktikperiode>($"api/praktikperiode/{id}");

    // @* Henter praktikperioder til en specifik elev via GET *@
    public async Task<List<Praktikperiode>> GetByElevIdAsync(int elevId) =>
        await _http.GetFromJsonAsync<List<Praktikperiode>>($"api/praktikperiode/elev/{elevId}");

    // @* Alias til GetByElevIdAsync - bruges fx i delmål-side *@
    public async Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId) =>
        await GetByElevIdAsync(elevId);

    // @* Henter praktikperioder til en specifik elevplan via GET *@
    public async Task<List<Praktikperiode>> GetByElevplanIdAsync(int elevplanId) =>
        await _http.GetFromJsonAsync<List<Praktikperiode>>($"api/praktikperiode/elevplan/{elevplanId}");

    // @* Opdaterer en praktikperiode via PUT *@
    public async Task UpdateAsync(Praktikperiode periode)
    {
        await _http.PutAsJsonAsync($"api/praktikperiode/{periode.Id}", periode);
    }

    // @* Tilføjer en ny praktikperiode via POST *@
    public async Task AddAsync(Praktikperiode periode)
    {
        await _http.PostAsJsonAsync("api/praktikperiode", periode);
    }
}
