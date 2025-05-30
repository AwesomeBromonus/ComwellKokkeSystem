using System.Net.Http.Json;
using Modeller;

// @* KLASSE: Serviceklasse der implementerer IUnderdelmaalSkabelonService og håndterer underdelmålsskabeloner via HTTP-requests *@
public class UnderdelmaalSkabelonService : IUnderdelmaalSkabelonService
{
    private readonly HttpClient _http;

    public UnderdelmaalSkabelonService(HttpClient http)
    {
        _http = http;
    }

    // @* Henter underdelmålsskabeloner for et givent delmålsskabelon-id via GET *@
    public async Task<List<UnderdelmaalSkabelon>> GetByDelmaalSkabelonIdAsync(int delmaalSkabelonId)
    {
        return await _http.GetFromJsonAsync<List<UnderdelmaalSkabelon>>(
            $"api/underdelmaalskabelon/delmaalskabelon/{delmaalSkabelonId}") ?? new();
    }

    // @* Tilføjer en ny underdelmålsskabelon via POST *@
    public async Task AddAsync(UnderdelmaalSkabelon underdelmaal)
    {
        await _http.PostAsJsonAsync("api/underdelmaalskabelon", underdelmaal);
    }

    // @* Opdaterer en eksisterende underdelmålsskabelon via PUT *@
    public async Task UpdateAsync(UnderdelmaalSkabelon underdelmaal)
    {
        await _http.PutAsJsonAsync($"api/underdelmaalskabelon/{underdelmaal.Id}", underdelmaal);
    }

    // @* Sletter en underdelmålsskabelon efter id via DELETE *@
    public async Task DeleteAsync(int id)
    {
        await _http.DeleteAsync($"api/underdelmaalskabelon/{id}");
    }
}
