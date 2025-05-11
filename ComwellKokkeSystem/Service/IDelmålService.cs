using Modeller;

public interface IDelmaalService
{
    Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId);
    Task UpdateDelmaalAsync(Delmål delmaal);

}

