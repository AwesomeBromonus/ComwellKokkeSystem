using System.Net.Http.Json;
using Modeller; // Indeholder LoginModel, RegisterModel og LoginResponse

// AuthService implementerer IAuthService (interface)
public class AuthService : IAuthService
{
    private readonly HttpClient _http;        // Bruger vi til at kalde API'en
    private readonly UserState _userState;    // Holder styr på hvem der er logget ind

    public AuthService(HttpClient http, UserState userState)
    {
        _http = http;
        _userState = userState;
    }

    // Kaldes når brugeren forsøger at logge ind
    public async Task<bool> Login(LoginModel login)
    {
        // Sender login-oplysninger som JSON til API
        var response = await _http.PostAsJsonAsync("api/users/login", login);

        if (response.IsSuccessStatusCode)
        {
            // Hvis login lykkes, læser vi svar-data (brugernavn og rolle)
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result != null)
            {
                // Vi gemmer brugerens oplysninger i UserState (global hukommelse)
                _userState.SetUser(result.Username, result.Role);
                return true;
            }
        }

        // Hvis noget fejler, returneres false
        return false;
    }

    // Kaldes når en ny bruger registreres
    public async Task<bool> Register(RegisterModel user)
    {
        var response = await _http.PostAsJsonAsync("api/users/register", user);
        return response.IsSuccessStatusCode;
    }

    // Kaldes når brugeren logger ud
    public Task Logout()
    {
        _userState.Logout(); // Vi nulstiller brugeren
        return Task.CompletedTask;
    }
}
