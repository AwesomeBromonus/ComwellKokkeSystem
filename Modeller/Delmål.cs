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
        public string Beskrivelse { get; set; } = string.Empty;
        public string Ansvarlig { get; set; } = string.Empty;
        public string Igangsætter { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Kommentar { get; set; } = string.Empty;

        public int ElevplanId { get; set; }
    }
}
