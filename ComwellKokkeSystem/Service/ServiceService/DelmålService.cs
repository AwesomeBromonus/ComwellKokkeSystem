using Modeller;
using System.Net.Http.Json;

public class DelmaalService : IDelmaalService
{
    private readonly HttpClient _http;

    public DelmaalService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Delmål>?> GetByPraktikperiodeIdAsync(int praktikperiodeId)
    {
        return await _http.GetFromJsonAsync<List<Delmål>>($"api/delmaal/praktikperiode/{praktikperiodeId}");
    }

    public async Task<List<Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId)
    {
        return await _http.GetFromJsonAsync<List<Delmål>>($"api/delmaal/elevplan/{elevplanId}/praktikperiode/{praktikperiodeId}");
    }

    public async Task<Delmål?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Delmål?>($"api/delmaal/{id}");
    }

    public async Task AddDelmaalAsync(Delmål delmaal)
    {
        await _http.PostAsJsonAsync("api/delmaal", delmaal);
    }

    public async Task UpdateDelmaalAsync(Delmål delmaal)
    {
        await _http.PutAsJsonAsync($"api/delmaal/{delmaal.Id}", delmaal);
    }
    public async Task<List<Delmål>> GetByElevplanIdAsync(int elevId)
    {
        // Implementer logik til at hente delmål baseret på elevplan-id (hvis relevant)
        return await _http.GetFromJsonAsync<List<Delmål>>($"api/delmaal/elevplan/{elevId}") ?? new List<Delmål>();
    }

    public async Task<List<Delmål>> GetDelmålForElevAsync(int elevId)
    {
        // Implementer logik til at hente delmål for en specifik elev
        return await _http.GetFromJsonAsync<List<Delmål>>($"api/delmaal/elev/{elevId}") ?? new List<Delmål>();
    }
    

    public async Task DeleteDelmaalAsync(int id)
    {
        await _http.DeleteAsync($"api/delmaal/{id}");
    }
    public async Task<List<Delmål>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<Delmål>>("api/delmaal/all");
    }
    public async Task<List<Delmål>> GetDelmaalMedDeadlineIndenFor14DageAsync()
    {
        return await _http.GetFromJsonAsync<List<Delmål>>("api/delmaal/deadlines-14dage") ?? new List<Delmål>();
    }


}

