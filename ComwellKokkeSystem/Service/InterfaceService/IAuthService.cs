using Modeller;

// Interface der definerer de vigtigste autentifikations- og brugerrelaterede operationer i systemet
public interface IAuthService
{
    // Forsøger at logge en bruger ind baseret på oplysningerne i UserModel.
    // Returnerer true hvis login lykkes, ellers false.
    Task<bool> Login(UserModel login);

    // Henter en bruger baseret på brugernavn. Returnerer UserModel hvis fundet, ellers null.
    Task<UserModel?> GetUserByUsername(string username);

    // Registrerer en ny bruger i systemet med oplysninger fra UserModel.
    // Returnerer true hvis registreringen lykkes, ellers false.
    Task<bool> Register(UserModel user);

    // Logger den aktuelle bruger ud.
    Task Logout();

    // Henter id'et for den aktuelle loggede bruger.
    // Returnerer id som int eller null hvis ingen er logget ind.
    Task<int?> GetCurrentUserIdAsync();

    // Henter rollen for den aktuelle loggede bruger som tekst, fx "admin" eller "elev".
    Task<string?> GetCurrentUserRoleAsync();

    // Henter en liste over brugere, der enten er administratorer eller kokke.
    Task<List<UserModel>> GetAdminsOgKokkeAsync();
}
