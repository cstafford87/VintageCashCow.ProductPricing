using VintageCashCow.WebUI.Server.Models;

namespace VintageCashCow.Tests.ViewModelTests;

[TestFixture]
public class ProductViewModelTests
{
    [Test]
    public void ProductViewModel_InitializesWithDefaults()
    {
// Act
        var vm = new ProductViewModel();

        // Assert
        Assert.That(vm.Id, Is.EqualTo(0));
        Assert.That(vm.Name, Is.EqualTo(string.Empty));
        Assert.That(vm.Price, Is.EqualTo(0m));
        Assert.That(vm.PriceUpdate, Is.Not.Null);
        Assert.That(vm.DiscountUpdate, Is.Not.Null);
    }

    [Test]
    public void ProductViewModel_CanSetAllProperties()
    {
        // Arrange
        var date = DateTime.UtcNow;
        var vm = new ProductViewModel
        {
            Id = 1,
            Name = "Test Product",
            Price = 99.99m,
            LastUpdated = date
        };

        // Assert
        Assert.That(vm.Id, Is.EqualTo(1));
        Assert.That(vm.Name, Is.EqualTo("Test Product"));
        Assert.That(vm.Price, Is.EqualTo(99.99m));
        Assert.That(vm.LastUpdated, Is.EqualTo(date));
    }
}
