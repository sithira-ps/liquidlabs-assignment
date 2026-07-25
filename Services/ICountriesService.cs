using System.Text.Json;
using liquidlabs_assignment.Models;

namespace liquidlabs_assignment.Services;

public interface ICountriesService
{
    Task<JsonElement> GetAllAsync();
    Task<JsonElement> GetByNameAsync();
    Task<JsonElement> GetByContinentAsync();
}