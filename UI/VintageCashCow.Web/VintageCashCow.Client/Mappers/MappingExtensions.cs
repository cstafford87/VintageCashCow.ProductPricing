using VintageCashCow.Application.Dtos;
using VintageCashCow.Client.Models;

namespace VintageCashCow.Client.Mappers;

/// <summary>
/// Provides extension methods for mapping between DTOs and ViewModels.
/// </summary>
/// <remarks>
/// This class contains methods to convert data transfer objects (DTOs) from the API
/// into view models used by the UI components.
/// </remarks>
public static class MappingExtensions
{
    /// <summary>
    /// Converts a <see cref="ProductDto"/> to a <see cref="ProductViewModel"/>.
    /// </summary>
    /// <param name="dto">The product DTO to convert.</param>
    /// <returns>A new <see cref="ProductViewModel"/> instance populated with data from the DTO.</returns>
    /// <remarks>
    /// This method initializes the <see cref="ProductViewModel.PriceUpdate"/> and
    /// <see cref="ProductViewModel.DiscountUpdate"/> properties with new instances.
    /// </remarks>
    public static ProductViewModel ToViewModel(this ProductDto dto)
    {
        return new ProductViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            LastUpdated = dto.LastUpdated,
            PriceUpdate = new PriceUpdateViewModel(),
            DiscountUpdate = new DiscountViewModel()
        };
    }

    /// <summary>
    /// Converts a <see cref="ProductHistoryDto"/> to a <see cref="ProductHistoryViewModel"/>.
    /// </summary>
    /// <param name="dto">The product history DTO to convert.</param>
    /// <returns>A new <see cref="ProductHistoryViewModel"/> instance populated with data from the DTO.</returns>
    /// <remarks>
    /// This method converts all price history entries from DTOs to view models using
    /// the <see cref="ToViewModel(PriceHistoryDto)"/> extension method.
    /// </remarks>
    public static ProductHistoryViewModel ToViewModel(this ProductHistoryDto dto)
    {
        return new ProductHistoryViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            PriceHistory = dto.PriceHistory.Select(ph => ph.ToViewModel()).ToList()
        };
    }

    /// <summary>
    /// Converts a <see cref="PriceHistoryDto"/> to a <see cref="PriceHistoryViewModel"/>.
    /// </summary>
    /// <param name="dto">The price history DTO to convert.</param>
    /// <returns>A new <see cref="PriceHistoryViewModel"/> instance populated with data from the DTO.</returns>
    public static PriceHistoryViewModel ToViewModel(this PriceHistoryDto dto)
    {
        return new PriceHistoryViewModel
        {
            Price = dto.Price,
            Date = dto.Date
        };
    }
}
