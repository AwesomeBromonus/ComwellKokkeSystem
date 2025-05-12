using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    public interface IDelmål
    {
        Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId);
        Task UpdateDelmaalAsync(Delmål delmaal);

    }


}
