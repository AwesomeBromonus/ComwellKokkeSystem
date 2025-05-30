using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // @* KLASSE: Interface for repository til håndtering af delmål *@
    public interface IDelmål
    {
        // @* METODE: Henter delmål tilknyttet en specifik praktikperiode via id *@
        Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId);

        // @* METODE: Henter delmål for given elevplan og praktikperiode *@
        Task<List<Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId);

        // @* METODE: Henter alle delmål tilknyttet en specifik elev *@
        Task<List<Delmål>> GetByElevIdAsync(int elevId);

        // @* METODE: Henter et enkelt delmål baseret på id *@
        Task<Delmål?> GetByIdAsync(int id);

        // @* METODE: Opretter et nyt delmål i databasen *@
        Task AddAsync(Delmål delmaal);

        // @* METODE: Opdaterer hele delmålet med nye data *@
        Task UpdateDelmaalAsync(Delmål delmaal);

        // @* METODE: Opdaterer kun statusfeltet for et delmål *@
        Task UpdateStatusAsync(int delmaalId, string nyStatus);

        // @* METODE: Sletter et delmål baseret på id *@
        Task DeleteDelmaalAsync(int id);

        // @* METODE: Henter alle delmål for et bestemt år (fx til rapportering) *@
        Task<List<Delmål>> GetAllForYearAsync(int year);

        // @* METODE: Henter alle delmål i systemet *@
        Task<List<Delmål>> GetAllAsync();

        // @* METODE: Henter delmål med deadline inden for et antal dage *@
        Task<List<Delmål>> GetWithDeadlineWithinDaysAsync(int antalDage);
    }
}
