using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeller
{
    public class Kommentar
    {
        public int Id { get; set; }                    // Kommentarens ID
        public int DelmålId { get; set; }              // 🔁 Bruger 'å' som resten af systemet
        public string Bruger { get; set; } = "";       // Navn eller rolle
        public string Indhold { get; set; } = "";      // Kommentarens indhold
        public DateTime Tidspunkt { get; set; } = DateTime.UtcNow;
    }
}
