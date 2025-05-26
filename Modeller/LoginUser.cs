using MongoDB.Bson.Serialization.Attributes;

namespace Modeller
{
    [BsonIgnoreExtraElements]
    public class UserModel
    {
        [BsonId]
        public int Id { get; set; }

        public string Email { get; set; } = "";         // ✅ Bruges som login-identitet
        public string Password { get; set; } = "";      // Kodeord (ikke hashed)
        public string Role { get; set; } = "";          // "elev", "admin", etc.

        public string Navn { get; set; } = "";
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
        public string Email { get; set; } = "";         // ✅ Email bruges i stedet for Username
        public string Password { get; set; } = "";
    }

    public class RegisterModel
    {
        public string Email { get; set; } = "";         // ✅ Email bruges ved oprettelse
        public string Password { get; set; } = "";
        public string Role { get; set; } = "";
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";         // ✅ Returneres efter login
        public string Role { get; set; } = "";
        public int? HotelId { get; set; }
        public int? ElevplanId { get; set; }
        public string Navn { get; set; } = "";
    }
}
