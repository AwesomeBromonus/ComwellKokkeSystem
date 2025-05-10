namespace Modeller
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Navn { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Brugernavn { get; set; } = string.Empty;
        public string Adgangskode { get; set; } = string.Empty;
        public string Tlf { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;

        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }
        public string Rolle { get; set; } = "bruger";
        public DateTime? StartDato { get; set; }
        public int? UddannelsesplanId { get; set; }
    }
}
