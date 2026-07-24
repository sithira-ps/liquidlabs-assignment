using System.Text.Json;
using liquidlabs_assignment.Models;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;

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

    public async Task<JsonElement> GetAllFromApiAsync()
    {
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiConfig.ApiKey}");
        Console.WriteLine(_apiConfig.ApiKey);
        var data = await _client.GetFromJsonAsync<JsonElement>($"{_apiConfig.BaseUrl}/v5?limit=5");
        return data;
    }

}