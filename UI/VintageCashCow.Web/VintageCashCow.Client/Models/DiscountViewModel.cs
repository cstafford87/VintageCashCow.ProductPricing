using System.ComponentModel.DataAnnotations;

namespace VintageCashCow.Client.Models;

/// <summary>
/// View model for applying a discount to a product with validation.
/// </summary>
/// <remarks>
/// This view model is used in forms to capture and validate discount percentages.
/// The discount must be between 0 and 100 percent.
/// </remarks>
public class DiscountViewModel
{
    /// <summary>
    /// Gets or sets the discount percentage to apply.
    /// </summary>
    /// <value>The discount percentage, which must be between 0 and 100.</value>
    /// <exception cref="ValidationException">Thrown when the discount is not between 0 and 100.</exception>
    [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
    public decimal Discount { get; set; }
}
