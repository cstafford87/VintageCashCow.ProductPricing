namespace VintageCashCow.ProductPricing.Domain.Data
{
    /// <summary>
    /// Represents a product with an identifier, name, price, and price history.
    /// </summary>
    /// <remarks>This class is used to model a product in an inventory or catalog system.  It includes
    /// properties for tracking the product's current price, name, and the history of price changes.</remarks>
    public class Product
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
        /// Gets or sets the price of the item.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the object was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the historical price data for the associated entity.
        /// </summary>
        /// <remarks>This property provides access to the price history, which can be used for analysis,
        /// reporting, or tracking changes. The list is initialized as an empty collection by default.</remarks>
        public List<PriceHistory> PriceHistory { get; set; } = new List<PriceHistory>();
    }
}
