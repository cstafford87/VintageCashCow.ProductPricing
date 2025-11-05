using System.ComponentModel.DataAnnotations;

namespace VintageCashCow.WebUI.Server.Models
{
    /// <summary>
    /// Represents the data model for updating a price, including validation constraints.
    /// </summary>
    /// <remarks>This view model is typically used to capture and validate user input for price updates. The
    /// <see cref="NewPrice"/> property enforces a minimum value of 0.01.</remarks>
    public class PriceUpdateViewModel
    {
        /// <summary>
        /// Gets or sets the new price of the item.
        /// </summary>
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal NewPrice { get; set; }
    }
}
