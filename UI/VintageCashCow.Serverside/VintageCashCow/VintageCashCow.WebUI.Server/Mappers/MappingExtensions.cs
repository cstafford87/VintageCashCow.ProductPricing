using VintageCashCow.Application.Dtos;
using VintageCashCow.WebUI.Server.Models;

namespace VintageCashCow.WebUI.Server.Mappers
{
    /// <summary>
    /// Provides extension methods for mapping data transfer objects (DTOs) to view models.
    /// </summary>
    /// <remarks>This static class contains methods that facilitate the conversion of application-layer DTOs
    /// to presentation-layer view models, ensuring proper separation of concerns between different architectural
    /// layers.</remarks>
    public static class MappingExtensions
    {
        /// <summary>
        /// Converts a <see cref="ProductDto"/> to a <see cref="ProductViewModel"/>.
        /// </summary>
        /// <param name="dto">The product DTO to convert.</param>
        /// <returns>A new <see cref="ProductViewModel"/> instance populated with data from the DTO.</returns>
        public static ProductViewModel ToViewModel(this ProductDto dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            LastUpdated = dto.LastUpdated
        };

        /// <summary>
        /// Converts a <see cref="ProductHistoryDto"/> to a <see cref="ProductHistoryViewModel"/>.
        /// </summary>
        /// <param name="dto">The product history DTO to convert.</param>
        /// <returns>A new <see cref="ProductHistoryViewModel"/> instance with all price history records mapped.</returns>
        public static ProductHistoryViewModel ToViewModel(this ProductHistoryDto dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            PriceHistory = dto.PriceHistory.Select(ph => ph.ToViewModel()).ToList()
        };

        /// <summary>
        /// Converts a <see cref="PriceHistoryDto"/> to a <see cref="PriceHistoryViewModel"/>.
        /// </summary>
        /// <param name="dto">The price history DTO to convert.</param>
        /// <returns>A new <see cref="PriceHistoryViewModel"/> instance populated with data from the DTO.</returns>
        public static PriceHistoryViewModel ToViewModel(this PriceHistoryDto dto) => new()
        {
            Price = dto.Price,
            Date = dto.Date
        };
    }
}
