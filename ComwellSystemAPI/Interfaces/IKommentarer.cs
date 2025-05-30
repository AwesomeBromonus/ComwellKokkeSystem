using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // Interface der definerer metoder til håndtering af kommentarer i systemet
    public interface IKommentar
    {
        // Tilføjer en ny kommentar til databasen
        Task AddAsync(Kommentar kommentar);

        // Henter alle kommentarer, som er knyttet til et bestemt delmål baseret på delmål id
        Task<List<Kommentar>> GetByDelmålIdAsync(int delmålId);
    }
}
