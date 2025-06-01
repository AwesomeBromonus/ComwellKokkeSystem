using Modeller;

namespace ComwellKokkeSystem.Service;

    public interface IElevplanService
    {
        // Hent alle elevplaner (typisk til admin)
        Task<List<Elevplan>?> GetElevplanerAsync();

        // Hent �n specifik elevplan ud fra ID
        Task<Elevplan?> GetElevplanByIdAsync(int id);

        // Hent elevplaner for en bestemt elev
        Task<Elevplan> GetElevplanForElevAsync(int elevId);

        // Tilføj ny elevplan
        Task AddElevplanAsync(Elevplan plan);

        // Opdater eksisterende elevplan
        Task UpdateElevplanAsync(Elevplan plan);

        // Slet elevplan ud fra ID
        Task DeleteElevplanAsync(int id);


}
