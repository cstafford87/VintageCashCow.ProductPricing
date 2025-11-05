using System.Net;
using System.Net.Http.Json;
using Moq;
using VintageCashCow.ProductPricing.Api.Requests;
using VintageCashCow.ProductPricing.Application.Dtos;
using VintageCashCow.ProductPricing.Application.Exceptions;
using VintageCashCow.ProductPricing.Application.Interfaces;

namespace VintageCashCow.ProductPricing.Tests.IntegrationTests
{
    [TestFixture]
    public class ProductApiIntegrationTests
    {
        private ProductApiWebApplicationFactory _factory;
        private HttpClient _client;
        private Mock<IProductService> _productServiceMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new ProductApiWebApplicationFactory();
        }

        [SetUp]
        public void SetUp()
        {
            _client = _factory.CreateClient();
            _productServiceMock = _factory.ProductServiceMock;
        }

        [Test]
        public async Task GetProducts_ReturnsSuccessAndListOfProducts()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new() { Id = 1, Name = "Product A", Price = 100.0m },
                new() { Id = 2, Name = "Product B", Price = 200.0m }
            };
            _productServiceMock.Setup(s => s.GetProductsAsync()).ReturnsAsync(products);

            // Act
            var response = await _client.GetAsync("api/Products");

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedProducts = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            Assert.That(returnedProducts, Is.Not.Null);
            Assert.That(returnedProducts, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task GetProductPriceHistory_ReturnsProductHistory_WhenProductExists()
        {
            // Arrange
            var product = new ProductHistoryDto { Id = 1, Name = "Product A", PriceHistory = new List<PriceHistoryDto>() };
            _productServiceMock.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var response = await _client.GetAsync("api/Products/1");

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedProduct = await response.Content.ReadFromJsonAsync<ProductHistoryDto>();
            Assert.That(returnedProduct, Is.Not.Null);
            Assert.That(returnedProduct.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetProductPriceHistory_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _productServiceMock.Setup(s => s.GetProductByIdAsync(It.IsAny<int>())).ThrowsAsync(new ProductNotFoundException("Product not found"));

            // Act
            var response = await _client.GetAsync("api/Products/999");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task ApplyDiscount_ReturnsOkWithCorrectData_WhenRequestIsValid()
        {
            // Arrange
            var request = new DiscountRequest { DiscountPercentage = 10 };
            var resultDto = new AppliedDiscountDto { OriginalPrice = 100.0m, DiscountedPrice = 90.0m };

            _productServiceMock.Setup(s => s.ApplyDiscountAsync(It.IsAny<DiscountDto>())).ReturnsAsync(resultDto);

            // Act
            var response = await _client.PostAsJsonAsync("api/Products/1/apply-discount", request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AppliedDiscountDto>();
            Assert.That(result.OriginalPrice, Is.EqualTo(100.0m));
            Assert.That(result.DiscountedPrice, Is.EqualTo(90.0m));
        }

        [Test]
        public async Task ApplyDiscount_ReturnsBadRequest_WhenDiscountIsInvalid()
        {
            // Arrange
            var request = new DiscountRequest { DiscountPercentage = 110 };
            _productServiceMock.Setup(s => s.ApplyDiscountAsync(It.IsAny<DiscountDto>())).ThrowsAsync(new InvalidDiscountException("Invalid discount"));

            // Act
            var response = await _client.PostAsJsonAsync("api/Products/1/apply-discount", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UpdatePrice_ReturnsOkWithUpdatedProduct_WhenRequestIsValid()
        {
            // Arrange
            var request = new UpdatePriceRequest { NewPrice = 150 };
            var updatedProduct = new ProductDto { Id = 1, Name = "Updated Product", Price = 150 };

            _productServiceMock.Setup(s => s.UpdateProductPriceAsync(It.IsAny<UpdatePriceDto>())).ReturnsAsync(updatedProduct);

            // Act
            var response = await _client.PutAsJsonAsync("api/Products/1/update-price", request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Price, Is.EqualTo(150m));
        }

        [Test]
        public async Task UpdatePrice_ReturnsBadRequest_WhenPriceIsInvalid()
        {
            // Arrange
            var request = new UpdatePriceRequest { NewPrice = -10 };
            _productServiceMock.Setup(s => s.UpdateProductPriceAsync(It.IsAny<UpdatePriceDto>())).ThrowsAsync(new InvalidPriceException("Invalid price"));

            // Act
            var response = await _client.PutAsJsonAsync("api/Products/1/update-price", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Reset();
            _client.Dispose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory.Dispose();
        }
    }
}
