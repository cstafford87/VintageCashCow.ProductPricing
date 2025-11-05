namespace VintageCashCow.Client.Models;

/// <summary>
/// View model representing a product with its complete price history.
/// </summary>
/// <remarks>
/// This view model is used to display historical price data for a product,
/// typically in a dialog or detailed view.
/// </remarks>
public class ProductHistoryViewModel
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
    /// Gets or sets the list of historical price entries for the product.
    /// </summary>
    /// <value>A list of <see cref="PriceHistoryViewModel"/> instances representing price changes over time.</value>
    public List<PriceHistoryViewModel> PriceHistory { get; set; } = [];
}
