using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    public interface IKommentar
    {
        Task AddAsync(Kommentar kommentar);
        Task<List<Kommentar>> GetByDelmålIdAsync(int delmålId);
    }
}
