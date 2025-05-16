using Modeller;

public interface IDelmaalService
{
    Task<List<Delmål>?> GetByPraktikperiodeIdAsync(int praktikperiodeId);
    Task<List<Delmål>?> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId);
    Task UpdateDelmaalAsync(Delmål delmaal);
    Task<List<Delmål>> GetByElevplanIdAsync(int elevId);
    Task<List<Delmål>> GetDelmålForElevAsync(int elevId);
}

