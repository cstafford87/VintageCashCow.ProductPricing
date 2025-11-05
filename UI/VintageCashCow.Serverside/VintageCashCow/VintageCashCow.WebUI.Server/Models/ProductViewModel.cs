using System.ComponentModel.DataAnnotations;

namespace VintageCashCow.WebUI.Server.Models
{
    /// <summary>
    /// Represents a product with details such as its identifier, name, price, last updated timestamp,  and associated
    /// pricing and discount information.
    /// </summary>
    /// <remarks>This view model is typically used to display product information in user interfaces or to
    /// transfer  product data between application layers. It includes properties for managing pricing updates and 
    /// discounts through the <see cref="PriceUpdate"/> and <see cref="Discount"/> properties, respectively.</remarks>
    public class ProductViewModel
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
        /// <remarks>The value is typically set to the current date and time when the entity is
        /// modified.</remarks>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the view model containing details for updating price information.
        /// </summary>
        public PriceUpdateViewModel PriceUpdate { get; set; } = new();

        /// <summary>
        /// Gets or sets the discount details for the current transaction.
        /// </summary>
        public DiscountViewModel DiscountUpdate { get; set; } = new();
    }
}
