using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // @* KLASSE: Interface der definerer metoder til håndtering af kommentarer i systemet *@
    public interface IKommentar
    {
        // @* METODE: Tilføjer en ny kommentar til databasen *@
        Task AddAsync(Kommentar kommentar);

        // @* METODE: Henter alle kommentarer knyttet til et specifikt delmål via delmålId *@
        Task<List<Kommentar>> GetByDelmålIdAsync(int delmålId);
    }
}
