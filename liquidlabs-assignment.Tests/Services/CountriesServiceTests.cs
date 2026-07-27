using System.Text.Json;
using liquidlabs_assignment.Models;
using liquidlabs_assignment.Repositories;
using liquidlabs_assignment.Services;
using FluentAssertions;
using Moq;

namespace liquidlabs_assignment.Tests;

public class CountriesServiceTests
{
    public CountriesServiceTests()
    {

    }

    [Fact]
    public async Task GetAll_WhenCacheHit_ReturnDataFromDB()
    {
        // arrange
        List<Country> testDbResponse = [
            new Country{
                Uuid="1",
                Name= "Country1",
                Continent = "Continent1",
                SyncLevel = SyncLevel.country
            },
            new Country{
                Uuid="2",
                Name= "Country2",
                Continent = "Continent2",
                SyncLevel = SyncLevel.continent
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
        result.ElementAt(0).SyncLevel.Should().Be(SyncLevel.country);
        result.ElementAt(2).Uuid.Should().Be("3");
        result.Count().Should().Be(testDbResponse.Count);
    }
}