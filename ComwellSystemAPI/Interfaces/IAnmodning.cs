using Modeller;

// @* KLASSE: Interface til repository der håndterer anmodninger i systemet *@
public interface IAnmodningRepository
{
    // @* METODE: Opretter en ny anmodning i databasen *@
    Task OpretAsync(Anmodning anmodning);

    // @* METODE: Henter en specifik anmodning baseret på id *@
    Task<Anmodning?> GetByIdAsync(int id);

    // @* METODE: Henter alle anmodninger til en given modtager *@
    Task<List<Anmodning>> GetTilModtagerAsync(int modtagerId);

    // @* METODE: Opdaterer en eksisterende anmodning *@
    Task UpdateAsync(Anmodning anmodning);

    // @* METODE: Henter alle anmodninger i systemet *@
    Task<List<Anmodning>> GetAlleAsync();

    // @* METODE: Behandler en anmodning som accepteret eller afvist *@
    Task BehandlAsync(int id, bool accepteret);
}
