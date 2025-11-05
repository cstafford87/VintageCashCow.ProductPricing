using Microsoft.AspNetCore.Components;
using VintageCashCow.Application;
using VintageCashCow.WebUI.Server.Mappers;
using VintageCashCow.WebUI.Server.Models;

namespace VintageCashCow.WebUI.Server.Components.Pages
{
    /// <summary>
    /// Represents the Main page component that displays and manages a list of products.
    /// </summary>
    /// <remarks>This partial class serves as the code-behind for the Main Razor component. It handles
    /// product data retrieval, loading states, and product update notifications.</remarks>
    public partial class Main
    {
        /// <summary>
        /// Gets or sets the product service used to retrieve and manage product data.
        /// </summary>
        [Inject]
        private IProductService ProductService { get; set; } = default!;

        /// <summary>
        /// Gets or sets the list of products displayed on the menu page.
        /// </summary>
        private List<ProductViewModel> Products = new();

        /// <summary>
        /// Gets or sets a value indicating whether the component is currently loading data.
        /// </summary>
        private bool _isLoading = true;

        /// <summary>
        /// Initializes the component by loading all products from the service.
        /// </summary>
        /// <remarks>This method is called when the component is first initialized. It fetches all products
        /// from the API and converts them to view models for display.</remarks>
        /// <returns>A task representing the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var dtos = await ProductService.GetAllProductsAsync();
            Products = dtos.Select(p => p.ToViewModel()).ToList();
            _isLoading = false;
        }

        /// <summary>
        /// Handles the event when a product is updated, refreshing the product in the list.
        /// </summary>
        /// <param name="updatedProduct">The updated product view model to replace in the list.</param>
        private void HandleProductUpdated(ProductViewModel updatedProduct)
        {
            var index = Products.FindIndex(p => p.Id == updatedProduct.Id);
            if (index != -1)
            {
                Products[index] = updatedProduct;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the component is currently loading data.
        /// </summary>
        public bool IsLoading => _isLoading;
    }
}
