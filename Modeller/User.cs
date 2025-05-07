namespace Modeller;

public abstract class User
{
    public int Id { get; set; }
    public string Navn { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Brugernavn { get; set; } = string.Empty;
    public string Adgangskode { get; set; } = string.Empty;
    public string Tlf { get; set; }
    public string Adresse { get; set; }
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }
}
