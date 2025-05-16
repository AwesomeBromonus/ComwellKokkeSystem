using Modeller;


    public interface IPraktikperiodeService
    {
        Task<List<Praktikperiode>> GetAllAsync();
        Task<Praktikperiode?> GetByIdAsync(int id);
 

}

