using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeller
{
    public class Praktikperiode
    {
        public int Id { get; set; }
        public string Navn { get; set; } = "";
        public DateTime StartDato { get; set; }
        public DateTime SlutDato { get; set; }
        public string Status { get; set; } = ""; // fx "Aktiv", "Ikke startet"
        public int ElevplanId { get; set; }
    }

}
