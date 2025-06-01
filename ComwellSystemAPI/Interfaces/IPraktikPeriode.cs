using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // @* KLASSE: Interface til håndtering af praktikperioder i systemet *@
    public interface IPraktikperiode
    {
        // @* METODE: Hent alle praktikperioder *@
        Task<List<Praktikperiode>> GetAllAsync();

        // @* METODE: Hent én praktikperiode baseret på id *@
        Task<Praktikperiode?> GetByIdAsync(int id);

        // @* METODE: Opret en ny praktikperiode *@
        Task AddAsync(Praktikperiode periode);

        // @* METODE: Opdater en eksisterende praktikperiode *@
        Task UpdateAsync(Praktikperiode periode);

        // @* METODE: Slet en praktikperiode baseret på id *@
        Task DeleteAsync(int id);


        // Hent alle praktikperioder tilknyttet en specifik elevplan
        Task<List<Praktikperiode>> GetByElevplanIdAsync(int elevplanId);
    }
}
