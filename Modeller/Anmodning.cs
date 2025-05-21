using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeller

    {
    public class Anmodning
    {
        public int Id { get; set; }
        public int? DelmaalId { get; set; }
        public int? UnderdelmaalId { get; set; } // ✅ NYT felt
        public int ElevId { get; set; }
        public int ModtagerId { get; set; }
        public string ØnsketStatus { get; set; } = "";
        public string Status { get; set; } = "Afventer";
        public DateTime Oprettet { get; set; } = DateTime.Now;
    }


}



