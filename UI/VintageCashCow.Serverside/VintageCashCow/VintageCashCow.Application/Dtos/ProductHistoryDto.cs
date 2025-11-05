namespace VintageCashCow.Application.Dtos
{
    /// <summary>
    /// Represents a data transfer object for product history, containing product details and a collection of price
    /// history records.
    /// </summary>
    /// <remarks>This class is used to transfer product history data between different layers of the application,
    /// including the product's unique identifier, name, and its complete price history timeline.</remarks>
    public class ProductHistoryDto
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
        /// Gets or sets the historical price data for the associated entity.
        /// </summary>
        public List<PriceHistoryDto> PriceHistory { get; set; } = new();
    }
}
