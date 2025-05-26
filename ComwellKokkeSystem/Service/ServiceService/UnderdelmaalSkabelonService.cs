using System.Net.Http.Json;
using Modeller;

public class UnderdelmaalSkabelonService : IUnderdelmaalSkabelonService
{
    private readonly HttpClient _http;

    public UnderdelmaalSkabelonService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<UnderdelmaalSkabelon>> GetByDelmaalSkabelonIdAsync(int delmaalSkabelonId)
    {
        return await _http.GetFromJsonAsync<List<UnderdelmaalSkabelon>>(
            $"api/underdelmaalskabelon/delmaalskabelon/{delmaalSkabelonId}") ?? new();
    }

    public async Task AddAsync(UnderdelmaalSkabelon underdelmaal)
    {
        await _http.PostAsJsonAsync("api/underdelmaalskabelon", underdelmaal);
    }

    public async Task UpdateAsync(UnderdelmaalSkabelon underdelmaal)
    {
        await _http.PutAsJsonAsync($"api/underdelmaalskabelon/{underdelmaal.Id}", underdelmaal);
    }

    public async Task DeleteAsync(int id)
    {
        await _http.DeleteAsync($"api/underdelmaalskabelon/{id}");
    }
}
