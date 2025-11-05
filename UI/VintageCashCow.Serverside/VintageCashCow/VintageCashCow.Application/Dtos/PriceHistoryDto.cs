namespace VintageCashCow.Application.Dtos
{
    /// <summary>
    /// Represents a data transfer object for storing the price history of an item,  including the price and the
    /// associated date.
    /// </summary>
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
