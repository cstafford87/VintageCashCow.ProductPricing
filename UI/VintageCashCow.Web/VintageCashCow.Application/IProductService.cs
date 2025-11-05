using System.Threading.Tasks;
using VintageCashCow.Application.Dtos;
using VintageCashCow.Application.Requests;

namespace VintageCashCow.Application
{
    /// <summary>
    /// Defines a contract for managing product-related operations, including retrieving, updating, and applying
    /// discounts to products.
    /// </summary>
    /// <remarks>This interface provides asynchronous methods for interacting with product data, such as
    /// retrieving all products, updating product prices, applying discounts, and fetching product history.
    /// Implementations of this interface should ensure thread safety and proper exception handling for operations that
    /// involve data access or modification.</remarks>
    public interface IProductService
    {
        /// <summary>
        /// Asynchronously retrieves a list of all available products.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see
        /// cref="ProductDto"/> objects representing the products. The list will be empty if no products are available.</returns>
        Task<List<ProductDto>> GetAllProductsAsync();

        /// <summary>
        /// Updates the price of a product with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="newPrice">The new price to set for the product. Must be a non-negative value.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ProductDto"/>
        /// representing the updated product, or <c>null</c> if the product with the specified identifier does not
        /// exist.</returns>
        Task<ProductDto> UpdateProductPriceAsync(int id, decimal newPrice);

        /// <summary>
        /// Applies a discount to the specified product.
        /// </summary>
        /// <remarks>This method updates the product's price based on the specified discount percentage.
        /// The discount is applied as a reduction from the product's current price. Ensure that the product ID is valid
        /// and the discount percentage is within the acceptable range before calling this method.</remarks>
        /// <param name="id">The unique identifier of the product to which the discount will be applied.</param>
        /// <param name="discountPercentage">The percentage of the discount to apply. Must be a value between 0 and 100.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="AppliedDiscountDto"/> object with details about the applied discount.</returns>
        Task<AppliedDiscountDto> ApplyDiscountToProduct(int id, decimal discountPercentage);

        /// <summary>
        /// Retrieves the history of a product based on its unique identifier.
        /// </summary>
        /// <remarks>This method is asynchronous and should be awaited. Ensure the product ID provided is
        /// valid and exists in the system.</remarks>
        /// <param name="id">The unique identifier of the product whose history is to be retrieved. Must be a positive integer.</param>
        /// <returns>A <see cref="ProductHistoryDto"/> containing the product's history details, or <see langword="null"/> if no
        /// product with the specified identifier exists.</returns>
        Task<ProductHistoryDto> GetProductHistoryAsync(int id);
    }
}
