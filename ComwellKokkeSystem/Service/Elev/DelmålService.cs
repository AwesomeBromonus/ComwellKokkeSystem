using Modeller;
using System.Net.Http.Json;

public class DelmaalService : IDelmaalService
{
    private readonly HttpClient _http;

    public DelmaalService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId)
    {
        return await _http.GetFromJsonAsync<List<Delmål>>($"api/delmaal/praktikperiode/{praktikperiodeId}");
    }
    public async Task UpdateDelmaalAsync(Delmål delmaal)
    {
        await _http.PutAsJsonAsync($"api/delmaal/{delmaal.Id}", delmaal);
    }
    public async Task<List<Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId)
    {
        return await _http.GetFromJsonAsync<List<Delmål>>($"api/delmaal/elevplan/{elevplanId}/praktikperiode/{praktikperiodeId}");
    }



}
