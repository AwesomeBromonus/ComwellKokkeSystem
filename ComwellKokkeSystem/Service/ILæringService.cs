using Modeller;
using Microsoft.AspNetCore.Components.Forms;

//
// Dette interface definerer, hvad en LæringService skal kunne gøre.
//
public interface ILæringService
{
    // Henter alle læringsmaterialer fra databasen
    Task<List<Læring>> HentAlleAsync();

    // Henter et specifikt læringsmateriale baseret på ID
    Task<Læring?> HentVedIdAsync(int id);

    // Tilføjer et nyt læringsmateriale til databasen
    Task TilføjAsync(Læring laering);

    // Upload af læringsmateriale med fil
    Task<bool> UploadAsync(Læring læring, IBrowserFile fil);
}
