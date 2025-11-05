namespace VintageCashCow.Application.Dtos
{
    /// <summary>
    /// Represents a discount applied to a product or service, including details about the discount and its effect on
    /// pricing.
    /// </summary>
    /// <remarks>This class provides information about a specific discount, including its identifier, name,
    /// and the original and discounted prices. It can be used to track and display applied discounts in pricing
    /// calculations or user interfaces.</remarks>
    public class AppliedDiscountDto
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
        /// Gets or sets the original price of the item before any discounts or adjustments are applied.
        /// </summary>
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// Gets or sets the price of the item after applying any applicable discounts.
        /// </summary>
        public decimal DiscountedPrice { get; set; }
    }
}
