namespace VintageCashCow.ProductPricing.Application.Dtos
{
    /// <summary>
    /// Represents the data transfer object used to update the price of a product.
    /// </summary>
    /// <remarks>This DTO is typically used in scenarios where a product's price needs to be updated. It
    /// contains the product identifier and the new price value to be applied.</remarks>
    public class UpdatePriceDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Gets or sets the new price of the item.
        /// </summary>
        public decimal NewPrice { get; set; }
    }
}
