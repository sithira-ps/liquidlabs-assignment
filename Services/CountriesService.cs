
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
        List<Country> countries = [];

        var dbData = await _repository.GetAllAsync();

        if (dbData.Count() != 0 && dbData != null)
        {
            return dbData;
        }
        else
        {
            var apiResponse = await _externalApiService.GetAllFromApiAsync(null, null);
            JsonElement countriesArray = apiResponse.GetProperty("data").GetProperty("objects"); // countries list is under data > objects > list
            foreach (JsonElement obj in countriesArray.EnumerateArray())
            {
                countries.Add(new Country
                {
                    Uuid = obj.GetProperty("uuid").GetString() ?? string.Empty,
                    Name = obj.GetProperty("names").GetProperty("common").GetString() ?? string.Empty,
                    Continent = obj.GetProperty("continents")[0].GetString() ?? string.Empty,
                    SyncLevel = SyncLevel.all
                });
            }

            if (countries.Count > 0)
            {
                await _repository.DeleteAllAsync();
                await _repository.CreateBatchAsync(countries);
            }
        }

        return countries;
    }

    public async Task<IEnumerable<Country>> GetByContinentAsync(string continent)
    {
        List<Country> countries = [];

        var dbData = await _repository.GetByContinentAsync(continent);

        if (dbData.Count() != 0 && dbData != null)
        {
            return dbData!;
        }
        else
        {
            var apiResponse = await _externalApiService.GetAllFromApiAsync(name: null, continent: continent);
            JsonElement countriesArray = apiResponse.GetProperty("data").GetProperty("objects"); // countries list is under data > objects > list
            foreach (JsonElement obj in countriesArray.EnumerateArray())
            {
                countries.Add(new Country
                {
                    Uuid = obj.GetProperty("uuid").GetString() ?? string.Empty,
                    Name = obj.GetProperty("names").GetProperty("common").GetString() ?? string.Empty,
                    Continent = obj.GetProperty("continents")[0].GetString() ?? string.Empty,
                    SyncLevel = SyncLevel.continent
                });
            }

            if (countries.Count > 0)
            {
                await _repository.DeleteByContinentAsync(continent);
                await _repository.CreateBatchAsync(countries);
            }
        }

        return countries;
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        var dbData = await _repository.GetByNameAsync(name);

        if (dbData != null)
        {
            return dbData;
        }
        else
        {
            var apiResponse = await _externalApiService.GetAllFromApiAsync(name: name, continent: null);
            JsonElement arrayElement = apiResponse.GetProperty("data").GetProperty("objects"); // countries list is under data > objects > list

            if (arrayElement.GetArrayLength() != 0)
            {
                var country = new Country
                {
                    Uuid = arrayElement[0].GetProperty("uuid").GetString() ?? string.Empty,
                    Name = arrayElement[0].GetProperty("names").GetProperty("common").GetString() ?? string.Empty,
                    Continent = arrayElement[0].GetProperty("continents")[0].GetString() ?? string.Empty,
                    SyncLevel = SyncLevel.country
                };

                await _repository.CreateBatchAsync([country]);

                return country;
            }
            else
            {
                return null;
            }
        }
    }
}