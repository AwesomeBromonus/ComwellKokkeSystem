using System.Net.Http.Json;
using Modeller;
using Microsoft.AspNetCore.Components.Forms;

public class LæringService : ILæringService
{
    private readonly HttpClient _http;
    public HttpClient Http => _http;

    public LæringService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Læring>> HentAlleAsync() =>
        await _http.GetFromJsonAsync<List<Læring>>("api/laering") ?? new();

    public async Task<Læring?> HentVedIdAsync(int id) =>
        await _http.GetFromJsonAsync<Læring>($"api/laering/{id}");

    public async Task TilføjAsync(Læring læring) =>
        await _http.PostAsJsonAsync("api/laering", læring);

    public async Task<bool> UploadAsync(Læring læring, IBrowserFile fil)
    {
        using var stream = fil.OpenReadStream(20 * 1024 * 1024); // max 20MB

        var content = new MultipartFormDataContent
        {
            { new StringContent(læring.Titel), "titel" },
            { new StringContent(læring.Beskrivelse), "beskrivelse" },
            { new StreamContent(stream), "file", fil.Name }
        };

        var response = await _http.PostAsync("api/laering/upload", content);
        return response.IsSuccessStatusCode;
    }
}
