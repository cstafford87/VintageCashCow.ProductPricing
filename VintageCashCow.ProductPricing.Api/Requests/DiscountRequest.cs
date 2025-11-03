namespace VintageCashCow.ProductPricing.Api.Requests
{
    /// <summary>
    /// Represents a request to apply a discount to a total price.
    /// </summary>
    /// <remarks>This class encapsulates the discount percentage to be applied. It can be used to pass
    /// discount information to methods or services that calculate final pricing.</remarks>
    public class DiscountRequest
    {
        /// <summary>
        /// Gets or sets the discount percentage to be applied to the total price.
        /// </summary>
        public decimal DiscountPercentage { get; set; }
    }
}