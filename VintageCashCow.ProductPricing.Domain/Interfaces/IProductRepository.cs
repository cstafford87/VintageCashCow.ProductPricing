using VintageCashCow.ProductPricing.Domain.Data;

namespace VintageCashCow.ProductPricing.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing product data, including retrieval and updates.
    /// </summary>
    /// <remarks>This interface provides asynchronous methods for accessing and modifying product information.
    /// Implementations of this interface should handle data persistence and ensure thread safety where
    /// applicable.</remarks>
    public interface IProductRepository
    {
        /// <summary>
        /// Asynchronously retrieves all products.
        /// </summary>
        /// <remarks>This method returns an enumerable collection of all products available in the data
        /// source. The collection will be empty if no products are found.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IEnumerable{T}"/>
        /// of <see cref="Product"/> objects.</returns>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Product"/> with
        /// the specified identifier, or <see langword="null"/> if no product with the given identifier exists.</returns>
        Task<Product> GetByIdAsync(int id);

        /// <summary>
        /// Updates the specified product in the data store asynchronously.
        /// </summary>
        /// <param name="product">The product to update. The product must have a valid identifier and contain the updated information.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(Product product);
    }
}
