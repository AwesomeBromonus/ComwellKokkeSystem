using System;
using System.Collections.Generic;

namespace Modeller
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string Navn { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int DelmaalDone { get; set; }
        public int DelmaalTotal { get; set; }
        public int DelmaalPercent => DelmaalTotal > 0 ? (DelmaalDone * 100) / DelmaalTotal : 0;
        public List<Delmål> DelmaalList { get; set; } = new();
        public bool IsSelected { get; set; }
        public int? DaysToSchool { get; set; }
        public Praktikperiode Praktikperiode { get; set; }
        public int? ElevplanId { get; set; }

        public List<Praktikperiode> Praktikperioder { get; set; }
    }
    
 
}