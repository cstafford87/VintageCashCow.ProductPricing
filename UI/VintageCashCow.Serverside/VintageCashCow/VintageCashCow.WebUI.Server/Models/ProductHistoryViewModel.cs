using System.Collections.Generic;

namespace VintageCashCow.WebUI.Server.Models
{
    /// <summary>
    /// Represents a view model for product history, including product details and its price history over time.
    /// </summary>
    /// <remarks>This class is typically used to encapsulate product information along with its associated
    /// price history for display or processing in user interfaces or business logic. It includes the product's unique
    /// identifier, name, a collection of price history records, and the total number of items related to the
    /// product.</remarks>
    public class ProductHistoryViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the historical price data for the associated item.
        /// </summary>
        public List<PriceHistoryViewModel> PriceHistory { get; set; } = new();
    }
}
