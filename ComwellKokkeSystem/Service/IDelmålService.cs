using Modeller;

public interface IDelmaalService
{
    Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId);
    Task<List<Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId);
    Task UpdateDelmaalAsync(Delmål delmaal);

}

