using System.Text.Json;

namespace liquidlabs_assignment.Services;

public interface IExternalApiService
{
    Task<JsonElement> GetAllFromApiAsync(string? name, string? continent);
}