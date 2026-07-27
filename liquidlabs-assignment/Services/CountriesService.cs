
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
        List<Country> countries;
        var dbData = await _repository.GetAllAsync();

        if (dbData != null && dbData.Count() != 0)
        {
            return dbData;
        }
        else
        {
            countries = await _externalApiService.GetAllFromApiAsync(null, null);

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
        List<Country> countries;

        var dbData = await _repository.GetByContinentAsync(continent);

        if (dbData != null && dbData.Count() != 0)
        {
            return dbData;
        }
        else
        {
            countries = await _externalApiService.GetAllFromApiAsync(null, continent: continent);

            if (countries.Count > 0)
            {
                await _repository.DeleteByContinentAsync(continent);
                await _repository.CreateBatchAsync(countries);
            }
        }

        return countries;
    }

    public async Task<Country?> GetByCountryAsync(string name)
    {
        List<Country> countries;

        var dbData = await _repository.GetByCountryAsync(name);

        if (dbData != null)
        {
            return dbData;
        }
        else
        {
            countries = await _externalApiService.GetAllFromApiAsync(name: name, continent: null);

            if (countries.Count != 0)
            {
                var country = new Country
                {
                    Uuid = countries[0].Uuid ?? string.Empty,
                    Name = countries[0].Name ?? string.Empty,
                    Continent = countries[0].Continent ?? string.Empty,
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