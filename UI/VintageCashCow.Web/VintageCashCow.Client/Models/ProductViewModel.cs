using System.ComponentModel.DataAnnotations;

namespace VintageCashCow.Client.Models;

/// <summary>
/// View model representing a product with price update and discount capabilities.
/// </summary>
/// <remarks>
/// This view model is used by the UI to display product information and bind to
/// form controls for updating prices and applying discounts.
/// </remarks>
public class ProductViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    /// <value>The product ID.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    /// <value>The product name.</value>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current price of the product.
    /// </summary>
    /// <value>The product price in decimal format.</value>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the product was last updated.
    /// </summary>
    /// <value>The last updated timestamp.</value>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Gets or sets the view model for updating the product price.
    /// </summary>
    /// <value>A <see cref="PriceUpdateViewModel"/> instance containing the new price value.</value>
    public PriceUpdateViewModel PriceUpdate { get; set; } = new();

    /// <summary>
    /// Gets or sets the view model for applying a discount to the product.
    /// </summary>
    /// <value>A <see cref="DiscountViewModel"/> instance containing the discount percentage.</value>
    public DiscountViewModel DiscountUpdate { get; set; } = new();
}
