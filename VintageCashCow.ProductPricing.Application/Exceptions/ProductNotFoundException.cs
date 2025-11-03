using System;

namespace VintageCashCow.ProductPricing.Application.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a specified product cannot be found.
    /// </summary>
    /// <remarks>This exception is typically used in scenarios where a product lookup operation fails  due to
    /// the product being unavailable or nonexistent in the data source.</remarks>
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base(message)
        {
        }
    }
}
