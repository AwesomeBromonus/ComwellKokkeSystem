using Modeller;
using System.Net.Http.Json;
using System.Security.Claims;

namespace ComwellKokkeSystem.Service
{
    // @* KLASSE: Serviceklasse der implementerer IUserService og håndterer brugerdata via HTTP-requests *@
    public class UserService : IUserService
    {
        private readonly HttpClient _http;

        // Konstruktoren modtager en HttpClient, som bruges til at sende HTTP-forespørgsler
        public UserService(HttpClient http)
        {
            _http = http;
        }

        // @* Henter en enkelt bruger baseret på brugerens unikke id via GET request *@
        public async Task<UserModel?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<UserModel>($"api/users/byid/{id}");
        }

        // @* Henter en liste over alle brugere i systemet via GET request *@
        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<UserModel>>("api/users/all");
        }

        // @* Henter en bruger baseret på brugernavn via GET request *@
        public async Task<UserModel?> GetByUsernameAsync(string username)
        {
            return await _http.GetFromJsonAsync<UserModel>($"api/users/{username}");
        }

        // @* Validerer login ved at sende brugernavn og kode til serveren via POST request.
        // Returnerer true hvis login er succesfuldt, ellers false *@
        public async Task<bool> ValidateLoginAsync(string username, string password)
        {
            var response = await _http.PostAsJsonAsync("api/users/login", new { username, password });
            return response.IsSuccessStatusCode;
        }

        // @* Henter praktikperioder for en given elev via GET request *@
        public async Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId)
        {
            return await _http.GetFromJsonAsync<List<Praktikperiode>>($"api/praktikperiode/elev/{elevId}");
        }

        // @* Sletter en bruger med det angivne id via DELETE request *@
        public async Task DeleteAsync(int id)
        {
            await _http.DeleteAsync($"api/users/{id}");
        }

        // @* Tildeler en elevplan til en bruger ved at sende PUT request med elevplan id i kroppen *@
        public async Task AssignElevplanToUserAsync(int userId, int elevplanId)
        {
            await _http.PutAsJsonAsync($"api/users/{userId}/assign-elevplan", elevplanId);
        }

        // @* Henter en liste over brugere med rollen admin eller kok via GET request *@
        public async Task<List<UserModel>> GetAdminsOgKokkeAsync()
        {
            return await _http.GetFromJsonAsync<List<UserModel>>("api/users/admins-og-kokke");
        }

        // @* Opdaterer brugerdata ved at sende PUT request med brugerobjekt i kroppen *@
        public async Task UpdateUserAsync(UserModel bruger)
        {
            await _http.PutAsJsonAsync($"api/users/{bruger.Id}", bruger);
        }

        // @* Skifter adgangskode for bruger via PUT request, sikrer at kaldet lykkes *@
        public async Task SkiftAdgangskodeAsync(int id, string nyKode)
        {
            var response = await _http.PutAsJsonAsync($"api/users/{id}/skiftkode", nyKode);
            response.EnsureSuccessStatusCode();
        }

        // @* Upload af profilbillede via POST request med multipart/form-data *@
        public async Task<bool> UploadProfilbilledeAsync(int id, Stream stream)
        {
            var content = new MultipartFormDataContent
            {
                { new StreamContent(stream), "file", $"{id}.jpg" }
            };

            var response = await _http.PostAsync($"api/users/{id}/upload-billede", content);
            return response.IsSuccessStatusCode;
        }
    }
}
