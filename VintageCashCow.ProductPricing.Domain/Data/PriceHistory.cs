namespace VintageCashCow.ProductPricing.Domain.Data
{
    /// <summary>
    /// Represents a record of a price at a specific point in time.
    /// </summary>
    /// <remarks>This class is typically used to store and track historical price data,  such as for financial
    /// or inventory systems.</remarks>
    public class PriceHistory
    {
        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the date value.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
