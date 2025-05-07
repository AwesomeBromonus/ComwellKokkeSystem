using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeller;
public class Hotel
{
    public int Id { get; set; }
    public string Navn { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string By { get; set; } = string.Empty;

    public List<Elev> Elev { get; set; } = new();
    public List<Kokke> Kokke{ get; set; } = new();
    public List<Admin> Admins { get; set; } = new();
}