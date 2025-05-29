using Modeller;

public interface IUserStateService
{
    UserModel? CurrentUser { get; }
    int? Id { get; }                         // ← Tilføjet
    string? Role { get; }                   // ← Valgfri, men praktisk
    bool IsLoggedIn { get; }
    bool IsLoggedInChecked { get; }
    event Action? OnChange;

    Task InitializeAsync();
    Task SetUserAsync(UserModel user);
    Task LogoutAsync();
    bool IsInRole(string role);
}
