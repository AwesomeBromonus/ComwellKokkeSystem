using Modeller;

public interface IKommentarService
{
    Task AddKommentarAsync(Kommentar kommentar);
    Task<List<Kommentar>> GetByDelmålIdAsync(int delmålId);
}
