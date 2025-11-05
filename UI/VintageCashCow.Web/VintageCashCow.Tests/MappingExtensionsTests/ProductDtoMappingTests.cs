using VintageCashCow.Client.Mappers;
using VintageCashCow.Application.Dtos;

namespace VintageCashCow.Tests.MappingExtensionsTests;

[TestFixture]
public class ProductDtoMappingTests
{
    [Test]
    public void ProductDto_ToViewModel_MapsAllProperties()
    {
        // Arrange
        var date = DateTime.UtcNow;
        var dto = new ProductDto
        {
            Id = 1,
            Name = "Test Product",
            Price = 9.99m,
            LastUpdated = date
        };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.Id, Is.EqualTo(dto.Id));
        Assert.That(vm.Name, Is.EqualTo(dto.Name));
        Assert.That(vm.Price, Is.EqualTo(dto.Price));
        Assert.That(vm.LastUpdated, Is.EqualTo(dto.LastUpdated));
    }

    [Test]
    public void ProductDto_ToViewModel_InitializesNestedViewModels()
    {
        // Arrange
        var dto = new ProductDto { Id = 1, Name = "X", Price = 9.99m, LastUpdated = DateTime.UtcNow };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.PriceUpdate, Is.Not.Null);
        Assert.That(vm.DiscountUpdate, Is.Not.Null);
    }

    [Test]
    public void ProductDto_ToViewModel_HandlesZeroPrice()
    {
        // Arrange
        var dto = new ProductDto { Id = 1, Name = "X", Price = 0m, LastUpdated = DateTime.UtcNow };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.Price, Is.EqualTo(0m));
    }

    [Test]
    public void ProductDto_ToViewModel_HandlesEmptyName()
    {
        // Arrange
        var dto = new ProductDto { Id = 1, Name = "", Price = 10m, LastUpdated = DateTime.UtcNow };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.Name, Is.EqualTo(string.Empty));
    }

    [Test]
    public void ProductDto_ToViewModel_HandlesLargePrice()
    {
        // Arrange
        var dto = new ProductDto { Id = 1, Name = "X", Price = 999999.99m, LastUpdated = DateTime.UtcNow };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.Price, Is.EqualTo(999999.99m));
    }
}
