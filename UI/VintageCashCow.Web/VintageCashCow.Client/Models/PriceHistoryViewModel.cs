namespace VintageCashCow.Client.Models;

/// <summary>
/// View model representing a single price history entry for a product.
/// </summary>
/// <remarks>
/// This view model captures a snapshot of a product's price at a specific point in time.
/// </remarks>
public class PriceHistoryViewModel
{
    /// <summary>
    /// Gets or sets the price of the product at this point in history.
    /// </summary>
    /// <value>The historical price in decimal format.</value>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this price was recorded.
    /// </summary>
    /// <value>The timestamp of this price entry.</value>
    public DateTime Date { get; set; }
}
