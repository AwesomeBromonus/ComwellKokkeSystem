using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    public interface IDelmål
    {
        Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId);
        Task<List<Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId);
        Task<List<Delmål>> GetByElevIdAsync(int elevId); // Til elevens overbliksside
        Task<Delmål?> GetByIdAsync(int id);
        Task AddAsync(Delmål delmaal); // Bruges ved oprettelse
        Task UpdateDelmaalAsync(Delmål delmaal); // Bruges ved checkbox
        Task UpdateStatusAsync(int delmaalId, string nyStatus);

    }
}
