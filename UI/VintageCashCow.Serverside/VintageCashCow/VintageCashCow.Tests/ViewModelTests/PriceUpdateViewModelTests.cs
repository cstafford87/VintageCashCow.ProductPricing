using System.ComponentModel.DataAnnotations;
using VintageCashCow.WebUI.Server.Models;

namespace VintageCashCow.Tests.ViewModelTests;

[TestFixture]
public class PriceUpdateViewModelTests
{
    [Test]
    public void PriceUpdateViewModel_InitializesWithDefaults()
    {
        // Act
        var vm = new PriceUpdateViewModel();

        // Assert
        Assert.That(vm.NewPrice, Is.EqualTo(0m));
    }

    [Test]
    public void PriceUpdateViewModel_ValidPrice_PassesValidation()
    {
        // Arrange
        var vm = new PriceUpdateViewModel { NewPrice = 10.50m };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(vm, context, results, true);

        // Assert
        Assert.That(isValid, Is.True);
        Assert.That(results.Count, Is.EqualTo(0));
    }

    [Test]
    public void PriceUpdateViewModel_ZeroPrice_FailsValidation()
    {
        // Arrange
        var vm = new PriceUpdateViewModel { NewPrice = 0m };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(vm, context, results, true);

        // Assert
        Assert.That(isValid, Is.False);
        Assert.That(results.Count, Is.GreaterThan(0));
        Assert.That(results[0].ErrorMessage, Does.Contain("greater than 0"));
    }

    [Test]
    public void PriceUpdateViewModel_NegativePrice_FailsValidation()
    {
        // Arrange
        var vm = new PriceUpdateViewModel { NewPrice = -5m };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(vm, context, results, true);

        // Assert
        Assert.That(isValid, Is.False);
    }

    [Test]
    public void PriceUpdateViewModel_MinimumValidPrice_PassesValidation()
    {
        // Arrange
        var vm = new PriceUpdateViewModel { NewPrice = 0.01m };
        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(vm, context, results, true);

        // Assert
        Assert.That(isValid, Is.True);
    }
}
