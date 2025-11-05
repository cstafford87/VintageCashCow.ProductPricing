using System.ComponentModel.DataAnnotations;

namespace VintageCashCow.WebUI.Server.Models
{
    /// <summary>
    /// Represents a view model for applying a discount, with validation to ensure the discount value is within a valid
    /// range.
    /// </summary>
    /// <remarks>The <see cref="Discount"/> property is constrained to values between 0 and 100, inclusive. 
    /// Attempting to set a value outside this range will result in a validation error.</remarks>
    public class DiscountViewModel
    {
        /// <summary>
        /// Gets or sets the discount percentage to be applied.
        /// </summary>
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public decimal Discount { get; set; }
    }
}
