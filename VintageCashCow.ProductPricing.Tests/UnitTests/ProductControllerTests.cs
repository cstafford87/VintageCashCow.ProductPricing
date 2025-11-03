using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VintageCashCow.ProductPricing.Api.Controllers;
using VintageCashCow.ProductPricing.Api.Requests;
using VintageCashCow.ProductPricing.Application.Dtos;
using VintageCashCow.ProductPricing.Application.Exceptions;
using VintageCashCow.ProductPricing.Application.Interfaces;

namespace VintageCashCow.ProductPricing.Tests.UnitTests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> _mockProductService;
        private ProductsController _controller;
        private Mock<ILogger<ProductsController>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockProductService = new Mock<IProductService>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockProductService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetProducts_ShouldReturnOkResult_WithListOfProducts()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product A", Price = 100 },
                new ProductDto { Id = 2, Name = "Product B", Price = 200 }
            };
            _mockProductService.Setup(service => service.GetProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returnValue = okResult.Value as IEnumerable<ProductDto>;
            Assert.That(returnValue.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetProducts_ShouldReturnNotFound_WhenNoProductsExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductsAsync()).ThrowsAsync(new ProductNotFoundException("No products found"));

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.Value, Is.EqualTo("No products found"));
        }

        [Test]
        public async Task GetProductPriceHistory_ShouldReturnOkResult_WhenProductExists()
        {
            // Arrange
            var product = new ProductHistoryDto { Id = 1, Name = "Product A", PriceHistory = new List<PriceHistoryDto>() };
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductPriceHistory(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.TypeOf<ProductHistoryDto>());
        }

        [Test]
        public async Task GetProductPriceHistory_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ThrowsAsync(new ProductNotFoundException("Product not found"));

            // Act
            var result = await _controller.GetProductPriceHistory(1);

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task ApplyDiscount_ShouldReturnOkResult_WhenRequestIsValid()
        {
            // Arrange
            var discountRequest = new DiscountRequest { DiscountPercentage = 10 };
            var appliedDiscount = new AppliedDiscountDto { Id = 1, Name = "Product A", OriginalPrice = 100, DiscountedPrice = 90 };
            _mockProductService.Setup(service => service.ApplyDiscountAsync(It.IsAny<DiscountDto>())).ReturnsAsync(appliedDiscount);

            // Act
            var result = await _controller.ApplyDiscount(1, discountRequest);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.TypeOf<AppliedDiscountDto>());
            var resultValue = okResult.Value as AppliedDiscountDto;
            Assert.That(resultValue.DiscountedPrice, Is.EqualTo(90));
        }

        [Test]
        public async Task ApplyDiscount_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var discountRequest = new DiscountRequest { DiscountPercentage = 10 };
            _mockProductService.Setup(s => s.ApplyDiscountAsync(It.IsAny<DiscountDto>())).ThrowsAsync(new ProductNotFoundException("Product not found"));

            // Act
            var result = await _controller.ApplyDiscount(1, discountRequest);

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task ApplyDiscount_ShouldReturnBadRequest_WhenInvalidDiscountExceptionIsThrown()
        {
            // Arrange
            var discountRequest = new DiscountRequest { DiscountPercentage = 110 };
            _mockProductService.Setup(s => s.ApplyDiscountAsync(It.IsAny<DiscountDto>())).ThrowsAsync(new InvalidDiscountException("Invalid discount"));

            // Act
            var result = await _controller.ApplyDiscount(1, discountRequest);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid discount"));
        }

        [Test]
        public async Task UpdatePrice_ShouldReturnOkResult_WhenRequestIsValid()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product A", Price = 200 };
            var updateRequest = new UpdatePriceRequest { NewPrice = 200 };

            _mockProductService.Setup(service => service.UpdateProductPriceAsync(It.IsAny<UpdatePriceDto>())).ReturnsAsync(productDto);

            // Act
            var result = await _controller.UpdatePrice(1, updateRequest);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.TypeOf<ProductDto>());
        }

        [Test]
        public async Task UpdatePrice_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var updateRequest = new UpdatePriceRequest { NewPrice = 150 };

            _mockProductService.Setup(service => service.UpdateProductPriceAsync(It.IsAny<UpdatePriceDto>())).ThrowsAsync(new ProductNotFoundException("Product not found"));

            // Act
            var result = await _controller.UpdatePrice(1, updateRequest);

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task UpdatePrice_ShouldReturnBadRequest_WhenInvalidPriceExceptionIsThrown()
        {
            // Arrange
            var updateRequest = new UpdatePriceRequest { NewPrice = -10 };

            _mockProductService.Setup(service => service.UpdateProductPriceAsync(It.IsAny<UpdatePriceDto>())).ThrowsAsync(new InvalidPriceException("Invalid price"));

            // Act
            var result = await _controller.UpdatePrice(1, updateRequest);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid price"));
        }
    }
}
