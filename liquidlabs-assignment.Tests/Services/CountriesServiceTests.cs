using System.Text.Json;
using liquidlabs_assignment.Models;
using liquidlabs_assignment.Repositories;
using liquidlabs_assignment.Services;
using FluentAssertions;
using Moq;

namespace liquidlabs_assignment.Tests;

public class CountriesServiceTests
{
    [Fact]
    public async Task GetAll_WhenCacheHit_ReturnDataFromDB()
    {
        // arrange
        List<Country> testDbResponse = [
            new Country{
                Uuid="1",
                Name= "Country1",
                Continent = "Continent1",
                SyncLevel = SyncLevel.all
            },
            new Country{
                Uuid="2",
                Name= "Country2",
                Continent = "Continent2",
                SyncLevel = SyncLevel.all
            },
            new Country{
                Uuid="3",
                Name= "Country3",
                Continent = "Continent3",
                SyncLevel = SyncLevel.all
            }
        ];

        var mockExternalApiService = new Mock<IExternalApiService>();

        var mockRepository = new Mock<ICountriesRepository>();
        mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(testDbResponse);

        var service = new CountriesService(mockExternalApiService.Object, mockRepository.Object);

        // act
        var result = await service.GetAllAsync();

        // assert
        result.Should().NotBeNull();
        result.ElementAt(0).Uuid.Should().Be("1");
        result.ElementAt(0).Name.Should().Be("Country1");
        result.ElementAt(0).Continent.Should().Be("Continent1");
        result.ElementAt(0).SyncLevel.Should().Be(SyncLevel.all);
        result.ElementAt(2).Uuid.Should().Be("3");
        result.Count().Should().Be(testDbResponse.Count);
    }

    [Fact]
    public async Task GetAll_WhenCacheMiss_ReturnDataFromApi()
    {
        // arrange
        List<Country> testDbResponse = [];
        List<Country> testApiResponse = [
                       new Country{
                Uuid="1",
                Name= "Country1",
                Continent = "Continent1",
                SyncLevel = SyncLevel.all
            },
            new Country{
                Uuid="2",
                Name= "Country2",
                Continent = "Continent2",
                SyncLevel = SyncLevel.all
            },
            new Country{
                Uuid="3",
                Name= "Country3",
                Continent = "Continent3",
                SyncLevel = SyncLevel.all
            }
                ];

        var mockExternalApiService = new Mock<IExternalApiService>();
        mockExternalApiService.Setup(api => api.GetAllFromApiAsync(null, null)).ReturnsAsync(testApiResponse);

        var mockRepository = new Mock<ICountriesRepository>();
        mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(testDbResponse);

        var service = new CountriesService(mockExternalApiService.Object, mockRepository.Object);

        // act
        var result = await service.GetAllAsync();

        // assert
        result.Should().NotBeNull();
        result.ElementAt(0).Uuid.Should().Be("1");
        result.ElementAt(0).Name.Should().Be("Country1");
        result.ElementAt(0).Continent.Should().Be("Continent1");
        result.ElementAt(0).SyncLevel.Should().Be(SyncLevel.all);
        result.ElementAt(2).Uuid.Should().Be("3");
        result.Count().Should().Be(testApiResponse.Count);
    }

    [Fact]
    public async Task GetByCountry_WhenCacheHit_ReturnDataFromDB()
    {
        // arrange
        Country testDbResponse = new Country
        {
            Uuid = "1",
            Name = "Country1",
            Continent = "Continent1",
            SyncLevel = SyncLevel.country
        };

        var mockExternalApiService = new Mock<IExternalApiService>();

        var mockRepository = new Mock<ICountriesRepository>();
        mockRepository.Setup(repo => repo.GetByCountryAsync("country1")).ReturnsAsync(testDbResponse);

        var service = new CountriesService(mockExternalApiService.Object, mockRepository.Object);

        // act
        var result = await service.GetByCountryAsync("country1");

        // assert
        result.Should().NotBeNull();
        result.Uuid.Should().Be("1");
        result.Name.Should().Be("Country1");
        result.Continent.Should().Be("Continent1");
        result.SyncLevel.Should().Be(SyncLevel.country);
    }

    [Fact]
    public async Task GetByCountry_WhenCacheMiss_ReturnDataFromApi()
    {
        // arrange
        List<Country> testApiResponse = [
               new Country{
                Uuid="1",
                Name= "Country1",
                Continent = "Continent1",
                SyncLevel = SyncLevel.country
            }
        ];

        var mockExternalApiService = new Mock<IExternalApiService>();
        mockExternalApiService.Setup(api => api.GetAllFromApiAsync("country1", null)).ReturnsAsync(testApiResponse);

        var mockRepository = new Mock<ICountriesRepository>();
        mockRepository.Setup(repo => repo.GetByCountryAsync("country1")).ReturnsAsync((Country?)null);

        var service = new CountriesService(mockExternalApiService.Object, mockRepository.Object);

        // act
        var result = await service.GetByCountryAsync("country1");

        // assert
        result.Should().NotBeNull();
        result.Uuid.Should().Be("1");
        result.Name.Should().Be("Country1");
        result.Continent.Should().Be("Continent1");
        result.SyncLevel.Should().Be(SyncLevel.country);
    }

    [Fact]
    public async Task GetByContinent_WhenCacheHit_ReturnDataFromDB()
    {
        // arrange
        List<Country> testDbResponse = [
            new Country{
                Uuid="1",
                Name= "Country1",
                Continent = "Continent1",
                SyncLevel = SyncLevel.continent
            },
            new Country{
                Uuid="2",
                Name= "Country2",
                Continent = "Continent1",
                SyncLevel = SyncLevel.continent
            },
            new Country{
                Uuid="3",
                Name= "Country3",
                Continent = "Continent1",
                SyncLevel = SyncLevel.continent
            }
        ];

        var mockExternalApiService = new Mock<IExternalApiService>();

        var mockRepository = new Mock<ICountriesRepository>();
        mockRepository.Setup(repo => repo.GetByContinentAsync("continent1")).ReturnsAsync(testDbResponse);

        var service = new CountriesService(mockExternalApiService.Object, mockRepository.Object);

        // act
        var result = await service.GetByContinentAsync("continent1");

        // assert
        result.Should().NotBeNull();
        result.ElementAt(0).Uuid.Should().Be("1");
        result.ElementAt(0).Name.Should().Be("Country1");
        result.ElementAt(0).Continent.Should().Be("Continent1");
        result.ElementAt(0).SyncLevel.Should().Be(SyncLevel.continent);
        result.ElementAt(2).Uuid.Should().Be("3");
        result.Count().Should().Be(testDbResponse.Count);
    }

    [Fact]
    public async Task GetByContinent_WhenCacheMiss_ReturnDataFromApi()
    {
        // arrange
        List<Country> testDbResponse = [];
        List<Country> testApiResponse = [
                       new Country{
                Uuid="1",
                Name= "Country1",
                Continent = "Continent1",
                SyncLevel = SyncLevel.country
            },
            new Country{
                Uuid="2",
                Name= "Country2",
                Continent = "Continent1",
                SyncLevel = SyncLevel.continent
            },
            new Country{
                Uuid="3",
                Name= "Country3",
                Continent = "Continent1",
                SyncLevel = SyncLevel.all
            }
                ];

        var mockExternalApiService = new Mock<IExternalApiService>();
        mockExternalApiService.Setup(api => api.GetAllFromApiAsync(null, "continent1")).ReturnsAsync(testApiResponse);

        var mockRepository = new Mock<ICountriesRepository>();
        mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(testDbResponse);

        var service = new CountriesService(mockExternalApiService.Object, mockRepository.Object);

        // act
        var result = await service.GetByContinentAsync("continent1");

        // assert
        result.Should().NotBeNull();
        result.ElementAt(0).Uuid.Should().Be("1");
        result.ElementAt(0).Name.Should().Be("Country1");
        result.ElementAt(0).Continent.Should().Be("Continent1");
        result.ElementAt(0).SyncLevel.Should().Be(SyncLevel.country);
        result.ElementAt(2).Uuid.Should().Be("3");
        result.Count().Should().Be(testApiResponse.Count);
    }

    [Fact]
    public async Task GetByCountry_WhenCountryNotFound_ReturnNull()
    {
        // arrange
        List<Country> testApiResponse = [];

        var mockExternalApiService = new Mock<IExternalApiService>();
        mockExternalApiService.Setup(api => api.GetAllFromApiAsync("no-country", null)).ReturnsAsync(testApiResponse);

        var mockRepository = new Mock<ICountriesRepository>();
        mockRepository.Setup(repo => repo.GetByCountryAsync("no-country")).ReturnsAsync((Country?)null);

        var service = new CountriesService(mockExternalApiService.Object, mockRepository.Object);

        // act
        var result = await service.GetByCountryAsync("no-country");

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByContinent_WhenContinentNotFound_ReturnEmptyArray()
    {
        // arrange
        List<Country> dbApiResponse = [];
        List<Country> testApiResponse = [];

        var mockExternalApiService = new Mock<IExternalApiService>();
        mockExternalApiService.Setup(api => api.GetAllFromApiAsync(null, "no-continent")).ReturnsAsync(testApiResponse);

        var mockRepository = new Mock<ICountriesRepository>();
        mockRepository.Setup(repo => repo.GetByContinentAsync("no-continent")).ReturnsAsync(dbApiResponse);

        var service = new CountriesService(mockExternalApiService.Object, mockRepository.Object);

        // act
        var result = await service.GetByContinentAsync("no-continent");

        // assert
        result.Should().NotBeNull();
        result.Count().Should().Be(0);
    }

}