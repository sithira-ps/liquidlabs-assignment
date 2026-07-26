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

    public async Task<JsonElement> GetAllFromApiAsync(string? name, string? continent)
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

        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiConfig.ApiKey}");
        var data = await _client.GetFromJsonAsync<JsonElement>(endpoint);
        return data;
    }

}