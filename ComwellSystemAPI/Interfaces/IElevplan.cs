using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // Interface, der definerer CRUD-operationer for elevplaner i databasen
    public interface IElevplan
    {
        // Henter alle elevplaner i systemet som en liste
        Task<List<Elevplan>> GetAllAsync();

        // Henter en enkelt elevplan baseret p� dens unikke id
        Task<Elevplan?> GetByIdAsync(int id);

        // Henter alle elevplaner tilknyttet en specifik elev baseret p� elevens id
        Task<List<Elevplan>> GetByElevIdAsync(int elevId);

        // Tilf�jer en ny elevplan til databasen
        Task AddAsync(Elevplan elevplan);

        // Opdaterer en eksisterende elevplan med nye data
        Task UpdateAsync(Elevplan elevplan);

        // Sletter en elevplan baseret p� id
        Task DeleteAsync(int id);
    }
}
