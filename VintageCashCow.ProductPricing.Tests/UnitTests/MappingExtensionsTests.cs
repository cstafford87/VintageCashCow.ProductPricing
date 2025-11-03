using VintageCashCow.ProductPricing.Api.Mappers;
using VintageCashCow.ProductPricing.Api.Requests;
using VintageCashCow.ProductPricing.Application.Dtos;
using VintageCashCow.ProductPricing.Application.Mappers;
using VintageCashCow.ProductPricing.Domain.Data;

namespace VintageCashCow.ProductPricing.Tests.UnitTests
{
    [TestFixture]
    public class MappingExtensionsTests
    {
        [Test]
        public void ToDto_ConvertsProductToProductDto_Correctly()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 100.0m,
                LastUpdated = DateTime.UtcNow
            };

            // Act
            var productDto = product.ToDto();

            // Assert
            Assert.That(productDto, Is.Not.Null);
            Assert.That(productDto.Id, Is.EqualTo(product.Id));
            Assert.That(productDto.Name, Is.EqualTo(product.Name));
            Assert.That(productDto.Price, Is.EqualTo(product.Price));
            Assert.That(productDto.LastUpdated, Is.EqualTo(product.LastUpdated));
        }

        [Test]
        public void ToDtos_ConvertsListOfProductsToListOfProductDtos_Correctly()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product A", Price = 100.0m },
                new() { Id = 2, Name = "Product B", Price = 200.0m }
            };

            // Act
            var productDtos = products.ToDtos();

            // Assert
            Assert.That(productDtos, Is.Not.Null);
            Assert.That(productDtos.Count(), Is.EqualTo(2));
            Assert.That(productDtos.First().Name, Is.EqualTo("Product A"));
        }

        [Test]
        public void ToDtos_WithEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var products = new List<Product>();

            // Act
            var productDtos = products.ToDtos();

            // Assert
            Assert.That(productDtos, Is.Not.Null);
            Assert.That(productDtos.Any(), Is.False);
        }

        [Test]
        public void ToPriceHistoryDto_ConvertsPriceHistoryToPriceHistoryDto_Correctly()
        {
            // Arrange
            var priceHistory = new PriceHistory
            {
                Price = 50.0m,
                Date = DateTime.UtcNow
            };

            // Act
            var priceHistoryDto = priceHistory.ToDto();

            // Assert
            Assert.That(priceHistoryDto, Is.Not.Null);
            Assert.That(priceHistoryDto.Price, Is.EqualTo(priceHistory.Price));
            Assert.That(priceHistoryDto.Date, Is.EqualTo(priceHistory.Date));
        }

        [Test]
        public void ToProductHistoryDto_ConvertsProductToProductHistoryDto_Correctly()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                PriceHistory = new List<PriceHistory>
                {
                    new() { Price = 90.0m, Date = DateTime.UtcNow.AddDays(-1) }
                }
            };

            // Act
            var productHistoryDto = product.ToProductHistoryDto();

            // Assert
            Assert.That(productHistoryDto, Is.Not.Null);
            Assert.That(productHistoryDto.Id, Is.EqualTo(product.Id));
            Assert.That(productHistoryDto.Name, Is.EqualTo(product.Name));
            Assert.That(productHistoryDto.PriceHistory, Is.Not.Null);
            Assert.That(productHistoryDto.PriceHistory.Count, Is.EqualTo(1));
            Assert.That(productHistoryDto.PriceHistory.First().Price, Is.EqualTo(90.0m));
        }

        [Test]
        public void ToAppliedDiscountDto_ConvertsProductAndDiscountedPriceToAppliedDiscountDto_Correctly()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 100.0m
            };
            var originalPrice = 100.0m;
            var discountedPrice = 80.0m;

            // Act
            var appliedDiscountDto = product.ToAppliedDiscountDto(originalPrice, discountedPrice);

            // Assert
            Assert.That(appliedDiscountDto, Is.Not.Null);
            Assert.That(appliedDiscountDto.Id, Is.EqualTo(product.Id));
            Assert.That(appliedDiscountDto.Name, Is.EqualTo(product.Name));
            Assert.That(appliedDiscountDto.OriginalPrice, Is.EqualTo(originalPrice));
            Assert.That(appliedDiscountDto.DiscountedPrice, Is.EqualTo(discountedPrice));
        }

        [Test]
        public void ToDto_ConvertsDiscountRequestToDiscountDto_Correctly()
        {
            // Arrange
            var discountRequest = new DiscountRequest { DiscountPercentage = 20 };
            var productId = 1;

            // Act
            var discountDto = discountRequest.ToDto(productId);

            // Assert
            Assert.That(discountDto, Is.Not.Null);
            Assert.That(discountDto.ProductId, Is.EqualTo(productId));
            Assert.That(discountDto.DiscountPercentage, Is.EqualTo(discountRequest.DiscountPercentage));
        }

        [Test]
        public void ToDto_ConvertsUpdatePriceRequestToUpdatePriceDto_Correctly()
        {
            // Arrange
            var updatePriceRequest = new UpdatePriceRequest { NewPrice = 150.0m };
            var productId = 1;

            // Act
            var updatePriceDto = updatePriceRequest.ToDto(productId);

            // Assert
            Assert.That(updatePriceDto, Is.Not.Null);
            Assert.That(updatePriceDto.ProductId, Is.EqualTo(productId));
            Assert.That(updatePriceDto.NewPrice, Is.EqualTo(updatePriceRequest.NewPrice));
        }
    }
}
