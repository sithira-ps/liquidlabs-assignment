
using liquidlabs_assignment.Models;

namespace liquidlabs_assignment.Services;

public class CountriesService : ICountriesService
{
    public async Task<Country> GetAllAsync()
    {
        return new Country
        {
            Id = 1,
            Name = "Sri Lanka",
            Region = "Asia"
        };
    }
}