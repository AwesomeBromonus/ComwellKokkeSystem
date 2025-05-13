using Modeller;

public interface IBesked
{
    // Henter alle beskeder
    Task<List<Besked>> GetAllAsync();

    // Henter en besked ud fra ID
    Task<Besked?> GetByIdAsync(int id);

    // Tilføjer en ny besked
    Task AddAsync(Besked besked);

    // Opdaterer en eksisterende besked
    Task UpdateAsync(Besked besked);

    // Sletter en besked ud fra ID
    Task DeleteAsync(int id);

    // Henter alle beskeder fra en specifik bruger
    Task<List<Besked>> GetByUserIdAsync(int userId);
}