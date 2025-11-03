namespace VintageCashCow.ProductPricing.Application.Dtos
{
    /// <summary>
    /// Represents the historical data of a product, including its identifier, name, and price history over time.
    /// </summary>
    /// <remarks>This data transfer object (DTO) is typically used to encapsulate product history information
    /// for communication between application layers. The <see cref="PriceHistory"/> property provides a detailed record
    /// of price changes associated with the product.</remarks>
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
