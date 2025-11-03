namespace VintageCashCow.ProductPricing.Application.Dtos
{
    /// <summary>
    /// Represents a data transfer object (DTO) for applying a discount to a product.
    /// </summary>
    /// <remarks>This class is typically used to transfer discount information, such as the product identifier
    /// and the discount percentage, between different layers of an application.</remarks>
    public class DiscountDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage to be applied to the total price.
        /// </summary>
        public decimal DiscountPercentage { get; set; }
    }
}
