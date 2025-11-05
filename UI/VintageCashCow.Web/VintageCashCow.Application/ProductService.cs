using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using VintageCashCow.Application.Dtos;
using VintageCashCow.Application.Requests;

namespace VintageCashCow.Application
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class with the specified HTTP client and
        /// logger.
        /// </summary>
        /// <remarks>The <paramref name="httpClient"/> parameter is expected to be configured for the API
        /// endpoints that the <see cref="ProductService"/> interacts with. The <paramref name="logger"/> parameter is
        /// used to log diagnostic and error information during the service's operation.</remarks>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance used to send HTTP requests.</param>
        /// <param name="logger">The <see cref="ILogger{TCategoryName}"/> instance used for logging operations.</param>
        public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all products from the remote API.
        /// </summary>
        /// <remarks>This method sends an HTTP GET request to the "api/products" endpoint to fetch the
        /// product data. The response is deserialized into a list of <see cref="ProductDto"/> objects. If the response
        /// is invalid or the product list is not found, an <see cref="InvalidOperationException"/> is thrown.</remarks>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains a list of
        /// <see cref="ProductDto"/> objects representing the products.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the response is invalid or the product list is not found.</exception>
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/products");
                response.EnsureSuccessStatusCode();
                var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();

                if (products is null)
                {
                    throw new InvalidOperationException("Product list not found or response was invalid.");
                }

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all products");
                throw;
            }
        }

        /// <summary>
        /// Updates the price of a product with the specified identifier.
        /// </summary>
        /// <remarks>This method sends an HTTP PUT request to update the product's price. If the operation
        /// is successful, the updated product details are returned. If the product is not found or the response is
        /// invalid, an <see cref="InvalidOperationException"/> is thrown.</remarks>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="newPrice">The new price to set for the product. Must be a non-negative value.</param>
        /// <returns>A <see cref="ProductDto"/> representing the updated product details.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the product is not found or the response is invalid.</exception>
        public async Task<ProductDto> UpdateProductPriceAsync(int id, decimal newPrice)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/products/{id}/update-price", new UpdatePriceRequest() { NewPrice = newPrice });
                response.EnsureSuccessStatusCode();

                var product = await response.Content.ReadFromJsonAsync<ProductDto>();
                if (product is null)
                {
                    throw new InvalidOperationException("Product not found or response was invalid.");
                }

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product price for product {ProductId}", id);
                throw;
            }
        }

        /// <summary>
        /// Applies a discount to the specified product by sending a request to the API.
        /// </summary>
        /// <remarks>This method sends a POST request to the API to apply the specified discount to the
        /// product. The API must return a valid response containing the applied discount details; otherwise, an
        /// exception is thrown.</remarks>
        /// <param name="id">The unique identifier of the product to which the discount will be applied.</param>
        /// <param name="discountPercentage">The percentage of the discount to apply. Must be a positive value.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="AppliedDiscountDto"/> object representing the details of the applied discount.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the API response is invalid or does not contain the applied discount details.</exception>
        public async Task<AppliedDiscountDto> ApplyDiscountToProduct(int id, decimal discountPercentage)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/products/{id}/apply-discount", new DiscountRequest { DiscountPercentage = discountPercentage });
                response.EnsureSuccessStatusCode();

                var appliedDiscount = await response.Content.ReadFromJsonAsync<AppliedDiscountDto>();

                if (appliedDiscount is null)
                {
                    throw new InvalidOperationException("Applied discount not found or response was invalid.");
                }

                return appliedDiscount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying discount for product {ProductId}", id);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the history of a product by its unique identifier.
        /// </summary>
        /// <remarks>This method sends an HTTP GET request to the API endpoint to retrieve the product
        /// history. Ensure that the provided <paramref name="id"/> corresponds to an existing product.</remarks>
        /// <param name="id">The unique identifier of the product whose history is to be retrieved. Must be a positive integer.</param>
        /// <returns>A <see cref="ProductHistoryDto"/> containing the product's history details.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the product is not found or the response is invalid.</exception>
        public async Task<ProductHistoryDto> GetProductHistoryAsync(int id)
        {
            try
            {
                var uri = $"api/products/{id}";
                var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                var product = await response.Content.ReadFromJsonAsync<ProductHistoryDto>();

                if (product is null)
                {

                    throw new InvalidOperationException("Product not found or response was invalid.");
                }

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting product history for product {ProductId}", id);
                throw;
            }
        }
    }
}
