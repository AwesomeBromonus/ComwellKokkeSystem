using System.Net.Http;
using System.Net.Http.Json;
using Modeller;

// @* KLASSE: Serviceklasse der implementerer IDelmaalSkabelonService og håndterer delmålsskabeloner via HTTP-requests *@
public class DelmaalSkabelonService : IDelmaalSkabelonService
{
    private readonly HttpClient _http;

    public DelmaalSkabelonService(HttpClient http)
    {
        _http = http;
    }

    // @* Henter alle delmålsskabeloner via GET *@
    public async Task<List<DelmaalSkabelon>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<DelmaalSkabelon>>("api/delmaalskabelon") ?? new();
    }

    // @* Henter en delmålsskabelon efter id via GET *@
    public async Task<DelmaalSkabelon?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<DelmaalSkabelon>($"api/delmaalskabelon/{id}");
    }

    // @* Tilføjer en ny delmålsskabelon via POST *@
    public async Task AddAsync(DelmaalSkabelon skabelon)
    {
        await _http.PostAsJsonAsync("api/delmaalskabelon", skabelon);
    }

    // @* Opdaterer en eksisterende delmålsskabelon via PUT *@
    public async Task UpdateAsync(DelmaalSkabelon skabelon)
    {
        await _http.PutAsJsonAsync($"api/delmaalskabelon/{skabelon.Id}", skabelon);
    }

    // @* Sletter en delmålsskabelon efter id via DELETE *@
    public async Task DeleteAsync(int id)
    {
        await _http.DeleteAsync($"api/delmaalskabelon/{id}");
    }

    // @* Henter delmålsskabeloner for en given praktikperiode via GET *@
    public async Task<List<DelmaalSkabelon>> GetByPeriodeAsync(int nummer)
    {
        return await _http.GetFromJsonAsync<List<DelmaalSkabelon>>($"api/delmaalskabelon/periode/{nummer}");
    }
}
