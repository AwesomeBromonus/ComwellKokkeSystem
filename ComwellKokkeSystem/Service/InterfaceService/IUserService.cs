using Modeller;

namespace ComwellKokkeSystem.Service
{
    // Interface der definerer de metoder, en bruger-service skal implementere
    public interface IUserService
    {
        // Henter en bruger baseret på brugerens unikke id.
        Task<UserModel?> GetByIdAsync(int id);

        // Henter alle brugere i systemet som en liste.
        Task<List<UserModel>> GetAllAsync();

        // Henter en bruger baseret på brugernavn.
        Task<UserModel?> GetByUsernameAsync(string username);

        // Validerer loginoplysninger, dvs. tjekker om brugernavn og adgangskode matcher.
        Task<bool> ValidateLoginAsync(string username, string password);

        // Henter en liste af praktikperioder tilknyttet en bestemt elev (via elevens id).
        Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId);

        // Sletter en bruger ud fra brugerens id.
        Task DeleteAsync(int id);

        // Tildeler en elevplan til en bruger, ved at opdatere brugerens elevplan-id.
        Task AssignElevplanToUserAsync(int userId, int elevplanId);

        // Henter en liste af brugere, som enten er administratorer eller kokke.
        Task<List<UserModel>> GetAdminsOgKokkeAsync();

        // Opdaterer oplysningerne for en bruger (fx navn, email, rolle mv.).
        Task UpdateUserAsync(UserModel bruger);
        Task SkiftAdgangskodeAsync(int id, string nyKode);

        // Upload af profilbillede til en bruger; modtager en stream med billeddata.
        Task<bool> UploadProfilbilledeAsync(int id, Stream stream);
        


    }
}
