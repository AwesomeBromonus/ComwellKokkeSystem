using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // Interface, der definerer CRUD-operationer for elevplaner i databasen
    public interface IElevplan
    {
        // Henter alle elevplaner i systemet som en liste
        Task<List<Elevplan>> GetAllAsync();

        // Henter en enkelt elevplan baseret på dens unikke id
        Task<Elevplan?> GetByIdAsync(int id);
        Task<Elevplan> GetByElevIdAsync(int elevId);
        Task AddAsync(Elevplan elevplan);

        // Opdaterer en eksisterende elevplan med nye data
        Task UpdateAsync(Elevplan elevplan);

        // Sletter en elevplan baseret på id
        Task DeleteAsync(int id);
    }
}
