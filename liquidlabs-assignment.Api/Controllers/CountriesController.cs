using System.Text.Json;
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
    public async Task<SuccessResponse<IEnumerable<Country>>> GetAllAsync()
    {
        var result = await _countriesService.GetAllAsync();
        return new SuccessResponse<IEnumerable<Country>>
        {
            status = "success",
            data = result,
        };
    }

    [HttpGet("{name}")]
    public async Task<SuccessResponse<IEnumerable<Country>>> GetByCountryAsync(string name)
    {
        Country? result = await _countriesService.GetByCountryAsync(name);
        var response = new SuccessResponse<IEnumerable<Country>>
        {
            status = "success",
            data = result != null ? [result] : [],
        };
        return response;
    }

    [HttpGet("continent/{continent}")]
    public async Task<SuccessResponse<IEnumerable<Country>>> GetByContinentAsync(string continent)
    {
        IEnumerable<Country> result = await _countriesService.GetByContinentAsync(continent);
        return new SuccessResponse<IEnumerable<Country>>
        {
            status = "success",
            data = result,
        };
    }
}