using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // @* KLASSE: Interface for repository til håndtering af delmål *@
    public interface IDelmål
    {
        Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId); 
        Task<Delmål?> GetByIdAsync(int id);
        Task AddAsync(Delmål delmaal);                    // Opret nyt delmål
        Task UpdateDelmaalAsync(Delmål delmaal);          // Opdater hele delmålet
        Task UpdateStatusAsync(int delmaalId, string nyStatus); // Opdater kun status
        Task DeleteDelmaalAsync(int id);                  // Slet delmål
     

        // @* METODE: Henter alle delmål i systemet *@
        Task<List<Delmål>> GetAllAsync();

        // @* METODE: Henter delmål med deadline inden for et antal dage *@
        Task<List<Delmål>> GetWithDeadlineWithinDaysAsync(int antalDage);
    }
}
