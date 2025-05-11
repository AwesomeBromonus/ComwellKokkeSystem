namespace Modeller;

public class Bruger
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = ""; // fx "admin", "elev"
    public string Navn { get; set; } = "";
    public string Email { get; set; } = "";
    public string Tlf { get; set; } = "";
    public string Adresse { get; set; } = "";
    public int HotelId { get; set; } // reference til hotel
    public DateTime StartDato { get; set; }
    public int? UddannelsesplanId { get; set; } // nullable hvis null i DB
}
