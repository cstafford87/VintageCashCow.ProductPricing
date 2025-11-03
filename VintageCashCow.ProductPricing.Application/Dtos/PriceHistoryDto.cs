using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageCashCow.ProductPricing.Application.Dtos
{
    /// <summary>
    /// Represents a data transfer object for storing historical price information.
    /// </summary>
    /// <remarks>This class is typically used to encapsulate the price of an item and the corresponding date 
    /// for historical tracking or reporting purposes.</remarks>
    public class PriceHistoryDto
    {
        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the date value.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
