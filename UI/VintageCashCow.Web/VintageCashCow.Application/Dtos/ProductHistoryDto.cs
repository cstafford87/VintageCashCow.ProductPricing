namespace VintageCashCow.Application.Dtos
{
    public class ProductHistoryDto
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
        /// Gets or sets the historical price data for the associated entity.
        /// </summary>
        public List<PriceHistoryDto> PriceHistory { get; set; } = new();
    }
}
