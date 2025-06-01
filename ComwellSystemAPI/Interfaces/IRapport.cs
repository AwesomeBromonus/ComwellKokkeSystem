using System.Threading.Tasks;

namespace ComwellSystemAPI.Interfaces
{
    public interface IRapportRepository
    {
        /// <summary>
        /// Genererer en Excel-rapport over alle elever og deres delmål.
        /// </summary>
        /// <returns>Excel-filen som byte-array (xlsx-format)</returns>
        Task<byte[]> GenererElevDelmaalExcelAsync();
    }
}
