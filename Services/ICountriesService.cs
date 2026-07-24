using liquidlabs_assignment.Models;

namespace liquidlabs_assignment.Services;

public interface ICountriesService
{
    Task<Country> GetAllAsync();
}