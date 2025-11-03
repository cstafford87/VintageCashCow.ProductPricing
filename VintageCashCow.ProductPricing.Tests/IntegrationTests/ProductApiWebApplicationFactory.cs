using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VintageCashCow.ProductPricing.Application.Interfaces;

namespace VintageCashCow.ProductPricing.Tests.IntegrationTests
{
    public class ProductApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly Lazy<Mock<IProductService>> _productServiceMockLazy = new(() => new Mock<IProductService>());
        public Mock<IProductService> ProductServiceMock => _productServiceMockLazy.Value;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped(_ => ProductServiceMock.Object);
            });
        }

        public void Reset()
        {
            if (_productServiceMockLazy.IsValueCreated)
            {
                _productServiceMockLazy.Value.Reset();
            }
        }
    }
}
