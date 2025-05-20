using ComwellKokkeSystem.Service;
using Modeller;
using System.Net.Http.Json;

public class UnderdelmaalService : IUnderdelmaalService
{
    private readonly HttpClient _http;

    public UnderdelmaalService(HttpClient http)
    {
        _http = http;
    }

    public async Task AddAsync(Underdelmaal underdelmaal)
    {
        await _http.PostAsJsonAsync("api/underdelmaal", underdelmaal);
    }

    public async Task UpdateAsync(Underdelmaal underdelmaal)
    {
        await _http.PutAsJsonAsync($"api/underdelmaal/{underdelmaal.Id}", underdelmaal);
    }

    public async Task<List<Underdelmaal>> GetByDelmaalIdAsync(int delmaalId)
    {
        return await _http.GetFromJsonAsync<List<Underdelmaal>>($"api/underdelmaal/delmaal/{delmaalId}")
               ?? new List<Underdelmaal>();
    }

    public async Task<Underdelmaal?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Underdelmaal>($"api/underdelmaal/{id}");
    }

    public async Task UpdateStatusAsync(int underdelmaalId, string nyStatus)
    {
        await _http.PutAsJsonAsync($"api/underdelmaal/status/{underdelmaalId}", nyStatus);
    }

    public async Task DeleteAsync(int id)
    {
        await _http.DeleteAsync($"api/underdelmaal/{id}");
    }
}
