using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Modeller
{
    // Repræsenterer en bruger i systemet (fx elev, kok, HR)
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? MongoId { get; set; }  // MongoDB internt ID

        public int Id { get; set; }           // Bruges til at linke til elevplaner

        public string Username { get; set; } = "";

        public string Password { get; set; } = ""; // Bør hashes i produktion

        public string Role { get; set; } = "";     // "Elev", "Kok", "HR", osv.
    }

    // Bruges ved login: indeholder brugernavn og adgangskode
    public class LoginModel
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    // Bruges til at oprette en ny bruger i systemet
    public class RegisterModel
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "";
    }
}
