using Modeller;

namespace Interface
{
    // Interface, der definerer metoder til håndtering af læringsmaterialer
    public interface ILæring
    {
        // Henter en liste over alle læringsmaterialer i systemet
        Task<List<Læring>> GetAllAsync();

        // Henter et enkelt læringsmateriale baseret på dets unikke id
        Task<Læring?> GetByIdAsync(int id);

        // Tilføjer et nyt læringsmateriale til databasen
        Task AddAsync(Læring læring);
    }
}
