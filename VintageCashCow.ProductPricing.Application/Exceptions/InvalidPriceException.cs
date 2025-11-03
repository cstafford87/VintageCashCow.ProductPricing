using System;

namespace VintageCashCow.ProductPricing.Application.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an invalid price is encountered.
    /// </summary>
    /// <remarks>This exception is typically used to indicate that a price value provided to an operation is
    /// invalid, such as being negative or exceeding a predefined limit.</remarks>
    public class InvalidPriceException : Exception
    {
        public InvalidPriceException(string message) : base(message)
        {
        }
    }
}
