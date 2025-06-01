using Modeller;

// Interface der definerer repository-funktioner til håndtering af anmodninger i systemet
public interface IAnmodningRepository
{
    // Opretter en ny anmodning i databasen
    Task OpretAsync(Anmodning anmodning);

    // Henter en specifik anmodning baseret på dens unikke id
    Task<Anmodning?> GetByIdAsync(int id);

    // Henter alle anmodninger, der er tilknyttet en bestemt modtager (typisk bruger)
    Task<List<Anmodning>> GetTilModtagerAsync(int modtagerId);

    // Opdaterer en eksisterende anmodning med nye data
    Task UpdateAsync(Anmodning anmodning);

    // Henter en komplet liste over alle anmodninger i systemet
    Task<List<Anmodning>> GetAlleAsync();

    // Behandler en anmodning ved at sætte den som accepteret eller afvist baseret på bool-flag
    Task BehandlAsync(int id, bool accepteret);
}
