using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageCashCow.Application.Requests
{
    /// <summary>
    /// Represents a request to apply a discount with a specified percentage.
    /// </summary>
    /// <remarks>This class is typically used to encapsulate the discount percentage for operations that
    /// calculate or apply discounts to prices or transactions.</remarks>
    public class DiscountRequest
    {
        /// <summary>
        /// Gets or sets the discount percentage to be applied to the total price.
        /// </summary>
        public decimal DiscountPercentage { get; set; }
    }
}
