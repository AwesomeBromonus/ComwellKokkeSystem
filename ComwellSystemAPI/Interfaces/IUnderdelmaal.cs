using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // Interface der definerer metoder til håndtering af underdelmål i systemet
    public interface IUnderdelmaal
    {
        // Tilføjer et nyt underdelmål til databasen
        Task AddAsync(Underdelmaal underdelmaal);

        // Opdaterer et eksisterende underdelmål
        Task UpdateAsync(Underdelmaal underdelmaal);

        // Henter en liste af underdelmål tilknyttet et bestemt delmål via delmål id
        Task<List<Underdelmaal>> GetByDelmaalIdAsync(int delmaalId);

        // Henter et enkelt underdelmål baseret på dets unikke id
        Task<Underdelmaal?> GetByIdAsync(int id);

        // Opdaterer kun statusfeltet for et underdelmål, fx "fuldført" eller "igangværende"
        Task UpdateStatusAsync(int underdelmaalId, string nyStatus);

        // Sletter et underdelmål baseret på id
        Task DeleteAsync(int id);
    }
}
