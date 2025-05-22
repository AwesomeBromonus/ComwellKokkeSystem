using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Modeller
{
    public class Delmål
    {
        public int Id { get; set; }
        public int PraktikperiodeId { get; set; }  // Reference til praktikperiode
        public string Beskrivelse { get; set; } = "";
        public string Ansvarlig { get; set; } = "";
        public string Igangsætter { get; set; } = "";
        public DateTime Deadline { get; set; }
        public string Status { get; set; } = ""; // Fx "Fuldført" eller "Ikke fuldført"
        public string Kommentar { get; set; } = "";
        public string Elevtur { get; set; } = "";
        public int ElevplanId { get; set; }
        public int ElevId { get; set; }
        public int? DelmaalSkabelonId { get; set; } 
        
        // Liste af underdelmål tilknyttet til dette delmål
        public List<Underdelmaal> UnderdelmaalList { get; set; } = new();
        
        // Beregn status baseret på underdelmål 
        public string CalculatedStatus
        {
            get
            {
                if (!UnderdelmaalList.Any()) return "Ikke påbegyndt";
                if (UnderdelmaalList.All(u => u.Status == "Fuldført")) return "Fuldført";
                if (UnderdelmaalList.Any(u => u.Status == "I gang")) return "I gang";
                return "Ikke fuldført"; 
            }
        }


    }

}
