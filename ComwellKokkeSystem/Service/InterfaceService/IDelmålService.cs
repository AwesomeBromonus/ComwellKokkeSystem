using Modeller;

public interface IDelmaalService
{
    //Finder praktikperiode id, og tager delmål udfra den praltikperiode
    Task<List<Delmål>> GetByPraktikperiodeIdAsync(int praktikperiodeId); 
    Task UpdateDelmaalAsync(Delmål delmaal);                           
    Task AddDelmaalAsync(Delmål delmaal);                               
    Task DeleteDelmaalAsync(int id);                                  
    Task<Delmål?> GetByIdAsync(int id);                                
    Task<List<Delmål>> GetAllAsync();                                   
    Task<List<Delmål>> GetDelmaalMedDeadlineIndenFor14DageAsync();       
}
