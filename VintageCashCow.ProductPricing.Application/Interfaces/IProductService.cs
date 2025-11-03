using VintageCashCow.ProductPricing.Application.Dtos;
using VintageCashCow.ProductPricing.Domain.Data;

namespace VintageCashCow.ProductPricing.Application.Interfaces
{
    /// <summary>
    /// Defines a contract for managing product-related operations, including retrieving products,  updating product
    /// prices, and applying discounts.
    /// </summary>
    /// <remarks>This interface provides methods for asynchronous operations on products, such as retrieving
    /// product  details, updating prices, and applying discounts. It ensures that product price changes and discounts 
    /// are properly recorded and calculated. Implementations of this interface should handle validation of  input
    /// parameters and ensure thread safety for concurrent operations.</remarks>
    public interface IProductService
    {
        /// <summary>
        /// Asynchronously retrieves a collection of products.
        /// </summary>
        /// <remarks>This method returns a collection of products as ProductDto objects.  The collection
        /// may be empty if no products are available.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IEnumerable{T} of ProductDto
        /// representing the products.</returns>
        Task<IEnumerable<ProductDto>> GetProductsAsync();

        /// <summary>
        /// Retrieves the product history details for the specified product ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="ProductHistoryDto"/> object with the product's history details, or <see langword="null"/> if the
        /// product is not found.</returns>
        Task<ProductHistoryDto> GetProductByIdAsync(int id);

        /// <summary>
        /// Updates the price of a product based on the provided data transfer object.
        /// </summary>
        /// <param name="dto">An object containing the product identifier and the new price to be applied.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ProductDto"/>
        /// representing the updated product details, including the new price.</returns>
        Task<ProductDto> UpdateProductPriceAsync(UpdatePriceDto dto);

        /// <summary>
        /// Applies the specified discount to the relevant entity and returns the result.
        /// </summary>
        /// <param name="dto">The discount details to be applied, including the discount type and value.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see
        /// cref="AppliedDiscountDto"/> object with details of the applied discount.</returns>
        Task<AppliedDiscountDto> ApplyDiscountAsync(DiscountDto dto);
    }
}
