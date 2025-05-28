using Modeller;
using Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// Interface for en service, der håndterer læringsmaterialer. 
/// Definerer funktioner til hentning, oprettelse og upload.
/// </summary>
public interface ILæringService
{
    /// <summary>
    /// Henter alle læringsmaterialer fra databasen.
    /// </summary>
    /// <returns>En liste af Læring-objekter.</returns>
    Task<List<Læring>> HentAlleAsync();

    /// <summary>
    /// Henter et enkelt læringsmateriale baseret på ID.
    /// </summary>
    /// <param name="id">ID for det ønskede læringsmateriale.</param>
    /// <returns>Et Læring-objekt eller null, hvis det ikke findes.</returns>
    Task<Læring?> HentVedIdAsync(int id);

    /// <summary>
    /// Tilføjer et nyt læringsmateriale til databasen.
    /// </summary>
    /// <param name="laering">Det læringsmateriale der skal tilføjes.</param>
    Task TilføjAsync(Læring laering);

    /// <summary>
    /// Uploader et læringsmateriale sammen med en tilhørende fil.
    /// </summary>
    /// <param name="læring">Objekt med titel og beskrivelse.</param>
    /// <param name="fil">Den valgte fil til upload.</param>
    /// <returns>True hvis upload lykkes, ellers false.</returns>
    Task<bool> UploadAsync(Læring læring, IBrowserFile fil);
}
