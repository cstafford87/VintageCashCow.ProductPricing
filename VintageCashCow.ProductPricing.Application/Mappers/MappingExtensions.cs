using VintageCashCow.ProductPricing.Application.Dtos;
using VintageCashCow.ProductPricing.Domain.Data;

namespace VintageCashCow.ProductPricing.Application.Mappers
{
    /// <summary>
    /// Provides extension methods for converting domain models to their corresponding Data Transfer Object (DTO)
    /// representations.
    /// </summary>
    /// <remarks>This static class includes methods for mapping various domain entities, such as <see
    /// cref="Product"/> and <see cref="PriceHistory"/>, to their respective DTOs, such as <see cref="ProductDto"/> and
    /// <see cref="PriceHistoryDto"/>. These methods are designed to simplify the transformation of data for use in
    /// scenarios like API responses or inter-service communication.</remarks>
    public static class MappingExtensions
    {
        #region ToDto

        /// <summary>
        /// Converts a collection of <see cref="Product"/> objects to a collection of <see cref="ProductDto"/> objects.
        /// </summary>
        /// <param name="products">The collection of <see cref="Product"/> objects to convert. Cannot be <see langword="null"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the converted <see cref="ProductDto"/> objects. If the input
        /// collection is empty, an empty collection is returned.</returns>
        public static IEnumerable<ProductDto> ToDtos(this IEnumerable<Product> products)
        {
            return products.Select(p => p.ToDto());
        }

        /// <summary>
        /// Converts a <see cref="Product"/> instance to a <see cref="ProductDto"/> instance.
        /// </summary>
        /// <param name="product">The <see cref="Product"/> instance to convert. Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="ProductDto"/> instance containing the data from the specified <see cref="Product"/>.</returns>
         public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                LastUpdated = product.LastUpdated,
            };
        }

        /// <summary>
        /// Converts a <see cref="PriceHistory"/> instance to a <see cref="PriceHistoryDto"/> instance.
        /// </summary>
        /// <param name="priceHistory">The <see cref="PriceHistory"/> object to convert. Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="PriceHistoryDto"/> object containing the price and date from the specified <see
        /// cref="PriceHistory"/>.</returns>
        public static PriceHistoryDto ToDto(this PriceHistory priceHistory)
        {
            return new PriceHistoryDto
            {
                Price = priceHistory.Price,
                Date = priceHistory.Date
            };
        }

        /// <summary>
        /// Converts a <see cref="Product"/> instance to a <see cref="ProductHistoryDto"/>.
        /// </summary>
        /// <param name="product">The <see cref="Product"/> instance to convert. Cannot be <c>null</c>.</param>
        /// <returns>A <see cref="ProductHistoryDto"/> containing the product's ID, name, and price history.</returns>
        public static ProductHistoryDto ToProductHistoryDto(this Product product)
        {
            return new ProductHistoryDto
            {
                Id = product.Id,
                Name = product.Name,
                PriceHistory = product.PriceHistory.Select(ph => ph.ToDto()).ToList()
            };
        }

        /// <summary>
        /// Converts the specified <see cref="Product"/> instance to an <see cref="AppliedDiscountDto"/> with the
        /// provided discounted price.
        /// </summary>
        /// <param name="product">The <see cref="Product"/> instance to convert. Cannot be <c>null</c>.</param>
        /// <param name="originalPrice">The original price of the product before the discount was applied.</param>
        /// <param name="discountedPrice">The price of the product after applying the discount.</param>
        /// <returns>An <see cref="AppliedDiscountDto"/> representing the product with its original and discounted prices.</returns>
        public static AppliedDiscountDto ToAppliedDiscountDto(this Product product, decimal originalPrice, decimal discountedPrice)
        {
            return new AppliedDiscountDto
            {
                Id = product.Id,
                Name = product.Name,
                OriginalPrice = originalPrice,
                DiscountedPrice = discountedPrice
            };
        }
        #endregion
    }
}
