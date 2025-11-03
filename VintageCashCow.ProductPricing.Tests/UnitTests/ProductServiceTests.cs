using Microsoft.Extensions.Logging;
using Moq;
using VintageCashCow.ProductPricing.Application.Dtos;
using VintageCashCow.ProductPricing.Application.Exceptions;
using VintageCashCow.ProductPricing.Application.Interfaces;
using VintageCashCow.ProductPricing.Application.Services;
using VintageCashCow.ProductPricing.Domain.Data;
using VintageCashCow.ProductPricing.Domain.Interfaces;

namespace VintageCashCow.ProductPricing.Tests.UnitTests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private IProductService _productService;
        private Product _product;
        private Mock<ILogger<ProductService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _productService = new ProductService(_mockProductRepository.Object, _mockLogger.Object);
            _product = new Product { Id = 1, Name = "Product A", Price = 100, PriceHistory = new List<PriceHistory>() };
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(_product);
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsNotIn(1))).ReturnsAsync((Product)null);
        }

        [Test]
        public async Task GetProductsAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                _product,
                new Product { Id = 2, Name = "Product B", Price = 200 }
            };
            _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetProductsAsync_ShouldThrowProductNotFoundException_WhenNoProductsExist()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act & Assert
            Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.GetProductsAsync());
        }

        [Test]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ProductHistoryDto>());
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void GetProductByIdAsync_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Act & Assert
            Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.GetProductByIdAsync(999));
        }

        [Test]
        public async Task UpdateProductPriceAsync_ShouldUpdatePrice_WhenProductExists()
        {
            // Arrange
            var updateDto = new UpdatePriceDto { ProductId = 1, NewPrice = 150 };
            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _productService.UpdateProductPriceAsync(updateDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Price, Is.EqualTo(150));
            _mockProductRepository.Verify(r => r.UpdateAsync(It.Is<Product>(p => p.Id == 1 && p.Price == 150)), Times.Once);
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void UpdateProductPriceAsync_ShouldThrowInvalidPriceException_WhenPriceIsInvalid(decimal invalidPrice)
        {
            // Arrange
            var updateDto = new UpdatePriceDto { ProductId = 1, NewPrice = invalidPrice };

            // Act & Assert
            Assert.ThrowsAsync<InvalidPriceException>(() => _productService.UpdateProductPriceAsync(updateDto));
        }

        [Test]
        public void UpdateProductPriceAsync_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var updateDto = new UpdatePriceDto { ProductId = 999, NewPrice = 150 };

            // Act & Assert
            Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.UpdateProductPriceAsync(updateDto));
        }

        [TestCase(0, 100)]
        [TestCase(10, 90)]
        [TestCase(100, 0)]
        public async Task ApplyDiscountAsync_ShouldReturnCorrectlyDiscountedPrice_WhenDiscountIsValid(decimal discount, decimal expectedPrice)
        {
            // Arrange
            var discountDto = new DiscountDto { ProductId = 1, DiscountPercentage = discount };
            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _productService.ApplyDiscountAsync(discountDto);

            // Assert
            Assert.That(result.OriginalPrice, Is.EqualTo(100));
            Assert.That(result.DiscountedPrice, Is.EqualTo(expectedPrice));
            _mockProductRepository.Verify(r => r.UpdateAsync(It.Is<Product>(p => p.Id == 1 && p.Price == expectedPrice)), Times.Once);
        }

        [TestCase(-1)]
        [TestCase(101)]
        public void ApplyDiscountAsync_ShouldThrowInvalidDiscountException_WhenDiscountIsInvalid(decimal invalidDiscount)
        {
            // Arrange
            var discountDto = new DiscountDto { ProductId = 1, DiscountPercentage = invalidDiscount };

            // Act & Assert
            Assert.ThrowsAsync<InvalidDiscountException>(() => _productService.ApplyDiscountAsync(discountDto));
        }

        [Test]
        public void ApplyDiscountAsync_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var discountDto = new DiscountDto { ProductId = 999, DiscountPercentage = 10 };

            // Act & Assert
            Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.ApplyDiscountAsync(discountDto));
        }
    }
}
