using ComwellKokkeSystem.Service;
using Modeller;
using System.Net.Http.Json;

// @* KLASSE: Serviceklasse der implementerer IUnderdelmaalService og håndterer underdelmål via HTTP-requests *@
public class UnderdelmaalService : IUnderdelmaalService
{
    private readonly HttpClient _http;

    public UnderdelmaalService(HttpClient http)
    {
        _http = http;
    }

    // @* Tilføjer et nyt underdelmål via POST *@
    public async Task AddAsync(Underdelmaal underdelmaal)
    {
        await _http.PostAsJsonAsync("api/underdelmaal", underdelmaal);
    }

    // @* Opdaterer et eksisterende underdelmål via PUT *@
    public async Task UpdateAsync(Underdelmaal underdelmaal)
    {
        await _http.PutAsJsonAsync($"api/underdelmaal/{underdelmaal.Id}", underdelmaal);
    }

    // @* Henter alle underdelmål for et givent delmål via GET *@
    public async Task<List<Underdelmaal>> GetByDelmaalIdAsync(int delmaalId)
    {
        return await _http.GetFromJsonAsync<List<Underdelmaal>>($"api/underdelmaal/delmaal/{delmaalId}")
               ?? new List<Underdelmaal>();
    }

    // @* Henter et underdelmål efter id via GET *@
    public async Task<Underdelmaal?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Underdelmaal>($"api/underdelmaal/{id}");
    }

    // @* Opdaterer status for et underdelmål via PUT *@
    public async Task UpdateStatusAsync(int underdelmaalId, string nyStatus)
    {
        await _http.PutAsJsonAsync($"api/underdelmaal/status/{underdelmaalId}", nyStatus);
    }

    // @* Sletter et underdelmål efter id via DELETE *@
    public async Task DeleteAsync(int id)
    {
        await _http.DeleteAsync($"api/underdelmaal/{id}");
    }
}
