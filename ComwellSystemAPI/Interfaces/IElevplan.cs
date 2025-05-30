using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // @* KLASSE: Interface der definerer CRUD-operationer for elevplaner i databasen *@
    public interface IElevplan
    {
        // @* METODE: Henter alle elevplaner som liste *@
        Task<List<Elevplan>> GetAllAsync();

        // @* METODE: Henter en enkelt elevplan baseret på id *@
        Task<Elevplan?> GetByIdAsync(int id);

        // @* METODE: Henter alle elevplaner for en specifik elev via elevId *@
        Task<List<Elevplan>> GetByElevIdAsync(int elevId);

        // @* METODE: Tilføjer en ny elevplan til databasen *@
        Task AddAsync(Elevplan elevplan);

        // @* METODE: Opdaterer en eksisterende elevplan *@
        Task UpdateAsync(Elevplan elevplan);

        // @* METODE: Sletter en elevplan baseret på id *@
        Task DeleteAsync(int id);
    }
}
