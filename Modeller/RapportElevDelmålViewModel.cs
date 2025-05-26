// RapportElevDelmålViewModel.cs
using System.Collections.Generic;
using System.Linq; // Til .Any()

namespace Modeller // Eller dit ViewModels namespace
{
    public class RapportElevDelmålViewModel
    {
        // Brugerinformation
        public int ElevId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string ElevNavn { get; set; } = string.Empty;
        public string Rolle { get; set; } = string.Empty;
        public string HotelNavn { get; set; } = string.Empty;

        // Delmål information
        public int DelmålId { get; set; }
        public string DelmålBeskrivelse { get; set; } = string.Empty;
        public string DelmålStatus { get; set; } = string.Empty; // Den rå status for hovedmålet
        public string DelmålCalculatedStatus { get; set; } = string.Empty; // Den beregnede status fra Delmål-modellen
        public string DelmålAnsvarlig { get; set; } = string.Empty;
        public string DelmålIgangsætter { get; set; } = string.Empty;
        public DateTime DelmålDeadline { get; set; }

        // Underdelmål - bruges til at beregne progress
        public List<Underdelmaal> UnderdelmaalList { get; set; } = new();

        // Praktikperiode information
        public int PraktikperiodeId { get; set; }
        public string PraktikperiodeNavn { get; set; } = string.Empty;

        // Progress for delmålet (beregnet i koden)
        public double DelmålProgressPercent { get; set; }
        public string DelmålProgressText { get; set; } = string.Empty;

        // Til afkrydsningsfelt i tabel
        public bool IsSelected { get; set; }
    }
}