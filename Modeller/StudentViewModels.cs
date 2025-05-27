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
        public string Role { get; set; } // Tilføjet
        public string HotelNavn { get; set; } // Tilføjet

        public int DelmaalDone
        {
            get
            {
                return DelmaalList
                    .Sum(dm => dm.UnderdelmaalList
                        .Count(ud => ud.Status == "Fuldført"));
            }
        }

        public int DelmaalTotal
        {
            get
            {
                return DelmaalList.Sum(dm => dm.UnderdelmaalList.Count);
            }
        }

        public int DelmaalPercent => DelmaalTotal > 0 ? (DelmaalDone * 100) / DelmaalTotal : 0;
        public List<Delmål> DelmaalList { get; set; } = new();
        public bool IsSelected { get; set; }
        public Praktikperiode? Praktikperiode { get; set; } = new();

        public int SelectedPraktikperiodeId { get; set; } = 0;

        public int DelmaalDoneForSelectedPeriod
        {
            get
            {
                if (SelectedPraktikperiodeId == 0) return DelmaalDone;
                return DelmaalList
                    .Where(dm => dm.PraktikperiodeId == SelectedPraktikperiodeId)
                    .Sum(dm => dm.UnderdelmaalList.Count(ud => ud.Status == "Fuldført"));
            }
        }

        public int DelmaalTotalForSelectedPeriod
        {
            get
            {
                if (SelectedPraktikperiodeId == 0) return DelmaalTotal;
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