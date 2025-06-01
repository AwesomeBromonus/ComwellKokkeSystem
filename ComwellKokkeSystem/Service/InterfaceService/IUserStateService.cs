using Modeller;

// Interface der definerer tilstanden for den aktuelle bruger i applikationen
public interface IUserStateService
{
    // Indeholder oplysninger om den aktuelle bruger, eller null hvis ingen er logget ind
    UserModel? CurrentUser { get; }

    // Id for den aktuelle bruger, praktisk at have hurtigt adgang til
    int? Id { get; }

    // Rollen som den aktuelle bruger har, fx "admin", "elev" eller "kok"
    string? Role { get; }

    // Angiver om der er en bruger logget ind (true hvis ja)
    bool IsLoggedIn { get; }

    // Angiver om loginstatus er blevet kontrolleret/initialiseret
    bool IsLoggedInChecked { get; }

    // En event, der kan abonneres på for at blive notificeret når brugerens tilstand ændres
    event Action? OnChange;

    // Initialiserer brugerens tilstand asynkront, fx ved opstart af applikationen
    Task InitializeAsync();

    // Sætter den aktuelle bruger og opdaterer tilstanden asynkront
    Task SetUserAsync(UserModel user);

    // Logger brugeren ud asynkront, og rydder tilstand
    Task LogoutAsync();

    // Returnerer true hvis den aktuelle bruger har den angivne rolle (case-insensitive)
    bool IsInRole(string role);
}
