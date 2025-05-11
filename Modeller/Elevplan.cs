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
        
        public DateTime OprettetDato { get; set; } = DateTime.Now;
        public string Kommentar { get; set; } = string.Empty;

        public DateTime StartDato { get; set; }

        public DateTime SlutDato { get; set; }
        
        public int ElevId { get; set; }


        public List<int> PraktikperiodeIds { get; set; } = new();

    }
}

