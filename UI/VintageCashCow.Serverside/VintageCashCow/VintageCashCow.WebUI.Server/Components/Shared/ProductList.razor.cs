using Microsoft.AspNetCore.Components;
using System.Globalization;
using VintageCashCow.WebUI.Server.Models;

namespace VintageCashCow.WebUI.Server.Components.Shared
{
    /// <summary>
    /// Represents a Blazor component that displays a list of products in a table format.
    /// </summary>
    /// <remarks>This partial class serves as the code-behind for the ProductList Razor component. It manages
    /// the display of products and handles product update notifications from child components.</remarks>
    public partial class ProductList
    {
        /// <summary>
        /// Gets or sets the list of products to display in the component.
        /// </summary>
        [Parameter]
        public List<ProductViewModel> Products { get; set; } = new();

        /// <summary>
        /// Gets or sets the event callback invoked when a product is updated.
        /// </summary>
        [Parameter]
        public EventCallback<ProductViewModel> OnProductUpdated { get; set; }

        /// <summary>
        /// The culture info used for formatting currency values in British Pounds (GBP).
        /// </summary>
        private readonly CultureInfo _gbpCulture = new("en-GB");

        /// <summary>
        /// Handles the product updated event and propagates it to the parent component.
        /// </summary>
        /// <param name="updatedProduct">The updated product view model.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleProductUpdated(ProductViewModel updatedProduct)
        {
            if (OnProductUpdated.HasDelegate)
            {
                await OnProductUpdated.InvokeAsync(updatedProduct);
            }
                
            StateHasChanged();
        }
    }
}
