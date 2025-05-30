using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    public interface IPraktikperiode
    {
        // Hent alle praktikperioder
        Task<List<Praktikperiode>> GetAllAsync();

        // Hent én praktikperiode
        Task<Praktikperiode?> GetByIdAsync(int id);

        // Opret ny praktikperiode
        Task AddAsync(Praktikperiode periode);

        // Opdater eksisterende praktikperiode
        Task UpdateAsync(Praktikperiode periode);

        // Slet en praktikperiode
        Task DeleteAsync(int id);

        Task<List<Praktikperiode>> GetByElevIdAsync(int elevId); // <-- Add this line!

        // Hent alle praktikperioder tilknyttet en specifik elevplan
        Task<List<Praktikperiode>> GetByElevplanIdAsync(int elevplanId);
    }
}
