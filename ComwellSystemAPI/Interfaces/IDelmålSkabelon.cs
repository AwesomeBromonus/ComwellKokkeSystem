using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // Interface der definerer metoder til håndtering af delmålsskabeloner i databasen
    public interface IDelmaalSkabelon
    {
        // Henter alle delmålsskabeloner som en liste
        Task<List<DelmaalSkabelon>> GetAllAsync();

        // Henter delmålsskabeloner for en bestemt praktikperiode baseret på praktikperiodens nummer
        Task<List<DelmaalSkabelon>> GetByPraktikperiodeNrAsync(int praktikperiodeNr);

        // Henter en enkelt delmålsskabelon baseret på dens unikke id
        Task<DelmaalSkabelon?> GetByIdAsync(int id);

        // Tilføjer en ny delmålsskabelon til databasen
        Task AddAsync(DelmaalSkabelon model);

        // Opdaterer en eksisterende delmålsskabelon
        Task UpdateAsync(DelmaalSkabelon model);

        // Sletter en delmålsskabelon baseret på id
        Task DeleteAsync(int id);
    }
}
