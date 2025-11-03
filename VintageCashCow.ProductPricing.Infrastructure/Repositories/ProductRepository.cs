using Microsoft.Extensions.Logging;
using VintageCashCow.ProductPricing.Domain.Data;
using VintageCashCow.ProductPricing.Domain.Interfaces;

namespace VintageCashCow.ProductPricing.Infrastructure.Repositories
{
    /// <summary>
    /// Provides methods for managing and retrieving product data, including fetching all products, retrieving a product
    /// by its identifier, and updating product information.
    /// </summary>
    /// <remarks>This repository serves as an in-memory data store for products, simulating basic CRUD
    /// operations. It maintains a static list of products and their associated price history. The repository is
    /// designed for demonstration purposes and does not persist data beyond the application's lifetime.</remarks>
    public class ProductRepository : IProductRepository
    {
        /// <summary>
        /// Represents a static collection of predefined products, each containing details such as  ID, name, current
        /// price, last updated timestamp, and price history.
        /// </summary>
        /// <remarks>This collection is initialized with a predefined set of products for demonstration or
        /// testing purposes.  Each product includes a history of price changes, represented as a list of <see
        /// cref="PriceHistory"/> objects. The <see cref="DateTime.UtcNow"/> value is used to set the last updated
        /// timestamp for the products.</remarks>
        private static readonly List<Product> _products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Product A",
                Price = 100.0m,
                LastUpdated = DateTime.UtcNow,
                PriceHistory = new List<PriceHistory>
                {
                    new PriceHistory { Price = 120.0m, Date = new DateTime(2024, 9, 1) },
                    new PriceHistory { Price = 110.0m, Date = new DateTime(2024, 8, 15) },
                    new PriceHistory { Price = 100.0m, Date = new DateTime(2024, 8, 10) }
                }
            },
            new Product
            {
                Id = 2,
                Name = "Product B",
                Price = 200.0m,
                LastUpdated = DateTime.UtcNow.AddDays(-1),
                PriceHistory = new List<PriceHistory>
                {
                    new PriceHistory { Price = 220.0m, Date = new DateTime(2024, 9, 1) },
                    new PriceHistory { Price = 210.0m, Date = new DateTime(2024, 8, 15) }
                }
            }
        };

        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ILogger<ProductRepository> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Asynchronously retrieves all products.
        /// </summary>
        /// <remarks>The returned collection reflects the current state of the product list. If no
        /// products are available, the collection will be empty.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/>
        /// of <see cref="Product"/> objects representing all available products.</returns>
        public Task<IEnumerable<Product>> GetAllAsync()
        {
            _logger.LogInformation("Getting all products from in-memory data store");
            return Task.FromResult<IEnumerable<Product>>(_products);
        }

        /// <summary>
        /// Retrieves a product with the specified identifier.
        /// </summary>
        /// <remarks>The search is performed on the in-memory collection of products. If multiple products
        /// share the same identifier, the first matching product is returned.</remarks>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Product"/> with
        /// the specified identifier, or <see langword="null"/> if no product is found.</returns>
        public Task<Product> GetByIdAsync(int id)
        {
            _logger.LogInformation("Getting product by id {ProductId} from in-memory data store", id);
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found in in-memory data store", id);
            }
            return Task.FromResult(product);
        }

        /// <summary>
        /// Updates the details of an existing product in the collection.
        /// </summary>
        /// <remarks>If a product with the specified <see cref="Product.Id"/> is found, its price, last
        /// updated timestamp, and price history are updated to match the provided product. If no matching product is
        /// found, the operation completes without making any changes.</remarks>
        /// <param name="product">The product containing updated information. The product's <see cref="Product.Id"/> must match an existing
        /// product in the collection.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task UpdateAsync(Product product)
        {
            _logger.LogInformation("Updating product with id {ProductId} in in-memory data store", product.Id);
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Price = product.Price;
                existingProduct.LastUpdated = product.LastUpdated;
                existingProduct.PriceHistory = product.PriceHistory;
                _logger.LogInformation("Product with id {ProductId} updated successfully", product.Id);
            }
            else
            {
                _logger.LogWarning("Product with id {ProductId} not found for update", product.Id);
            }
            return Task.CompletedTask;
        }
    }
}
