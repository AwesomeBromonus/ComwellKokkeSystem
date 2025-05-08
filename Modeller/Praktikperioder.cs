using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeller
{
    public class Praktikperiode
    {
    public int Id { get; set; } // fx 1, 2, 3
    public string Navn { get; set; } = string.Empty;
    public List<Delmål> Delmål { get; set; } = new();
    

    }
}
