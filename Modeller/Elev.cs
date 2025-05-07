using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeller
{
    public class Elev : User
    {
        public Elevplan? Uddannelsesplan { get; set; }

        public int? UddannelsesplanId { get; set; } // FK hvis du bruger EF Core
        public string Rolle { get; set; } = "Elev";
    }
}