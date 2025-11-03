using Microsoft.Extensions.Logging;
using VintageCashCow.ProductPricing.Application.Dtos;
using VintageCashCow.ProductPricing.Application.Exceptions;
using VintageCashCow.ProductPricing.Application.Interfaces;
using VintageCashCow.ProductPricing.Application.Mappers;
using VintageCashCow.ProductPricing.Domain.Data;
using VintageCashCow.ProductPricing.Domain.Interfaces;

namespace VintageCashCow.ProductPricing.Application.Services
{
    /// <summary>
    /// Provides services for managing products, including retrieving, updating, and applying discounts.
    /// </summary>
    /// <remarks>This class implements the <see cref="IProductService"/> interface and acts as a mediator
    /// between the product repository and the application logic. It provides methods to retrieve products, update their
    /// prices, and apply discounts, while ensuring proper validation and logging. Exceptions are thrown for invalid
    /// operations, such as when a product is not found or when invalid input is provided.</remarks>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepository">The repository used to manage product data.</param>
        /// <param name="logger">The logger instance used for logging operations within the service.</param>
        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all products from the repository.
        /// </summary>
        /// <remarks>This method fetches all products available in the repository and converts them to
        /// DTOs. If no products are found, a <see cref="ProductNotFoundException"/> is thrown.</remarks>
        /// <returns>A collection of <see cref="ProductDto"/> objects representing the products in the repository.</returns>
        /// <exception cref="ProductNotFoundException">Thrown when no products are found in the repository.</exception>
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            _logger.LogInformation("Getting all products from repository");
            var products = await _productRepository.GetAllAsync();

            if (!products.Any())
            {
                _logger.LogWarning("No products found in repository");
                throw new ProductNotFoundException("No products found");
            }

            return products.ToDtos();
        }

        /// <summary>
        /// Retrieves the product history for the specified product ID.
        /// </summary>
        /// <remarks>This method logs the operation and throws an exception if the product is not
        /// found.</remarks>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>A <see cref="ProductHistoryDto"/> representing the product's history.</returns>
        /// <exception cref="ProductNotFoundException">Thrown if a product with the specified <paramref name="id"/> does not exist in the repository.</exception>
        public async Task<ProductHistoryDto> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("Getting product by id {ProductId} from repository", id);
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found in repository", id);
                throw new ProductNotFoundException($"Product with id {id} not found");
            }

            return product.ToProductHistoryDto();
        }

        /// <summary>
        /// Updates the price of a product identified by the specified product ID.
        /// </summary>
        /// <remarks>This method retrieves the product by its ID, validates the new price, and updates the
        /// product's price and last updated timestamp. The previous price is recorded in the product's price history.
        /// If the product is not found, or if the new price is invalid, an exception is thrown.</remarks>
        /// <param name="dto">An object containing the product ID and the new price to be set. The new price must be greater than zero.</param>
        /// <returns>A <see cref="ProductDto"/> representing the updated product, including the new price and other product
        /// details.</returns>
        /// <exception cref="InvalidPriceException">Thrown if the new price specified in <paramref name="dto"/> is less than or equal to zero.</exception>
        /// <exception cref="ProductNotFoundException">Thrown if no product with the specified ID exists in the repository.</exception>
        public async Task<ProductDto> UpdateProductPriceAsync(UpdatePriceDto dto)
        {
            _logger.LogInformation("Updating price for product with id {ProductId}", dto.ProductId);
            if (dto.NewPrice <= 0)
            {
                _logger.LogError("Invalid new price {NewPrice} for product with id {ProductId}", dto.NewPrice, dto.ProductId);
                throw new InvalidPriceException("New price must be positive.");
            }

            var product = await _productRepository.GetByIdAsync(dto.ProductId);

            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found when updating price", dto.ProductId);
                throw new ProductNotFoundException($"Product with id {dto.ProductId} not found");
            }

            product.PriceHistory.Add(new PriceHistory { Price = product.Price, Date = product.LastUpdated });
            product.Price = dto.NewPrice;
            product.LastUpdated = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            _logger.LogInformation("Price updated successfully for product with id {ProductId}", dto.ProductId);
            return product.ToDto();
        }

        /// <summary>
        /// Applies a discount to a product based on the provided discount details.
        /// </summary>
        /// <remarks>This method retrieves the product by its ID, validates the discount percentage, and
        /// calculates the discounted price. If the product is not found, or if the discount percentage is invalid, an
        /// exception is thrown.</remarks>
        /// <param name="dto">An object containing the discount details, including the product ID and the discount percentage. The <see
        /// cref="DiscountDto.DiscountPercentage"/> must be between 0 and 100.</param>
        /// <returns>An <see cref="AppliedDiscountDto"/> containing the product details and the newly calculated discounted
        /// price.</returns>
        /// <exception cref="InvalidDiscountException">Thrown if the <see cref="DiscountDto.DiscountPercentage"/> is less than 0 or greater than 100.</exception>
        /// <exception cref="ProductNotFoundException">Thrown if no product is found with the specified <see cref="DiscountDto.ProductId"/>.</exception>
        public async Task<AppliedDiscountDto> ApplyDiscountAsync(DiscountDto dto)
        {
            _logger.LogInformation("Applying discount for product with id {ProductId}", dto.ProductId);
            if (dto.DiscountPercentage < 0 || dto.DiscountPercentage > 100)
            {
                _logger.LogError("Invalid discount percentage {DiscountPercentage} for product with id {ProductId}", dto.DiscountPercentage, dto.ProductId);
                throw new InvalidDiscountException("Discount percentage must be between 0 and 100.");
            }

            var product = await _productRepository.GetByIdAsync(dto.ProductId);

            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found when applying discount", dto.ProductId);
                throw new ProductNotFoundException($"Product with id {dto.ProductId} not found");
            }

            var originalPrice = product.Price;
            var discountedPrice = CalculateDiscountedPrice(dto, originalPrice);

            product.PriceHistory.Add(new PriceHistory { Price = originalPrice, Date = product.LastUpdated });
            product.Price = discountedPrice;
            product.LastUpdated = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);

            return product.ToAppliedDiscountDto(originalPrice, discountedPrice);
        }

        /// <summary>
        /// Calculates the discounted price based on the original price and the discount percentage.
        /// </summary>
        /// <param name="dto">An object containing the discount details, including the discount percentage.</param>
        /// <param name="originalPrice">The original price of the item before applying the discount. Must be a non-negative value.</param>
        /// <returns>The price after applying the discount. Returns the original price if the discount percentage is zero.</returns>
         private static decimal CalculateDiscountedPrice(DiscountDto dto, decimal originalPrice)
        {
            return originalPrice * (1 - dto.DiscountPercentage / 100.0m);
        }
    }
}
