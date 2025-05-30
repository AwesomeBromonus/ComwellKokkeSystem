using Modeller;
using System.Net.Http.Json;

// @* KLASSE: Serviceklasse der implementerer IDelmaalService til håndtering af delmål via HTTP-requests *@
public class DelmaalService : IDelmaalService
{
    private readonly HttpClient _http;

    public DelmaalService(HttpClient http)
    {
        _http = http;
    }

    // @* Henter delmål til en given praktikperiode via GET *@
    public async Task<List<Delmål>?> GetByPraktikperiodeIdAsync(int praktikperiodeId)
    {
        return await _http.GetFromJsonAsync<List<Delmål>>($"api/delmaal/praktikperiode/{praktikperiodeId}");
    }

    // @* Henter delmål for en given elevplan og praktikperiode via GET *@
    public async Task<List<Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId)
    {
        return await _http.GetFromJsonAsync<List<Delmål>>($"api/delmaal/elevplan/{elevplanId}/praktikperiode/{praktikperiodeId}");
    }

    // @* Henter et delmål efter id via GET *@
    public async Task<Delmål?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Delmål?>($"api/delmaal/{id}");
    }

    // @* Tilføjer et nyt delmål via POST *@
    public async Task AddDelmaalAsync(Delmål delmaal)
    {
        await _http.PostAsJsonAsync("api/delmaal", delmaal);
    }

    // @* Opdaterer et eksisterende delmål via PUT *@
    public async Task UpdateDelmaalAsync(Delmål delmaal)
    {
        await _http.PutAsJsonAsync($"api/delmaal/{delmaal.Id}", delmaal);
    }

    // @* Henter delmål baseret på elevplan-id via GET *@
    public async Task<List<Delmål>> GetByElevplanIdAsync(int elevId)
    {
        // Implementer logik til at hente delmål baseret på elevplan-id (hvis relevant)
        return await _http.GetFromJsonAsync<List<Delmål>>($"api/delmaal/elevplan/{elevId}") ?? new List<Delmål>();
    }

    // @* Henter delmål for en specifik elev via GET *@
    public async Task<List<Delmål>> GetDelmålForElevAsync(int elevId)
    {
        // Implementer logik til at hente delmål for en specifik elev
        return await _http.GetFromJsonAsync<List<Delmål>>($"api/delmaal/elev/{elevId}") ?? new List<Delmål>();
    }

    // @* Sletter et delmål efter id via DELETE *@
    public async Task DeleteDelmaalAsync(int id)
    {
        await _http.DeleteAsync($"api/delmaal/{id}");
    }

    // @* Henter alle delmål via GET *@
    public async Task<List<Delmål>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<Delmål>>("api/delmaal/all");
    }

    // @* Henter delmål med deadline inden for 14 dage via GET *@
    public async Task<List<Delmål>> GetDelmaalMedDeadlineIndenFor14DageAsync()
    {
        return await _http.GetFromJsonAsync<List<Delmål>>("api/delmaal/deadlines-14dage") ?? new List<Delmål>();
    }
}
