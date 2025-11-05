using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using VintageCashCow.Client.Models;

namespace VintageCashCow.Tests.ViewModelTests;

[TestFixture]
public class DiscountViewModelTests
{
    [Test]
    public void DiscountViewModel_InitializesWithDefaults()
    {
        // Act
        var vm = new DiscountViewModel();

        // Assert
        Assert.That(vm.Discount, Is.EqualTo(0m));
    }

    [Test]
    public void DiscountViewModel_ValidDiscount_PassesValidation()
    {
        // Arrange
        var vm = new DiscountViewModel { Discount = 25m };
        var context = new ValidationContext(vm);
        var results = new System.Collections.Generic.List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(vm, context, results, true);

        // Assert
        Assert.That(isValid, Is.True);
        Assert.That(results.Count, Is.EqualTo(0));
    }

    [Test]
    public void DiscountViewModel_ZeroDiscount_PassesValidation()
    {
        // Arrange
        var vm = new DiscountViewModel { Discount = 0m };
        var context = new ValidationContext(vm);
        var results = new System.Collections.Generic.List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(vm, context, results, true);

        // Assert
        Assert.That(isValid, Is.True);
    }

    [Test]
    public void DiscountViewModel_MaximumDiscount_PassesValidation()
    {
        // Arrange
        var vm = new DiscountViewModel { Discount = 100m };
        var context = new ValidationContext(vm);
        var results = new System.Collections.Generic.List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(vm, context, results, true);

        // Assert
        Assert.That(isValid, Is.True);
    }

    [Test]
    public void DiscountViewModel_NegativeDiscount_FailsValidation()
    {
        // Arrange
        var vm = new DiscountViewModel { Discount = -1m };
        var context = new ValidationContext(vm);
        var results = new System.Collections.Generic.List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(vm, context, results, true);

        // Assert
        Assert.That(isValid, Is.False);
        Assert.That(results[0].ErrorMessage, Does.Contain("between 0 and 100"));
    }

    [Test]
    public void DiscountViewModel_DiscountOver100_FailsValidation()
    {
        // Arrange
        var vm = new DiscountViewModel { Discount = 101m };
        var context = new ValidationContext(vm);
        var results = new System.Collections.Generic.List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(vm, context, results, true);

        // Assert
        Assert.That(isValid, Is.False);
        Assert.That(results[0].ErrorMessage, Does.Contain("between 0 and 100"));
    }
}
