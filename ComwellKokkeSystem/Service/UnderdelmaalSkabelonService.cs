using ComwellKokkeSystem.Service;
using Modeller;
using System.Net.Http.Json;

public class UnderdelmaalSkabelonService : IUnderdelmaalSkabelonService
{
    private readonly HttpClient _http;

    public UnderdelmaalSkabelonService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<UnderdelmaalSkabelon>> GetByDelmaalSkabelonIdAsync(int delmaalSkabelonId)
    {
        return await _http.GetFromJsonAsync<List<UnderdelmaalSkabelon>>($"api/underdelmaalskabelon/delmaalskabelon/{delmaalSkabelonId}")
               ?? new List<UnderdelmaalSkabelon>();
    }

    public async Task AddAsync(UnderdelmaalSkabelon skabelon)
    {
        await _http.PostAsJsonAsync("api/underdelmaalskabelon", skabelon);
    }

    public async Task DeleteAsync(int id)
    {
        await _http.DeleteAsync($"api/underdelmaalskabelon/{id}");
    }
    public async Task UpdateAsync(UnderdelmaalSkabelon skabelon)
    {
        await _http.PutAsJsonAsync($"api/underdelmaalskabelon/{skabelon.Id}", skabelon);
    }

}
