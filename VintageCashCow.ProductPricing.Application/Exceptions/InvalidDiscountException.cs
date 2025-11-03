using System;

namespace VintageCashCow.ProductPricing.Application.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an invalid discount is encountered.
    /// </summary>
    /// <remarks>This exception is typically thrown when a discount value is invalid, such as being out of the
    /// acceptable range or violating business rules. Use this exception to indicate issues with discount validation in
    /// your application.</remarks>
    public class InvalidDiscountException : Exception
    {
        public InvalidDiscountException(string message) : base(message)
        {
        }
    }
}
