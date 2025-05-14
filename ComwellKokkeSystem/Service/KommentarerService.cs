using Modeller;
using System.Net.Http;
using System.Net.Http.Json;

public class KommentarService : IKommentarService
{
    private readonly HttpClient _http;

    public KommentarService(HttpClient http)
    {
        _http = http;
    }

    public async Task AddKommentarAsync(Kommentar kommentar)
    {
        await _http.PostAsJsonAsync("api/kommentar", kommentar);
    }

    public async Task<List<Kommentar>> GetByDelmålIdAsync(int delmålId)
    {
        return await _http.GetFromJsonAsync<List<Kommentar>>($"api/kommentar/delmål/{delmålId}")
               ?? new List<Kommentar>();
    }
}
