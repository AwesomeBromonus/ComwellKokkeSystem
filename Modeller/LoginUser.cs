
namespace Modeller
{
    // Repræsenterer en bruger i systemet (fx elev, kok, HR)
    public class UserModel
    {
        // Unikt ID for brugeren (genereres automatisk af databasen)
        public int Id { get; set; }

        // Brugernavn som bruges til login
        public string Username { get; set; }

        // Adgangskode til login (bør hashes før den gemmes)
        public string Password { get; set; }

        // Rolle i systemet: fx "Elev", "Kok", "HR", "Admin"
        public string Role { get; set; }
    }

    // Bruges ved login: indeholder brugernavn og adgangskode
    public class LoginModel
    {
        // Brugernavn til login
        public string Username { get; set; }

        // Adgangskode til login
        public string Password { get; set; }
    }

    // Bruges til at oprette en ny bruger i systemet
    public class RegisterModel
    {
        // Det ønskede brugernavn
        public string Username { get; set; }

        // Adgangskode valgt af brugeren
        public string Password { get; set; }

        // Den rolle brugeren skal have, fx "Elev"
        public string Role { get; set; }
    }
}
