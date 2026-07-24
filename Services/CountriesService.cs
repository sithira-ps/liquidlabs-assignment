
using System.Text.Json;
using liquidlabs_assignment.Models;

namespace liquidlabs_assignment.Services;

public class CountriesService : ICountriesService
{
    private readonly IExternalApiService _externalApiService;

    public CountriesService(IExternalApiService externalApiService)
    {
        _externalApiService = externalApiService;
    }

    public async Task<JsonElement> GetAllAsync()
    {
        var data = await _externalApiService.GetAllFromApiAsync();
        return data;
    }
}