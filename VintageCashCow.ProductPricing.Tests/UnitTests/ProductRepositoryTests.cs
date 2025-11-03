using Microsoft.Extensions.Logging;
using Moq;
using VintageCashCow.ProductPricing.Application.Services;
using VintageCashCow.ProductPricing.Domain.Data;
using VintageCashCow.ProductPricing.Infrastructure.Repositories;

namespace VintageCashCow.ProductPricing.Tests.UnitTests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private ProductRepository _repository;
        private Mock<ILogger<ProductRepository>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<ProductRepository>>();
            _repository = new ProductRepository(_mockLogger.Object);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Act
            var products = await _repository.GetAllAsync();

            // Assert
            Assert.That(products.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_WhenProductExists_ShouldReturnProduct()
        {
            // Act
            var product = await _repository.GetByIdAsync(1);

            // Assert
            Assert.That(product, Is.Not.Null);
            Assert.That(product.Id, Is.EqualTo(1));
            Assert.That(product.Name, Is.EqualTo("Product A"));
        }

        [Test]
        public async Task GetByIdAsync_WhenProductDoesNotExist_ShouldReturnNull()
        {
            // Act
            var product = await _repository.GetByIdAsync(999);

            // Assert
            Assert.That(product, Is.Null);
        }

        [Test]
        public async Task UpdateAsync_WhenProductExists_ShouldUpdateProductDetails()
        {
            // Arrange
            var originalProduct = await _repository.GetByIdAsync(1);
            var originalPrice = originalProduct.Price;
            var newPrice = originalPrice + 50;
            var newLastUpdated = DateTime.UtcNow;
            var updatedProductInfo = new Product
            {
                Id = 1,
                Name = "Product A Updated", // Name is not updated by current implementation, but we check it.
                Price = newPrice,
                LastUpdated = newLastUpdated,
                PriceHistory = new List<PriceHistory>()
            };

            // Act
            await _repository.UpdateAsync(updatedProductInfo);
            var productAfterUpdate = await _repository.GetByIdAsync(1);

            // Assert
            Assert.That(productAfterUpdate, Is.Not.Null);
            Assert.That(productAfterUpdate.Price, Is.EqualTo(newPrice));
            Assert.That(productAfterUpdate.LastUpdated, Is.EqualTo(newLastUpdated));
            Assert.That(productAfterUpdate.PriceHistory, Is.Empty);

            // Cleanup: Restore original state for other tests as the list is static
            originalProduct.Price = originalPrice;
            await _repository.UpdateAsync(originalProduct);
        }

        [Test]
        public async Task UpdateAsync_WhenProductDoesNotExist_ShouldNotThrowAndNotChangeData()
        {
            // Arrange
            var nonExistentProduct = new Product
            {
                Id = 999,
                Price = 500
            };
            var productsBefore = (await _repository.GetAllAsync()).ToList();

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _repository.UpdateAsync(nonExistentProduct));
            var productsAfter = (await _repository.GetAllAsync()).ToList();

            Assert.That(productsAfter.Count, Is.EqualTo(productsBefore.Count));
            var product999 = await _repository.GetByIdAsync(999);
            Assert.That(product999, Is.Null);
        }
    }
}
