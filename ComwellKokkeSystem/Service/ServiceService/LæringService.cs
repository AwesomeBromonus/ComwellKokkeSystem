using System.Net.Http.Json;
using Modeller;

public class LæringService : ILæringService
{
    private readonly HttpClient _http;
    public HttpClient Http => _http;
    public LæringService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Læring>> HentAlleAsync() =>
        await _http.GetFromJsonAsync<List<Læring>>("api/laering");

    public async Task<Læring?> HentVedIdAsync(int id) =>
        await _http.GetFromJsonAsync<Læring>($"api/laering/{id}");

    public async Task TilføjAsync(Læring læring) =>
        await _http.PostAsJsonAsync("api/laering", læring);
}