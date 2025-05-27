using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeller
{
    public class Læring
    {
        public int Id { get; set; }
        public string Titel { get; set; } = "";
        public string Beskrivelse { get; set; } = "";
        public string FilNavn { get; set; } = "";
        public string FilSti { get; set; } = ""; // F.eks. URL eller local path
        public string Filtype { get; set; } = ""; // f.eks. "pdf", "mp4", "jpg"
        public DateTime Oprettet { get; set; } = DateTime.UtcNow;
        public string? FilIndholdBase64 { get; set; }
    }
}