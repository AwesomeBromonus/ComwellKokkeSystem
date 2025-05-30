using Microsoft.JSInterop;
using Modeller;
using System.Text.Json;

// @* KLASSE: Implementerer IUserStateService og håndterer den aktuelle brugers tilstand i applikationen *@
public class UserStateService : IUserStateService
{
    private readonly IJSRuntime _js;               // JavaScript runtime til at tilgå browserens localStorage
    private const string StorageKey = "userState"; // Nøgle til at gemme brugerdata i localStorage

    // Den aktuelle bruger, hvis nogen er logget ind; ellers null
    public UserModel? CurrentUser { get; private set; }

    // Returnerer true hvis der er en bruger logget ind (CurrentUser ikke null)
    public bool IsLoggedIn => CurrentUser != null;

    // Angiver om loginstatus er kontrolleret og initialiseret
    public bool IsLoggedInChecked { get; private set; }

    // Event der udsendes, når brugerens tilstand ændres – bruges af UI til at opdatere sig
    public event Action? OnChange;

    // Konstruktor modtager IJSRuntime til at interagere med JavaScript i browseren
    public UserStateService(IJSRuntime js)
    {
        _js = js;
    }

    // @* Initialiserer brugerens tilstand asynkront, ved at læse data fra browserens localStorage *@
    public async Task InitializeAsync()
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", StorageKey);
        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                // Deserialiser JSON-streng til UserModel objekt
                var user = JsonSerializer.Deserialize<UserModel>(json);
                if (user != null)
                {
                    CurrentUser = user;
                }
            }
            catch
            {
                // Fejlhåndtering kan tilføjes her, fx logning
            }
        }

        // Marker at loginstatus nu er kontrolleret
        IsLoggedInChecked = true;

        // Notificer abonnenter om tilstandsændring
        NotifyStateChanged();
    }

    // @* Sætter den aktuelle bruger og gemmer brugerdata i localStorage som JSON *@
    public async Task SetUserAsync(UserModel user)
    {
        CurrentUser = user;
        IsLoggedInChecked = true;

        var json = JsonSerializer.Serialize(user);
        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);

        NotifyStateChanged();
    }

    // @* Logger brugeren ud ved at nulstille CurrentUser og fjerne data fra localStorage *@
    public async Task LogoutAsync()
    {
        CurrentUser = null;
        IsLoggedInChecked = true;

        await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);

        NotifyStateChanged();
    }

    // @* Returnerer true, hvis den aktuelle bruger har den angivne rolle (case-insensitive) *@
    public bool IsInRole(string role)
    {
        return CurrentUser?.Role?.Equals(role, StringComparison.OrdinalIgnoreCase) == true;
    }

    // @* Hjælpefunktion til at udløse OnChange-eventet *@
    private void NotifyStateChanged() => OnChange?.Invoke();

    // @* Hjælpeegenskaber til hurtig adgang til brugerens Id og rolle *@
    public int? Id => CurrentUser?.Id;
    public string? Role => CurrentUser?.Role;
}
