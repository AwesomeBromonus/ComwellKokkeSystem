using Modeller;
using System.Net.Http.Json;

// AuthService implementerer IAuthService og håndterer autentifikation og brugerdata
public class AuthService : IAuthService
{
    private readonly HttpClient _http;                    // HttpClient til at sende HTTP-requests til backend
    private readonly IUserStateService _userState;        // Service til at opdatere og læse brugerens tilstand i appen

    // Konstruktor injicerer HttpClient og IUserStateService
    public AuthService(HttpClient http, IUserStateService userState)
    {
        _http = http;
        _userState = userState;
    }

    // Forsøger login ved at sende brugerdata til backend
    // Hvis login lykkes, opdateres brugerens tilstand i appen
    public async Task<bool> Login(UserModel login)
    {
        var response = await _http.PostAsJsonAsync("api/users/login", login);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<UserModel>();
            if (result != null)
            {
                await _userState.SetUserAsync(result);
                return true;
            }
        }

        return false;
    }

    // Henter brugerdata baseret på brugernavn via GET request til backend
    public async Task<UserModel?> GetUserByUsername(string username)
    {
        return await _http.GetFromJsonAsync<UserModel>($"api/users/{username}");
    }

    // Registrerer en ny bruger ved at sende brugerdata til backend
    // Returnerer true hvis registreringen lykkes, ellers false
    public async Task<bool> Register(UserModel user)
    { 
        {
            var response = await _http.PostAsJsonAsync("api/users/register", user);
            return response.IsSuccessStatusCode;
        }
    }

    // Returnerer brugerens id fra lokal tilstand asynkront
    public Task<int?> GetCurrentUserIdAsync()
        => Task.FromResult(_userState.CurrentUser?.Id);

    // Returnerer brugerens rolle fra lokal tilstand asynkront
    public Task<string?> GetCurrentUserRoleAsync()
        => Task.FromResult(_userState.CurrentUser?.Role);

    // Logger brugeren ud ved at rydde lokal tilstand
    public async Task Logout()
    {
        await _userState.LogoutAsync();
    }

    // Henter listen over brugere med rolle admin eller kok via GET request til backend
    public async Task<List<UserModel>> GetAdminsOgKokkeAsync()
    {
        return await _http.GetFromJsonAsync<List<UserModel>>("api/users/admins-og-kokke");
    }
}
