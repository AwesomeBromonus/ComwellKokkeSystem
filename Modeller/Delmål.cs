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

    }

}
