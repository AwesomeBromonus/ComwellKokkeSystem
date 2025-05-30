using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // Interface der definerer metoder til håndtering af underdelmålsskabeloner
    public interface IUnderdelmaalSkabelon
    {
        // Henter en liste af underdelmålsskabeloner tilknyttet et bestemt delmålsskabelon-id
        Task<List<UnderdelmaalSkabelon>> GetByDelmaalSkabelonIdAsync(int delmaalSkabelonId);

        // Tilføjer en ny underdelmålsskabelon til databasen
        Task AddAsync(UnderdelmaalSkabelon item);

        // Sletter en underdelmålsskabelon baseret på id
        Task DeleteAsync(int id);

        // Opdaterer en eksisterende underdelmålsskabelon med nye data
        Task UpdateAsync(UnderdelmaalSkabelon model);
    }
}
