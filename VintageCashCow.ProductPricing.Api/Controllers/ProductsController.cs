using Microsoft.AspNetCore.Mvc;
using VintageCashCow.ProductPricing.Api.Mappers;
using VintageCashCow.ProductPricing.Api.Requests;
using VintageCashCow.ProductPricing.Application.Dtos;
using VintageCashCow.ProductPricing.Application.Exceptions;
using VintageCashCow.ProductPricing.Application.Interfaces;

namespace VintageCashCow.ProductPricing.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for managing products, including retrieving product details, applying discounts, updating
    /// prices, and fetching price history.
    /// </summary>
    /// <remarks>This controller handles HTTP requests related to product operations. It interacts with the
    /// <see cref="IProductService"/> to perform business logic and returns appropriate HTTP responses based on the
    /// operation's outcome. Common responses include 200 OK for successful operations, 404 Not Found for missing
    /// resources, and 400 Bad Request for invalid input.</remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="productService">The service used to manage product-related operations. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="logger">The logger instance used to log diagnostic and operational information. This parameter cannot be <see
        /// langword="null"/>.</param>
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a collection of all available products.
        /// </summary>
        /// <remarks>This method fetches all products from the data source and returns them as a
        /// collection of <see cref="ProductDto"/> objects. If no products are found, a 404 Not Found response is
        /// returned.</remarks>
        /// <returns>An <see cref="ActionResult"/> containing an <see cref="IEnumerable{T}"/> of <see cref="ProductDto"/> objects
        /// if products are found; otherwise, a 404 Not Found response.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            _logger.LogInformation("Getting all products");
            try
            {
                var products = await _productService.GetProductsAsync();
                return Ok(products);
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogWarning(ex, "No products found");
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the price history of a product based on its unique identifier.
        /// </summary>
        /// <remarks>This method logs the operation and handles cases where the product is not found by
        /// returning a 404 status code.</remarks>
        /// <param name="id">The unique identifier of the product whose price history is to be retrieved.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="ProductHistoryDto"/> object with the product's
        /// price history if the product is found; otherwise, a <see cref="NotFoundResult"/> with an appropriate error
        /// message.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductHistoryDto>> GetProductPriceHistory(int id)
        {
            _logger.LogInformation("Getting product price history for product with id {ProductId}", id);
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogWarning(ex, "Product with id {ProductId} not found", id);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Applies a discount to the specified product.
        /// </summary>
        /// <remarks>This method logs the discount application process and handles exceptions for invalid
        /// products or discount requests.</remarks>
        /// <param name="id">The unique identifier of the product to which the discount will be applied.</param>
        /// <param name="request">The discount details, including the discount amount or percentage, provided in the request body.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="AppliedDiscountDto"/> object with the details of
        /// the applied discount if successful. Returns a <see cref="NotFoundResult"/> if the product with the specified
        /// <paramref name="id"/> does not exist. Returns a <see cref="BadRequestResult"/> if the discount details in
        /// the <paramref name="request"/> are invalid.</returns>
        [HttpPost("{id}/apply-discount")]
        public async Task<ActionResult<AppliedDiscountDto>> ApplyDiscount(int id, [FromBody] DiscountRequest request)
        {
            _logger.LogInformation("Applying discount for product with id {ProductId}", id);
            try
            {
                var result = await _productService.ApplyDiscountAsync(request.ToDto(id));

                _logger.LogInformation("Discount applied successfully for product with id {ProductId}", id);
                return Ok(result);
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogWarning(ex, "Product with id {ProductId} not found when applying discount", id);
                return NotFound(ex.Message);
            }
            catch (InvalidDiscountException ex)
            {
                _logger.LogError(ex, "Error applying discount for product with id {ProductId}", id);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates the price of a product with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product whose price is to be updated.</param>
        /// <param name="request">The request containing the new price details.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the updated product details as a <see cref="ProductDto"/>.
        /// Returns a 200 OK response if the price is updated successfully. Returns a 404 Not Found response if the
        /// product with the specified identifier does not exist. Returns a 400 Bad Request response if the provided
        /// price is invalid.</returns>
        [HttpPut("{id}/update-price")]
        public async Task<ActionResult<ProductDto>> UpdatePrice(int id, [FromBody] UpdatePriceRequest request)
        {
            _logger.LogInformation("Updating price for product with id {ProductId}", id);
            try
            {
                var updatePriceDto = request.ToDto(id);

                var product = await _productService.UpdateProductPriceAsync(updatePriceDto);

                _logger.LogInformation("Price updated successfully for product with id {ProductId}", id);
                return Ok(product);
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogWarning(ex, "Product with id {ProductId} not found when updating price", id);
                return NotFound(ex.Message);
            }
            catch (InvalidPriceException ex)
            {
                _logger.LogError(ex, "Error updating price for product with id {ProductId}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
