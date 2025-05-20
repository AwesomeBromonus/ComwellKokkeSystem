using Modeller;

public interface IAnmodningRepository
{
    Task OpretAsync(Anmodning anmodning);
    Task<Anmodning?> GetByIdAsync(int id);
    Task<List<Anmodning>> GetTilModtagerAsync(int modtagerId);
    Task UpdateAsync(Anmodning anmodning);
    Task<List<Anmodning>> GetAlleAsync(); // valgfri – hvis du vil vise alle anmodninger
    Task BehandlAsync(int id, bool accepteret);

}
