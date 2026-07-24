using liquidlabs_assignment.Models;
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
    public async Task<Country> GetAllAsync()
    {
        var result = await _countriesService.GetAllAsync();
        return result;
    }
}