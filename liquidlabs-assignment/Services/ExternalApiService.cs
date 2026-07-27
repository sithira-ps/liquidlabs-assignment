using System.Text.Json;
using liquidlabs_assignment.Models;
using Microsoft.Extensions.Options;

namespace liquidlabs_assignment.Services;

public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _client;
    private readonly ExternalApiConfig _apiConfig;

    public ExternalApiService(HttpClient client, IOptions<ExternalApiConfig> apiConfig)
    {
        _client = client;
        _apiConfig = apiConfig.Value;
    }

    public async Task<List<Country>> GetAllFromApiAsync(string? name, string? continent)
    {
        string endpoint;

        // set the endpoint based on which is searching for
        if (name == null && continent == null)
            endpoint = $"{_apiConfig.BaseUrl}/v5?limit=100";
        else if (name != null && continent == null)
            endpoint = $"{_apiConfig.BaseUrl}/v5/names.common/{name}";
        else if (name == null && continent != null)
            endpoint = $"{_apiConfig.BaseUrl}/v5/continents/{continent}";
        else
            throw new Exception();


        var data = await _client.GetFromJsonAsync<JsonElement>(endpoint);
        JsonElement countriesArray = data.GetProperty("data").GetProperty("objects"); // countries list is under data > objects > list

        List<Country> countries = [];

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

        return countries;
    }

}