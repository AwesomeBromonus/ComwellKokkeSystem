using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    public interface IDelmål
    {
        Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId);
        Task<List<Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId);
        Task<List<Delmål>> GetByElevIdAsync(int elevId); // Til elevens overbliksside
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

