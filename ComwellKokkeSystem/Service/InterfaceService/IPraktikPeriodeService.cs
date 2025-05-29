using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPraktikperiodeService
{
    Task<List<Praktikperiode>> GetAllAsync();
    Task<Praktikperiode?> GetByIdAsync(int id);

    Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId); // evt. kan slettes
    Task<List<Praktikperiode>> GetByElevIdAsync(int elevId);
    Task<List<Praktikperiode>> GetByElevplanIdAsync(int elevplanId);

    Task AddAsync(Praktikperiode periode); // <- Tilføjet denne
    Task UpdateAsync(Praktikperiode periode);
}
