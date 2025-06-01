using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // @* KLASSE: Interface der definerer CRUD-operationer for elevplaner i databasen *@
    public interface IElevplan
    {
        // @* METODE: Henter alle elevplaner som liste *@
        Task<List<Elevplan>> GetAllAsync();

        // @* METODE: Henter en enkelt elevplan baseret p� id *@
        Task<Elevplan?> GetByIdAsync(int id);
        Task<Elevplan> GetByElevIdAsync(int elevId);
        Task AddAsync(Elevplan elevplan);

        // @* METODE: Opdaterer en eksisterende elevplan *@
        Task UpdateAsync(Elevplan elevplan);

        // @* METODE: Sletter en elevplan baseret p� id *@
        Task DeleteAsync(int id);
    }
}
