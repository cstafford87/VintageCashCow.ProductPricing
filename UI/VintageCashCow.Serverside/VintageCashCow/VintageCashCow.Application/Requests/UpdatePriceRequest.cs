using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageCashCow.Application.Requests
{
    /// <summary>
    /// Represents a request to update the price of an item.
    /// </summary>
    /// <remarks>This class is used to encapsulate the new price value when making a price update
    /// request.</remarks>
    public class UpdatePriceRequest
    {
        /// <summary>
        /// Gets or sets the new price of the item.
        /// </summary>
        public decimal NewPrice { get; set; }
    }
}
