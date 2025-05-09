using Modeller;


    public interface IPraktikperiodeService
    {
        Task<List<Praktikperiode>> GetAllAsync();
        Task<Praktikperiode?> GetByIdAsync(int id);
    Task UpdateDelmålAsync(int praktikperiodeId, int delmålId, string nyStatus);

}

