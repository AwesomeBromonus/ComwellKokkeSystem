using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;


    public interface IPraktikperiodeService
    {
        Task<List<Praktikperiode>> GetAllAsync();
        Task<Praktikperiode?> GetByIdAsync(int id);


        Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId);

        Task<List<Praktikperiode>> GetByElevIdAsync(int elevId);


    }

