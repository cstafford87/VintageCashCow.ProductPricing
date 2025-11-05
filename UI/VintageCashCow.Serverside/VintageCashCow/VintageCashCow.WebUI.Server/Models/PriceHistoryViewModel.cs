namespace VintageCashCow.WebUI.Server.Models
{
    /// <summary>
    /// Represents a view model for tracking the price history of an item, including the price and the date it was
    /// recorded.
    /// </summary>
    public class PriceHistoryViewModel
    {
        /// <summary>
        /// Gets or sets the price of the item at a specific point in time.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the date and time when this price was recorded.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
