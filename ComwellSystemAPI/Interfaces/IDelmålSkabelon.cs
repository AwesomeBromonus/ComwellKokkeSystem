using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // @* KLASSE: Interface til håndtering af delmålsskabeloner i databasen *@
    public interface IDelmaalSkabelon
    {
        // @* METODE: Henter alle delmålsskabeloner som liste *@
        Task<List<DelmaalSkabelon>> GetAllAsync();

        // @* METODE: Henter delmålsskabeloner for en bestemt praktikperiode (nummer) *@
        Task<List<DelmaalSkabelon>> GetByPraktikperiodeNrAsync(int praktikperiodeNr);

        // @* METODE: Henter en enkelt delmålsskabelon baseret på id *@
        Task<DelmaalSkabelon?> GetByIdAsync(int id);

        // @* METODE: Tilføjer en ny delmålsskabelon til databasen *@
        Task AddAsync(DelmaalSkabelon model);

        // @* METODE: Opdaterer en eksisterende delmålsskabelon *@
        Task UpdateAsync(DelmaalSkabelon model);

        // @* METODE: Sletter en delmålsskabelon baseret på id *@
        Task DeleteAsync(int id);
    }
}
