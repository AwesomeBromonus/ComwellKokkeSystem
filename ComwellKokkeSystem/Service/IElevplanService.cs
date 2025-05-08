using Modeller;

// Definerer de operationer, der kan udf�res p� elevplaner fra client-siden
public interface IElevplanService
{
    // Hent alle elevplaner
    Task<List<Elevplan>> GetElevplanerAsync();

    // Hent �n specifik elevplan ud fra ID
    Task<Elevplan?> GetElevplanByIdAsync(int id);

    // Tilf�j ny elevplan
    Task AddElevplanAsync(Elevplan plan);

    // Opdater eksisterende elevplan
    Task UpdateElevplanAsync(Elevplan plan);

    // Slet elevplan ud fra ID
    Task DeleteElevplanAsync(int id);
}

