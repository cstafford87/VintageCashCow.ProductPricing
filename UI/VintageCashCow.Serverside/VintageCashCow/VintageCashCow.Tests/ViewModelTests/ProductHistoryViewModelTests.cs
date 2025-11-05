using VintageCashCow.WebUI.Server.Models;

namespace VintageCashCow.Tests.ViewModelTests;

[TestFixture]
public class ProductHistoryViewModelTests
{
    [Test]
    public void ProductHistoryViewModel_InitializesWithDefaults()
    {
        // Act
        var vm = new ProductHistoryViewModel();

        // Assert
        Assert.That(vm.Id, Is.EqualTo(0));
        Assert.That(vm.Name, Is.EqualTo(string.Empty));
        Assert.That(vm.PriceHistory, Is.Not.Null);
        Assert.That(vm.PriceHistory.Count, Is.EqualTo(0));
    }

    [Test]
    public void ProductHistoryViewModel_CanSetProperties()
    {
        // Arrange
        var history = new List<PriceHistoryViewModel>
        {
            new() { Price = 10m, Date = DateTime.UtcNow }
        };

        var vm = new ProductHistoryViewModel
        {
            Id = 1,
            Name = "Product",
            PriceHistory = history
        };

        // Assert
        Assert.That(vm.Id, Is.EqualTo(1));
        Assert.That(vm.Name, Is.EqualTo("Product"));
        Assert.That(vm.PriceHistory.Count, Is.EqualTo(1));
    }
}
