using System.Text.Json;
using liquidlabs_assignment.Models;

namespace liquidlabs_assignment.Services;

public interface IExternalApiService
{
    Task<JsonElement> GetAllFromApiAsync(string? name, string? continent);
}