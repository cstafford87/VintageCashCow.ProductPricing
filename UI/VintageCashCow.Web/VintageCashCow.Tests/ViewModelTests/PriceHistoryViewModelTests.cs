using NUnit.Framework;
using System;
using VintageCashCow.Client.Models;

namespace VintageCashCow.Tests.ViewModelTests;

[TestFixture]
public class PriceHistoryViewModelTests
{
    [Test]
    public void PriceHistoryViewModel_InitializesWithDefaults()
    {
        // Act
        var vm = new PriceHistoryViewModel();

        // Assert
        Assert.That(vm.Price, Is.EqualTo(0m));
        Assert.That(vm.Date, Is.EqualTo(default(DateTime)));
    }

    [Test]
    public void PriceHistoryViewModel_CanSetProperties()
    {
        // Arrange
        var date = DateTime.UtcNow;
        var vm = new PriceHistoryViewModel
        {
            Price = 25.99m,
            Date = date
        };

        // Assert
        Assert.That(vm.Price, Is.EqualTo(25.99m));
        Assert.That(vm.Date, Is.EqualTo(date));
    }
}
