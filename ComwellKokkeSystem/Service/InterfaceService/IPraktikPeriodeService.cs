using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPraktikperiodeService
{
    Task<List<Praktikperiode>> GetAllAsync();
    Task<Praktikperiode?> GetByIdAsync(int id);

    Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId); // evt. kan slettes, da den er ens med næste
    Task<List<Praktikperiode>> GetByElevIdAsync(int elevId);

    Task<List<Praktikperiode>> GetByElevplanIdAsync(int elevplanId);
    Task UpdateAsync(Praktikperiode periode);

}
