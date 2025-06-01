using Modeller;

// Interface der definerer metoder til håndtering af brugerdata i databasen
public interface IUserRepository
{
    // Tilføjer en ny bruger til databasen
    Task AddAsync(UserModel user);

    // Henter en liste over alle brugere i systemet
    Task<List<UserModel>> GetAllAsync();

    // Henter en bruger baseret på brugernavn
    Task<UserModel?> GetByUsernameAsync(string username);

    // Henter en bruger baseret på id
    Task<UserModel?> GetByIdAsync(int id);

    // Sletter en bruger baseret på id
    Task DeleteAsync(int id);

    // Validerer om brugernavn og adgangskode matcher en bruger i databasen
    Task<bool> ValidateLogin(string username, string password);

    // Opdaterer en brugers oplysninger
    Task UpdateUserAsync(UserModel bruger);

    // Tildeler en elevplan til en bruger baseret på bruger-id og elevplan-id
    Task AssignElevplanToUserAsync(int userId, int elevplanId);

    // Henter en liste over brugere med rollerne admin og kok
    Task<List<UserModel>> GetAdminsOgKokkeAsync();
}
