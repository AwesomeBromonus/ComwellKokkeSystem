using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeller
{
    public class DelmaalSkabelon
    {
        public int Id { get; set; }
        public int PraktikperiodeNummer { get; set; } // 1, 2 eller 3
        public string Beskrivelse { get; set; } = string.Empty;
        public string Ansvarlig { get; set; } = "Nærmeste leder";
        public string Igangsætter { get; set; } = "Nærmeste leder";
        public int DeadlineOffsetDage { get; set; } // fx 10 dage efter startdato
    }

}

