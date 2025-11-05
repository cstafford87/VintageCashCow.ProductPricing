using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using VintageCashCow.Application;
using VintageCashCow.Application.Dtos;

namespace VintageCashCow.Tests
{
    internal sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _responder;

        public FakeHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responder)
        {
            _responder = responder ?? throw new ArgumentNullException(nameof(responder));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => _responder(request, cancellationToken);
    }

    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<ILogger<ProductService>> _loggerMock = null!;

        [SetUp]
        public void SetUp() => _loggerMock = new Mock<ILogger<ProductService>>();

        #region Helpers

        private static Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> JsonResponder(object? payload, HttpStatusCode status = HttpStatusCode.OK)
        {
            return (req, ct) =>
            {
                if (payload is null)
                {
                    return Task.FromResult(new HttpResponseMessage(status)
                    {
                        Content = new StringContent("null", Encoding.UTF8, "application/json")
                    });
                }

                var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                return Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent(json, Encoding.UTF8, "application/json") });
            };
        }

        private ProductService CreateService(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responder)
        {
            var client = new HttpClient(new FakeHttpMessageHandler(responder)) { BaseAddress = new Uri("http://localhost/") };
            return new ProductService(client, _loggerMock.Object);
        }

        private void VerifyLoggerErrorCalledOnce()
        {
            var count = _loggerMock.Invocations.Count(i => i.Method.Name == "Log" && i.Arguments.Count > 0 && i.Arguments[0] is LogLevel lvl && lvl == LogLevel.Error);
            Assert.AreEqual(1, count, "Expected a single error-level Log invocation");
        }

        #endregion

        #region GetAllProductsAsync

        [Test]
        public async Task GetAllProductsAsync_WhenApiReturnsProducts_ReturnsList()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new() { Id = 1, Name = "A", Price = 10m, LastUpdated = DateTime.UtcNow },
                new() { Id = 2, Name = "B", Price = 20m, LastUpdated = DateTime.UtcNow }
            };

            var svc = CreateService(JsonResponder(products));

            // Act
            var result = await svc.GetAllProductsAsync();

            // Assert
            Assert.That(result, Is.Not.Null.And.Count.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("A"));
        }

        [Test]
        public async Task GetAllProductsAsync_WhenApiReturnsEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var svc = CreateService(JsonResponder(new List<ProductDto>()));

            // Act
            var result = await svc.GetAllProductsAsync();

            // Assert
            Assert.That(result, Is.Not.Null.And.Empty);
        }

        [Test]
        public void GetAllProductsAsync_WhenResponseContentIsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var svc = CreateService(JsonResponder(null));

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => svc.GetAllProductsAsync());
        }

        [Test]
        public void GetAllProductsAsync_WhenHttpRequestThrows_LogsAndPropagates()
        {
            // Arrange
            var svc = CreateService((req, ct) => throw new HttpRequestException("network"));

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(() => svc.GetAllProductsAsync());
            VerifyLoggerErrorCalledOnce();
        }

        [Test]
        public void GetAllProductsAsync_WhenApiReturnsNonSuccessStatus_ThrowsHttpRequestException()
        {
            // Arrange
            var svc = CreateService(JsonResponder(new List<ProductDto>(), HttpStatusCode.InternalServerError));

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(() => svc.GetAllProductsAsync());
        }

        #endregion

        #region UpdateProductPriceAsync

        [Test]
        public async Task UpdateProductPriceAsync_WhenApiReturnsProduct_ReturnsUpdatedProduct()
        {
            // Arrange
            var expected = new ProductDto { Id = 5, Name = "Thing", Price = 12.34m, LastUpdated = DateTime.UtcNow };
            var svc = CreateService(JsonResponder(expected));

            // Act
            var result = await svc.UpdateProductPriceAsync(expected.Id, 15m);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(expected.Id));
        }

        [Test]
        public async Task UpdateProductPriceAsync_SendsCorrectRequest()
        {
            // Arrange
            HttpRequestMessage? capturedRequest = null;
            var expected = new ProductDto { Id = 5, Name = "Thing", Price = 15m, LastUpdated = DateTime.UtcNow };

            var svc = CreateService((req, ct) =>
            {
                capturedRequest = req;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(expected, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), Encoding.UTF8, "application/json")
                });
            });

            // Act
            await svc.UpdateProductPriceAsync(5, 15m);

            // Assert
            Assert.That(capturedRequest, Is.Not.Null);
            Assert.That(capturedRequest!.Method, Is.EqualTo(HttpMethod.Put));
            Assert.That(capturedRequest.RequestUri!.ToString(), Does.Contain("/5/update-price"));
        }

        [Test]
        public void UpdateProductPriceAsync_WhenResponseContentIsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var svc = CreateService(JsonResponder(null));

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateProductPriceAsync(1, 1));
        }

        [Test]
        public void UpdateProductPriceAsync_WhenUnderlyingThrows_LogsAndPropagates()
        {
            // Arrange
            var svc = CreateService((req, ct) => throw new InvalidOperationException("boom"));

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateProductPriceAsync(1, 1));
            VerifyLoggerErrorCalledOnce();
        }

        [Test]
        public async Task UpdateProductPriceAsync_HandlesZeroPrice()
        {
            // Arrange
            var expected = new ProductDto { Id = 1, Name = "Test", Price = 0m, LastUpdated = DateTime.UtcNow };
            var svc = CreateService(JsonResponder(expected));

            // Act
            var result = await svc.UpdateProductPriceAsync(1, 0m);

            // Assert
            Assert.That(result.Price, Is.EqualTo(0m));
        }

        #endregion

        #region ApplyDiscountToProduct

        [Test]
        public async Task ApplyDiscountToProduct_WhenApiReturnsAppliedDiscount_ReturnsDto()
        {
            // Arrange
            var dto = new AppliedDiscountDto { Id = 2, Name = "X", OriginalPrice = 10m, DiscountedPrice = 8m };
            var svc = CreateService(JsonResponder(dto));

            // Act
            var result = await svc.ApplyDiscountToProduct(dto.Id, 20);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.DiscountedPrice, Is.EqualTo(dto.DiscountedPrice));
        }

        [Test]
        public async Task ApplyDiscountToProduct_SendsCorrectRequest()
        {
            // Arrange
            HttpRequestMessage? capturedRequest = null;
            var dto = new AppliedDiscountDto { Id = 2, Name = "X", OriginalPrice = 10m, DiscountedPrice = 8m };

            var svc = CreateService((req, ct) =>
            {
                capturedRequest = req;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(dto, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), Encoding.UTF8, "application/json")
                });
            });

            // Act
            await svc.ApplyDiscountToProduct(2, 20);

            // Assert
            Assert.That(capturedRequest, Is.Not.Null);
            Assert.That(capturedRequest!.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(capturedRequest.RequestUri!.ToString(), Does.Contain("/2/apply-discount"));
        }

        [Test]
        public void ApplyDiscountToProduct_WhenResponseContentIsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var svc = CreateService(JsonResponder(null));

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => svc.ApplyDiscountToProduct(2, 20));
        }

        [Test]
        public void ApplyDiscountToProduct_WhenUnderlyingThrows_LogsAndPropagates()
        {
            // Arrange
            var svc = CreateService((req, ct) => throw new Exception("err"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => svc.ApplyDiscountToProduct(2, 20));
            VerifyLoggerErrorCalledOnce();
        }

        [Test]
        public async Task ApplyDiscountToProduct_HandlesZeroDiscount()
        {
            // Arrange
            var dto = new AppliedDiscountDto { Id = 2, Name = "X", OriginalPrice = 10m, DiscountedPrice = 10m };
            var svc = CreateService(JsonResponder(dto));

            // Act
            var result = await svc.ApplyDiscountToProduct(2, 0);

            // Assert
            Assert.That(result.DiscountedPrice, Is.EqualTo(result.OriginalPrice));
        }

        [Test]
        public async Task ApplyDiscountToProduct_Handles100PercentDiscount()
        {
            // Arrange
            var dto = new AppliedDiscountDto { Id = 2, Name = "X", OriginalPrice = 10m, DiscountedPrice = 0m };
            var svc = CreateService(JsonResponder(dto));

            // Act
            var result = await svc.ApplyDiscountToProduct(2, 100);

            // Assert
            Assert.That(result.DiscountedPrice, Is.EqualTo(0m));
        }

        #endregion

        #region GetProductHistoryAsync

        [Test]
        public async Task GetProductHistoryAsync_WhenApiReturnsHistory_ReturnsDto()
        {
            // Arrange
            var dto = new ProductHistoryDto
            {
                Id = 9,
                Name = "History",
                PriceHistory = new List<PriceHistoryDto> { new() { Price = 1.1m, Date = DateTime.UtcNow } }
            };

            var svc = CreateService(JsonResponder(dto));

            // Act
            var result = await svc.GetProductHistoryAsync(dto.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(dto.Id));
            Assert.That(result.PriceHistory.Count, Is.EqualTo(dto.PriceHistory.Count));
        }

        [Test]
        public async Task GetProductHistoryAsync_HandlesEmptyPriceHistory()
        {
            // Arrange
            var dto = new ProductHistoryDto
            {
                Id = 9,
                Name = "History",
                PriceHistory = new List<PriceHistoryDto>()
            };

            var svc = CreateService(JsonResponder(dto));

            // Act
            var result = await svc.GetProductHistoryAsync(dto.Id);

            // Assert
            Assert.That(result.PriceHistory, Is.Empty);
        }

        [Test]
        public async Task GetProductHistoryAsync_SendsCorrectRequest()
        {
            // Arrange
            HttpRequestMessage? capturedRequest = null;
            var dto = new ProductHistoryDto { Id = 9, Name = "History", PriceHistory = new List<PriceHistoryDto>() };

            var svc = CreateService((req, ct) =>
            {
                capturedRequest = req;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(dto, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), Encoding.UTF8, "application/json")
                });
            });

            // Act
            await svc.GetProductHistoryAsync(9);

            // Assert
            Assert.That(capturedRequest, Is.Not.Null);
            Assert.That(capturedRequest!.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(capturedRequest.RequestUri!.ToString(), Does.Contain("/9"));
        }

        [Test]
        public void GetProductHistoryAsync_WhenResponseContentIsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var svc = CreateService(JsonResponder(null));

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => svc.GetProductHistoryAsync(1));
        }

        [Test]
        public void GetProductHistoryAsync_WhenUnderlyingThrows_LogsAndPropagates()
        {
            // Arrange
            var svc = CreateService((req, ct) => throw new Exception("err"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => svc.GetProductHistoryAsync(1));
            VerifyLoggerErrorCalledOnce();
        }

        #endregion
    }
}
