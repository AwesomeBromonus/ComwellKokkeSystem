using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // Interface der definerer de metoder, som en repository til delmål skal implementere
    public interface IDelmål
    {
        Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId); 
        Task<Delmål?> GetByIdAsync(int id);
        Task AddAsync(Delmål delmaal);                    // Opret nyt delmål
        Task UpdateDelmaalAsync(Delmål delmaal);          // Opdater hele delmålet
        Task UpdateStatusAsync(int delmaalId, string nyStatus); // Opdater kun status
        Task DeleteDelmaalAsync(int id);                  // Slet delmål
        Task<List<Delmål>> GetAllForYearAsync(int year);

        // Henter alle delmål i systemet
        Task<List<Delmål>> GetAllAsync();

        // Henter delmål med deadline inden for et givent antal dage; nyttigt til påmindelser og overblik
        Task<List<Delmål>> GetWithDeadlineWithinDaysAsync(int antalDage);
    }
}
