using System.Text.Json;
using liquidlabs_assignment.Models;

namespace liquidlabs_assignment.Services;

public interface ICountriesService
{
    Task<IEnumerable<Country>> GetAllAsync();
    Task<Country?> GetByNameAsync(string name);
    Task<IEnumerable<Country>> GetByContinentAsync(string continent);
}