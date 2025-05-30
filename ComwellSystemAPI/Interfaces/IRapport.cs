using System.Threading.Tasks;

namespace ComwellSystemAPI.Interfaces
{
    // @* KLASSE: Interface til rapportgenerering *@
    public interface IRapportRepository
    {
        // @* METODE: Generer Excel-rapport med elevers delmål, returnerer som byte-array *@
        Task<byte[]> GenererElevDelmaalExcelAsync();
    }
}
