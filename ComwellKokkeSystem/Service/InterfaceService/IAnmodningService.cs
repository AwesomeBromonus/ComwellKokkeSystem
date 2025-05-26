using Modeller;

namespace ComwellKokkeSystem.Service
{
    public interface IAnmodningService
    {
        Task OpretAsync(Anmodning anmodning);
        Task<Anmodning?> GetByIdAsync(int id);
        Task<List<Anmodning>> GetTilModtagerAsync(int modtagerId);
        Task UpdateAsync(Anmodning anmodning);
        Task<List<Anmodning>> GetAlleAsync(); // valgfri – bruges f.eks. af admin
        Task BehandlAsync(int anmodningId, bool accepteret);

    }
}
