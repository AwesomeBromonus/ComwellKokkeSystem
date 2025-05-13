using Modeller;

// Definerer de operationer, der kan udføres på beskeder fra client-siden
public interface IBeskedService
{
    // Hent alle beskeder
    Task<List<Besked>> GetBeskederAsync();

    // Hent én specifik besked ud fra ID
    Task<Besked?> GetBeskedByIdAsync(int id);

    // Tilføj ny besked
    Task AddBeskedAsync(Besked besked);

    // Opdater eksisterende besked
    Task UpdateBeskedAsync(Besked besked);

    // Slet besked ud fra ID
    Task DeleteBeskedAsync(int id);
    Task<List<Besked>> GetByUserIdAsync(int userId);
}