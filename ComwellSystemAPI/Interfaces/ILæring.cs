using Modeller;
namespace Interface;
public interface ILæring
{
    Task<List<Læring>> GetAllAsync();
    Task<Læring?> GetByIdAsync(int id);
    Task AddAsync(Læring læring);
}