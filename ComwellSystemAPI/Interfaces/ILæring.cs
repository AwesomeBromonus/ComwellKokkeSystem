using Modeller;

namespace Interface
{
    // @* KLASSE: Interface, der definerer metoder til håndtering af læringsmaterialer *@
    public interface ILæring
    {
        // @* METODE: Henter en liste over alle læringsmaterialer i systemet *@
        Task<List<Læring>> GetAllAsync();

        // @* METODE: Henter et enkelt læringsmateriale baseret på dets unikke id *@
        Task<Læring?> GetByIdAsync(int id);

        // @* METODE: Tilføjer et nyt læringsmateriale til databasen *@
        Task AddAsync(Læring læring);
    }
}
