using Microsoft.JSInterop;
using System.Text.Json;

public class UserState
{
    private readonly IJSRuntime _js;

    public string? Username { get; private set; }
    public string? Role { get; private set; }
    public int? Id { get; private set; }
    public int? HotelId { get; private set; }
    public int? ElevplanId { get; private set; }
    public string? Navn { get; private set; }
    public string? Email { get; private set; }

    public bool IsLoggedIn => !string.IsNullOrEmpty(Username);
    public bool IsLoggedInChecked { get; private set; } = false;

    public event Action? OnChange;

    public UserState(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InitializeAsync()
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", "userState");
        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                var data = JsonSerializer.Deserialize<UserData>(json);
                if (data != null)
                {
                    Username = data.Username;
                    Role = data.Role;
                    Id = data.Id;
                    HotelId = data.HotelId;
                    ElevplanId = data.ElevplanId;
                    Navn = data.Navn;
                    Email = data.Email;
                }
            }
            catch { }
        }

        IsLoggedInChecked = true;
        NotifyStateChanged();
    }

    public async Task SetUserAsync(string username, string role, int id, int? hotelId, int? elevplanId, string navn, string email)
    {
        Username = username;
        Role = role;
        Id = id;
        HotelId = hotelId;
        ElevplanId = elevplanId;
        Navn = navn;
        Email = email;
        IsLoggedInChecked = true;

        var json = JsonSerializer.Serialize(new UserData
        {
            Username = username,
            Role = role,
            Id = id,
            HotelId = hotelId,
            ElevplanId = elevplanId,
            Navn = navn,
            Email = email
        });

        await _js.InvokeVoidAsync("localStorage.setItem", "userState", json);
        NotifyStateChanged();
    }

    public async Task LogoutAsync()
    {
        Username = null;
        Role = null;
        Id = null;
        HotelId = null;
        ElevplanId = null;
        Navn = null;
        Email = null;
        IsLoggedInChecked = true;

        await _js.InvokeVoidAsync("localStorage.removeItem", "userState");
        NotifyStateChanged();
    }

    public bool IsElevAdgang =>
    Role?.ToLower() == "elev" || Role?.ToLower() == "kok";



    private void NotifyStateChanged() => OnChange?.Invoke();

    private class UserData
    {
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";
        public int Id { get; set; }
        public int? HotelId { get; set; }
        public int? ElevplanId { get; set; }
        public string Navn { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
