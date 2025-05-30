using Modeller;
using System.Net.Http;
using System.Net.Http.Json;

// @* KLASSE: Serviceklasse der implementerer IKommentarService til håndtering af kommentarer via HTTP-requests *@
public class KommentarService : IKommentarService
{
    private readonly HttpClient _http;

    public KommentarService(HttpClient http)
    {
        _http = http;
    }

    // @* Tilføjer en kommentar via POST *@
    public async Task AddKommentarAsync(Kommentar kommentar)
    {
        await _http.PostAsJsonAsync("api/kommentar", kommentar);
    }

    // @* Henter kommentarer for et specifikt delmål via GET *@
    public async Task<List<Kommentar>> GetByDelmålIdAsync(int delmålId)
    {
        return await _http.GetFromJsonAsync<List<Kommentar>>($"api/kommentar/delmål/{delmålId}")
               ?? new List<Kommentar>();
    }
}
