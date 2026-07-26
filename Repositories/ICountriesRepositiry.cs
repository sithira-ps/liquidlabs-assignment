using liquidlabs_assignment.Models;

namespace liquidlabs_assignment.Repositories;

public interface ICountriesRepository
{
    Task<IEnumerable<Country>> GetAllAsync();
    Task<Country?> GetByNameAsync(string name);
    Task<IEnumerable<Country>> GetByContinentAsync(string continent);
    Task<int> CreateBatchAsync(List<Country> countries);
    Task<bool> DeleteByContinentAsync(string continent);
    Task<bool> DeleteAllAsync();
}