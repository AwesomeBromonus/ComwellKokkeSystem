using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeller
{
    public class Elevplan
    {
        public int Id { get; set; }
        public string Titel { get; set; } = string.Empty;
        public DateTime OprettetDato { get; set; } = DateTime.Now;
        public string Kommentar { get; set; } = string.Empty;

        public DateTime StartDato { get; set; }

        public DateTime SlutDato { get; set; }
        public int PraktikperiodeId { get; set; } // Tilknytning til valgt praktikperiode
        public List<Praktikperiode> Praktikperiode { get; set; } = new();
    }
}

