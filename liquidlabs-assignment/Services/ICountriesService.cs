using System.Text.Json;
using liquidlabs_assignment.Models;

namespace liquidlabs_assignment.Services;

public interface ICountriesService
{
    Task<IEnumerable<Country>> GetAllAsync();
    Task<IEnumerable<Country>> GetByContinentAsync(string continent);
    Task<Country?> GetByCountryAsync(string name);
}