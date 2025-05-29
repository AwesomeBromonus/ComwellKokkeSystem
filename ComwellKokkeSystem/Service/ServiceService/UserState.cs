using Microsoft.JSInterop;
using Modeller;
using System.Text.Json;

public class UserStateService : IUserStateService
{
    private readonly IJSRuntime _js;
    private const string StorageKey = "userState";

    public UserModel? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;
    public bool IsLoggedInChecked { get; private set; }

    public event Action? OnChange;

    public UserStateService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InitializeAsync()
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", StorageKey);
        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                var user = JsonSerializer.Deserialize<UserModel>(json);
                if (user != null)
                {
                    CurrentUser = user;
                }
            }
            catch { /* Log evt. fejl her */ }
        }

        IsLoggedInChecked = true;
        NotifyStateChanged();
    }

    public async Task SetUserAsync(UserModel user)
    {
        CurrentUser = user;
        IsLoggedInChecked = true;

        var json = JsonSerializer.Serialize(user);
        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);

        NotifyStateChanged();
    }

    public async Task LogoutAsync()
    {
        CurrentUser = null;
        IsLoggedInChecked = true;

        await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
        NotifyStateChanged();
    }

    public bool IsInRole(string role)
    {
        return CurrentUser?.Role?.Equals(role, StringComparison.OrdinalIgnoreCase) == true;
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
    public int? Id => CurrentUser?.Id;
    public string? Role => CurrentUser?.Role;

}
