public class UserState
{
    public string? Username { get; private set; } // Navn på den loggede bruger
    public string? Role { get; private set; }     // Rolle: fx "admin" eller "elev"

    // Er brugeren logget ind? Vi tjekker om Username er sat
    public bool IsLoggedIn => !string.IsNullOrEmpty(Username);

    // Er login-status tjekket? Bruges til at vise "indlæser..." på sider
    public bool IsLoggedInChecked { get; private set; } = false;

    // Bruges til at give besked til komponenter, når der sker login/logout
    public event Action? OnChange;

    // Gemmer brugerens oplysninger og giver besked om ændring
    public void SetUser(string username, string role)
    {
        Username = username;
        Role = role;
        IsLoggedInChecked = true;
        NotifyStateChanged();
    }

    // Logger brugeren ud og nulstiller alt
    public void Logout()
    {
        Username = null;
        Role = null;
        IsLoggedInChecked = true;
        NotifyStateChanged();
    }

    // Denne metode kalder alle som har tilmeldt sig OnChange
    private void NotifyStateChanged() => OnChange?.Invoke();
}
