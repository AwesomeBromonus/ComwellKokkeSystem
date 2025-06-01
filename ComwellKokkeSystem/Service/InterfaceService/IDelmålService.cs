using Modeller;

public interface IDelmaalService
{
    Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId); // ✅ behold
    Task UpdateDelmaalAsync(Delmål delmaal);                             // ✅ behold
    Task AddDelmaalAsync(Delmål delmaal);                                // ✅ behold
    Task DeleteDelmaalAsync(int id);                                     // ✅ behold
    Task<Delmål?> GetByIdAsync(int id);                                  // ✅ behold
    Task<List<Delmål>> GetAllAsync();                                    // ✅ behold
    Task<List<Delmål>> GetDelmaalMedDeadlineIndenFor14DageAsync();       // ✅ behold
}
