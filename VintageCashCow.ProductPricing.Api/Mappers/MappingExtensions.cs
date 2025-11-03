using VintageCashCow.ProductPricing.Api.Requests;
using VintageCashCow.ProductPricing.Application.Dtos;

namespace VintageCashCow.ProductPricing.Api.Mappers
{
    /// <summary>
    /// Provides extension methods for mapping request objects to their corresponding DTO (Data Transfer Object)
    /// representations.
    /// </summary>
    /// <remarks>This static class contains methods to convert domain-specific request objects, such as <see
    /// cref="DiscountRequest"/> and <see cref="UpdatePriceRequest"/>, into their respective DTOs, <see
    /// cref="DiscountDto"/> and <see cref="UpdatePriceDto"/>. These methods simplify the transformation of data for use
    /// in application layers or external systems.</remarks>
    public static class MappingExtensions
    {
        #region ToDto

        /// <summary>
        /// Converts a <see cref="DiscountRequest"/> object to a <see cref="DiscountDto"/> object.
        /// </summary>
        /// <param name="discountRequest">The source <see cref="DiscountRequest"/> containing the discount details.</param>
        /// <param name="id">The product identifier to associate with the resulting <see cref="DiscountDto"/>.</param>
        /// <returns>A <see cref="DiscountDto"/> object populated with the specified product identifier and discount percentage.</returns>
        public static DiscountDto ToDto(this DiscountRequest discountRequest, int id)
        {
            return new DiscountDto 
            { 
                ProductId = id, 
                DiscountPercentage = discountRequest.DiscountPercentage
            };
        }

        /// <summary>
        /// Converts an <see cref="UpdatePriceRequest"/> object to an <see cref="UpdatePriceDto"/> object.
        /// </summary>
        /// <param name="updatePriceRequest">The request object containing the new price information.</param>
        /// <param name="id">The unique identifier of the product to be updated.</param>
        /// <returns>An <see cref="UpdatePriceDto"/> object containing the product ID and the new price.</returns>
        public static UpdatePriceDto ToDto(this UpdatePriceRequest updatePriceRequest, int id)
        {
            return new UpdatePriceDto
            {
                ProductId = id,
                NewPrice = updatePriceRequest.NewPrice
            };
        }
        #endregion
    }
}
