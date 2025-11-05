using VintageCashCow.Application.Dtos;
using VintageCashCow.WebUI.Server.Mappers;

namespace VintageCashCow.Tests.MappingExtensionsTests;

[TestFixture]
public class ProductHistoryDtoMappingTests
{
    [Test]
    public void ProductHistoryDto_ToViewModel_MapsAllProperties()
    {
        // Arrange
        var date = DateTime.UtcNow;
        var dto = new ProductHistoryDto
        {
            Id = 2,
            Name = "History Product",
            PriceHistory = [new PriceHistoryDto { Price = 1.23m, Date = date }]
        };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.Id, Is.EqualTo(dto.Id));
        Assert.That(vm.Name, Is.EqualTo(dto.Name));
        Assert.That(vm.PriceHistory.Count, Is.EqualTo(dto.PriceHistory.Count));
        Assert.That(vm.PriceHistory[0].Price, Is.EqualTo(dto.PriceHistory[0].Price));
        Assert.That(vm.PriceHistory[0].Date, Is.EqualTo(dto.PriceHistory[0].Date));
    }

    [Test]
    public void ProductHistoryDto_ToViewModel_HandlesEmptyPriceHistory()
    {
        // Arrange
        var dto = new ProductHistoryDto
        {
            Id = 2,
            Name = "H",
            PriceHistory = []
        };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.PriceHistory, Is.Not.Null);
        Assert.That(vm.PriceHistory.Count, Is.EqualTo(0));
    }

    [Test]
    public void ProductHistoryDto_ToViewModel_HandlesMultiplePriceHistoryEntries()
    {
        // Arrange
        var dto = new ProductHistoryDto
        {
            Id = 2,
            Name = "H",
            PriceHistory =
            [ new PriceHistoryDto { Price = 10m, Date = DateTime.UtcNow.AddDays(-2) },
                new PriceHistoryDto { Price = 15m, Date = DateTime.UtcNow.AddDays(-1) },
                new PriceHistoryDto { Price = 20m, Date = DateTime.UtcNow } ]
        };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.PriceHistory.Count, Is.EqualTo(3));
        Assert.That(vm.PriceHistory[0].Price, Is.EqualTo(10m));
        Assert.That(vm.PriceHistory[1].Price, Is.EqualTo(15m));
        Assert.That(vm.PriceHistory[2].Price, Is.EqualTo(20m));
    }
}
