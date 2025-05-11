public class UserState
{
    public string? Username { get; private set; }
    public string? Role { get; private set; }
    public int? Id { get; private set; } // 👈 Tilføjet

    public bool IsLoggedIn => !string.IsNullOrEmpty(Username);
    public bool IsLoggedInChecked { get; private set; } = false;

    public event Action? OnChange;

    public void SetUser(string username, string role, int id)
    {
        Username = username;
        Role = role;
        Id = id;
        IsLoggedInChecked = true;
        NotifyStateChanged();
    }

    public void Logout()
    {
        Username = null;
        Role = null;
        Id = null;
        IsLoggedInChecked = true;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
