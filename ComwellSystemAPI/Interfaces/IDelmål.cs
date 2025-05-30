using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    // Interface der definerer de metoder, som en repository til delmål skal implementere
    public interface IDelmål
    {
        // Henter en liste af delmål tilknyttet en specifik praktikperiode via id
        Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId);

        // Henter delmål for en given elevplan og praktikperiode kombination
        Task<List<Delmål>> GetByElevplanIdAndPraktikperiodeIdAsync(int elevplanId, int praktikperiodeId);

        // Henter alle delmål tilknyttet en specifik elev; bruges fx på elevens overbliksside
        Task<List<Delmål>> GetByElevIdAsync(int elevId);

        // Henter et enkelt delmål baseret på id
        Task<Delmål?> GetByIdAsync(int id);

        // Opretter et nyt delmål i databasen
        Task AddAsync(Delmål delmaal);

        // Opdaterer hele delmålet med nye data
        Task UpdateDelmaalAsync(Delmål delmaal);

        // Opdaterer kun statusfeltet for et delmål, fx "fuldført" eller "under arbejde"
        Task UpdateStatusAsync(int delmaalId, string nyStatus);

        // Sletter et delmål baseret på id
        Task DeleteDelmaalAsync(int id);

        // Henter alle delmål for et bestemt år, fx for rapportering eller statistik
        Task<List<Delmål>> GetAllForYearAsync(int year);

        // Henter alle delmål i systemet
        Task<List<Delmål>> GetAllAsync();

        // Henter delmål med deadline inden for et givent antal dage; nyttigt til påmindelser og overblik
        Task<List<Delmål>> GetWithDeadlineWithinDaysAsync(int antalDage);
    }
}
