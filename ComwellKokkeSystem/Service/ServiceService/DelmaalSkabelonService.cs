using System.Net.Http;
using System.Net.Http.Json;
using Modeller;

public class DelmaalSkabelonService : IDelmaalSkabelonService
{
    private readonly HttpClient _http;

    public DelmaalSkabelonService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DelmaalSkabelon>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<DelmaalSkabelon>>("api/delmaalskabelon") ?? new();
    }

    public async Task<DelmaalSkabelon?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<DelmaalSkabelon>($"api/delmaalskabelon/{id}");
    }

    public async Task AddAsync(DelmaalSkabelon skabelon)
    {
        await _http.PostAsJsonAsync("api/delmaalskabelon", skabelon);
    }

    public async Task UpdateAsync(DelmaalSkabelon skabelon)
    {
        await _http.PutAsJsonAsync($"api/delmaalskabelon/{skabelon.Id}", skabelon);
    }

    public async Task DeleteAsync(int id)
    {
        await _http.DeleteAsync($"api/delmaalskabelon/{id}");
    }
    public async Task<List<DelmaalSkabelon>> GetByPeriodeAsync(int nummer)
    {
        return await _http.GetFromJsonAsync<List<DelmaalSkabelon>>($"api/delmaalskabelon/periode/{nummer}");
    }

}
