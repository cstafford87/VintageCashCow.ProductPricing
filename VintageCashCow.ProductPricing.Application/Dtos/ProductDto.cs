namespace VintageCashCow.ProductPricing.Application.Dtos
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a product, containing details such as its identifier, name, price,
    /// and last updated timestamp.
    /// </summary>
    /// <remarks>This class is typically used to transfer product data between different layers of an
    /// application, such as between the database and the user interface.</remarks>
    public class ProductDto
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
        /// Gets or sets the date and time when the entity was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
