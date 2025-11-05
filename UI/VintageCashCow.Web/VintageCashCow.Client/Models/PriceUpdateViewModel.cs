using System.ComponentModel.DataAnnotations;

namespace VintageCashCow.Client.Models;

/// <summary>
/// View model for updating a product's price with validation.
/// </summary>
/// <remarks>
/// This view model is used in forms to capture and validate new price values.
/// The price must be greater than zero.
/// </remarks>
public class PriceUpdateViewModel
{
    /// <summary>
    /// Gets or sets the new price for the product.
    /// </summary>
    /// <value>The new price value, which must be greater than 0.</value>
    /// <exception cref="ValidationException">Thrown when the price is not greater than 0.</exception>
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal NewPrice { get; set; }
}
