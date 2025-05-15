using Modeller;

public interface IDelmaalService
{
    Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId);
    Task<List<Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId);
    Task UpdateDelmaalAsync(Delmål delmaal);
    Task AddDelmaalAsync(Delmål delmaal);                 // Tilføj delmål
    Task DeleteDelmaalAsync(int id);                      // Slet delmål
    Task<Delmål?> GetByIdAsync(int id);                   // Hent enkelt delmål (bruges i controller og modal hvis nødvendigt)
}
