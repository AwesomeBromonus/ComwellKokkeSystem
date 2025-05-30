using System.Net.Http.Json;
using Modeller;
using Microsoft.AspNetCore.Components.Forms;

// @* KLASSE: Serviceklasse der implementerer ILæringService og håndterer læringsmaterialer via HTTP-requests *@
public class LæringService : ILæringService
{
    // HttpClient bruges til at sende HTTP-forespørgsler til backend API'et.
    private readonly HttpClient _http;

    // Gør HttpClient tilgængelig, f.eks. til tests eller videreudvidelser
    public HttpClient Http => _http;

    // Konstruktor der modtager en HttpClient via dependency injection.
    public LæringService(HttpClient http)
    {
        _http = http;
    }

    // Henter alle læringsmaterialer fra API'et.
    public async Task<List<Læring>> HentAlleAsync() =>
        await _http.GetFromJsonAsync<List<Læring>>("api/laering") ?? new();

    // Henter et specifikt læringsmateriale baseret på ID.
    public async Task<Læring?> HentVedIdAsync(int id) =>
        await _http.GetFromJsonAsync<Læring>($"api/laering/{id}");

    // Tilføjer et læringsmateriale til databasen via API'et.
    public async Task TilføjAsync(Læring læring) =>
        await _http.PostAsJsonAsync("api/laering", læring);

    // Uploader et læringsmateriale samt tilhørende fil via multipart/form-data.
    // Maksimal filstørrelse 20MB.
    public async Task<bool> UploadAsync(Læring læring, IBrowserFile fil)
    {
        using var stream = fil.OpenReadStream(20 * 1024 * 1024); // maks 20MB

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
