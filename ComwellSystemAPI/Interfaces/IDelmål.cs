using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    public interface IDelmål
    {
        Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId); 
        Task<Delmål?> GetByIdAsync(int id);
        Task AddAsync(Delmål delmaal);                    // Opret nyt delmål
        Task UpdateDelmaalAsync(Delmål delmaal);          // Opdater hele delmålet
        Task UpdateStatusAsync(int delmaalId, string nyStatus); // Opdater kun status
        Task DeleteDelmaalAsync(int id);                  // Slet delmål
        Task<List<Delmål>> GetAllForYearAsync(int year);
        Task<List<Delmål>> GetAllAsync();
        Task<List<Delmål>> GetWithDeadlineWithinDaysAsync(int antalDage);


    }
}

