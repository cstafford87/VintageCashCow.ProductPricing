using NUnit.Framework;
using System;
using VintageCashCow.Client.Mappers;
using VintageCashCow.Application.Dtos;

namespace VintageCashCow.Tests.MappingExtensionsTests;

[TestFixture]
public class PriceHistoryDtoMappingTests
{
    [Test]
    public void PriceHistoryDto_ToViewModel_MapsAllProperties()
    {
        // Arrange
        var date = DateTime.UtcNow;
        var dto = new PriceHistoryDto { Price = 2.34m, Date = date };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.Price, Is.EqualTo(dto.Price));
        Assert.That(vm.Date, Is.EqualTo(dto.Date));
    }

    [Test]
    public void PriceHistoryDto_ToViewModel_HandlesZeroPrice()
    {
        // Arrange
        var dto = new PriceHistoryDto { Price = 0m, Date = DateTime.UtcNow };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.Price, Is.EqualTo(0m));
    }

    [Test]
    public void PriceHistoryDto_ToViewModel_HandlesMinDate()
    {
        // Arrange
        var dto = new PriceHistoryDto { Price = 10m, Date = DateTime.MinValue };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.Date, Is.EqualTo(DateTime.MinValue));
    }

    [Test]
    public void PriceHistoryDto_ToViewModel_HandlesMaxDate()
    {
        // Arrange
        var dto = new PriceHistoryDto { Price = 10m, Date = DateTime.MaxValue };

        // Act
        var vm = dto.ToViewModel();

        // Assert
        Assert.That(vm.Date, Is.EqualTo(DateTime.MaxValue));
    }
}
