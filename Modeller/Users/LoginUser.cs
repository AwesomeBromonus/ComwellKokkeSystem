using MongoDB.Bson.Serialization.Attributes;



namespace Modeller
{
    [BsonIgnoreExtraElements]

    public class UserModel
    {
        [BsonId]
        public int Id { get; set; }

        public string Username { get; set; } = "";        // Brugernavn til login
        public string Password { get; set; } = "";        // Kodeord (ikke hashed)
        public string Role { get; set; } = "";            // "elev", "admin", etc.

        // 👇 Felter fra dine MongoDB-dokumenter
        public string Navn { get; set; } = "";
        public string Email { get; set; } = "";
        public string Tlf { get; set; } = "";
        public string Adresse { get; set; } = "";

        public int HotelId { get; set; }
        public DateTime StartDato { get; set; } = DateTime.UtcNow;
        public DateTime? SlutDato { get; set; }
        public int? ElevplanId { get; set; }
        public string? HotelNavn { get; set; }
        
    }

    public class LoginModel
    {
        public string Username { get; set; } = "";  
        public string Password { get; set; } = "";
    }

    public class RegisterModel
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "";
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";
        public int? HotelId { get; set; }
        public int? ElevplanId { get; set; }
        public string Navn { get; set; } = "";    
        public string Email { get; set; } = "";
    }


}
