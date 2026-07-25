using System.Text.Json;
using liquidlabs_assignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace liquidlabs_assignment.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ICountriesService _countriesService;

    public CountriesController(ICountriesService countriesService)
    {
        _countriesService = countriesService;
    }

    [HttpGet]
    public async Task<JsonElement> GetAllAsync()
    {
        var result = await _countriesService.GetAllAsync();
        return result;
    }

    [HttpGet("{name}")]
    public async Task<JsonElement> GetByNameAsync(string name)
    {
        var result = await _countriesService.GetAllAsync();
        return result;
    }

    [HttpGet("continent/{continent}")]
    public async Task<JsonElement> GetByContinentAsync(string continent)
    {
        Console.WriteLine(continent);
        var result = await _countriesService.GetAllAsync();
        return result;
    }
}