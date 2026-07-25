using System.Text.Json;
using liquidlabs_assignment.Models;

namespace liquidlabs_assignment.Repositories;

public interface ICountriesRepository
{
    Task<IEnumerable<Country>> GetAllAsync();
    Task<Country?> GetByNameAsync(string name);
    // Task<IEnumerable<Country>> GetByContinentAsync(string continent);

}