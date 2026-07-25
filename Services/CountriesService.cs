
using System.Text.Json;
using liquidlabs_assignment.Models;
using liquidlabs_assignment.Repositories;

namespace liquidlabs_assignment.Services;

public class CountriesService : ICountriesService
{
    private readonly IExternalApiService _externalApiService;
    private readonly ICountriesRepository _repository;

    public CountriesService(IExternalApiService externalApiService, ICountriesRepository repository)
    {
        _externalApiService = externalApiService;
        _repository = repository;
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        var countries = new List<Country>();
        var apiResponse = await _externalApiService.GetAllFromApiAsync(null, null);
        JsonElement countriesArray = apiResponse.GetProperty("data").GetProperty("objects"); // countries list is under data > objects > list
        foreach (JsonElement obj in countriesArray.EnumerateArray())
        {
            countries.Add(new Country
            {
                Uuid = obj.GetProperty("uuid").GetString() ?? string.Empty,
                Name = obj.GetProperty("names").GetProperty("common").GetString() ?? string.Empty,
                Continent = obj.GetProperty("continents")[0].GetString() ?? string.Empty,
            });
        }

        return countries;
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        var country = new Country();
        // var dbData = await _repository.GetByNameAsync(name);

        var apiResponse = await _externalApiService.GetAllFromApiAsync(name: name, continent: null);
        JsonElement countriesArray = apiResponse.GetProperty("data").GetProperty("objects")[0]; // countries list is under data > objects > list

        return new Country
        {
            Uuid = countriesArray.GetProperty("uuid").GetString() ?? string.Empty,
            Name = countriesArray.GetProperty("names").GetProperty("common").GetString() ?? string.Empty,
            Continent = countriesArray.GetProperty("continents")[0].GetString() ?? string.Empty,
        };
    }

    public async Task<IEnumerable<Country>> GetByContinentAsync(string continent)
    {
        var countries = new List<Country>();
        var apiResponse = await _externalApiService.GetAllFromApiAsync(name: null, continent: continent);
        JsonElement countriesArray = apiResponse.GetProperty("data").GetProperty("objects"); // countries list is under data > objects > list
        foreach (JsonElement obj in countriesArray.EnumerateArray())
        {
            countries.Add(new Country
            {
                Uuid = obj.GetProperty("uuid").GetString() ?? string.Empty,
                Name = obj.GetProperty("names").GetProperty("common").GetString() ?? string.Empty,
                Continent = obj.GetProperty("continents")[0].GetString() ?? string.Empty,
            });
        }

        return countries;
    }
}