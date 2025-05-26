using System;
using System.Collections.Generic;
using System.Linq;

namespace Modeller
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string Navn { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        // samlet progress for alle delmål og underdelmål 
        // Vi beregner disse dynamisk baseret på DelmaalList
        public int DelmaalDone
        {
            get
            {
                // Tæller alle underdelmål, der er "Fuldført"
                return DelmaalList
                    .Sum(dm => dm.UnderdelmaalList
                        .Count(ud => ud.Status == "Fuldført"));
            }
        }

        public int DelmaalTotal
        {
            get
            {
                // Tæller det samlede antal underdelmål for alle delmål
                return DelmaalList.Sum(dm => dm.UnderdelmaalList.Count);
            }
        }

        // Proceten beregnes ud fra det totale antal underdelmål og de fuldførte
        public int DelmaalPercent => DelmaalTotal > 0 ? (DelmaalDone * 100) / DelmaalTotal : 0;
        public List<Delmål> DelmaalList { get; set; } = new();
        public bool IsSelected { get; set; }
        public Praktikperiode? Praktikperiode { get; set; } = new();

        // Egenskab for flitering af delmål efter praktikperiode
        public int SelectedPraktikperiodeId { get; set; } = 0; // 0 for "alle praktikperioder" 

        public int DelmaalDoneForSelectedPeriod
        {
            get
            {
                if (SelectedPraktikperiodeId == 0) return DelmaalDone; // alle praktikperioder

                return DelmaalList
                    .Where(dm => dm.PraktikperiodeId == SelectedPraktikperiodeId)
                    .Sum(dm => dm.UnderdelmaalList.Count(ud => ud.Status == "Fuldført"));
            }
        }

        public int DelmaalTotalForSelectedPeriod
        {
            get
            {
                
                    if (SelectedPraktikperiodeId == 0) return DelmaalTotal; // Alle perioder

                    return DelmaalList
                        .Where(dm => dm.PraktikperiodeId == SelectedPraktikperiodeId)
                        .Sum(dm => dm.UnderdelmaalList.Count);
            }
        }

        public int DelmaalPercentForSelectedPeriod =>
            DelmaalTotalForSelectedPeriod > 0
                ? (DelmaalDoneForSelectedPeriod * 100) / DelmaalTotalForSelectedPeriod
                : 0;
        
        public List<Praktikperiode> Praktikperioder { get; set; } = new();
        public int? ElevplanId { get; set; }
    }
}
    
 
