using System.Net.Http.Json;
using Modeller;
using Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// Implementering af ILæringService, der bruger HttpClient til at kommunikere med API'et.
/// </summary>
public class LæringService : ILæringService
{
    // HttpClient bruges til at sende HTTP-forespørgsler til backend API'et.
    private readonly HttpClient _http;

    // Gør HttpClient tilgængelig, f.eks. til tests eller videreudvidelser
    public HttpClient Http => _http;

    /// <summary>
    /// Konstruktør der modtager en HttpClient via dependency injection.
    /// </summary>
    /// <param name="http">En konfigureret HttpClient instans.</param>
    public LæringService(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Henter alle læringsmaterialer fra API'et.
    /// </summary>
    /// <returns>En liste med læringsmaterialer. Returnerer tom liste hvis intet findes.</returns>
    public async Task<List<Læring>> HentAlleAsync() =>
        await _http.GetFromJsonAsync<List<Læring>>("api/laering") ?? new();

    /// <summary>
    /// Henter et specifikt læringsmateriale baseret på ID.
    /// </summary>
    /// <param name="id">ID på læringsmaterialet.</param>
    /// <returns>Et Læring-objekt eller null hvis det ikke findes.</returns>
    public async Task<Læring?> HentVedIdAsync(int id) =>
        await _http.GetFromJsonAsync<Læring>($"api/laering/{id}");

    /// <summary>
    /// Tilføjer et læringsmateriale til databasen via API'et.
    /// </summary>
    /// <param name="læring">Det læringsmateriale der skal gemmes.</param>
    public async Task TilføjAsync(Læring læring) =>
        await _http.PostAsJsonAsync("api/laering", læring);

    /// <summary>
    /// Uploader et læringsmateriale samt tilhørende fil via multipart/form-data.
    /// </summary>
    /// <param name="læring">Objekt med titel og beskrivelse.</param>
    /// <param name="fil">Den fil der skal uploades (maks 20 MB).</param>
    /// <returns>True hvis upload lykkes, ellers false.</returns>
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