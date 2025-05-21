using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeller
{
    public class UnderdelmaalSkabelon
    {
        public int Id { get; set; }
        public int DelmaalSkabelonId { get; set; } // Reference til tilhørende DelmaalSkabelon
        public string Beskrivelse { get; set; } = string.Empty;
        public int DeadlineOffsetDage { get; set; } = 0;
    }
}


