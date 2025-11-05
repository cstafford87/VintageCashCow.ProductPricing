using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;
using VintageCashCow.Application;
using VintageCashCow.WebUI.Server.Mappers;
using VintageCashCow.WebUI.Server.Models;

namespace VintageCashCow.WebUI.Server.Components.Shared
{
    /// <summary>
    /// Represents a Blazor component that provides actions for managing a product, including price updates,
    /// discount applications, and price history viewing.
    /// </summary>
    /// <remarks>This partial class serves as the code-behind for the ProductActions Razor component. It handles
    /// user interactions for updating product prices, applying discounts, and displaying price history through
    /// dialogs.</remarks>
    public partial class ProductActions
    {
        /// <summary>
        /// Gets or sets the product service used to perform product-related operations.
        /// </summary>
        [Inject]
        private IProductService ProductService { get; set; } = default!;

        /// <summary>
        /// Gets or sets the snackbar service used to display notifications to the user.
        /// </summary>
        [Inject]
        private ISnackbar Snackbar { get; set; } = default!;

        /// <summary>
        /// Gets or sets the dialog service used to display dialogs such as price history.
        /// </summary>
        [Inject]
        private IDialogService DialogService { get; set; } = default!;

        /// <summary>
        /// Gets or sets the logger instance for logging errors and diagnostic information.
        /// </summary>
        [Inject]
        private ILogger<ProductActions> Logger { get; set; } = default!;

        /// <summary>
        /// Gets or sets the product view model that this component manages.
        /// </summary>
        [Parameter, EditorRequired]
        public ProductViewModel Product { get; set; } = default!;

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
        /// Updates the price of the current product to the specified new price.
        /// </summary>
        /// <remarks>This method calls the product service to update the product price, displays a success
        /// notification, and notifies the parent component of the update. If an error occurs, it logs the error
        /// and displays an error notification.</remarks>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task UpdatePrice()
        {
            try
            {
                var updatedProductDto = await ProductService.UpdateProductPriceAsync(Product.Id, Product.PriceUpdate.NewPrice);
                var updatedProductViewModel = updatedProductDto.ToViewModel();

                // Create new instances to reset the form values
                updatedProductViewModel.PriceUpdate = new PriceUpdateViewModel();
                updatedProductViewModel.DiscountUpdate = new DiscountViewModel();

                Snackbar.Add($"Price for {updatedProductDto.Name} updated to {updatedProductDto.Price.ToString("C", _gbpCulture)}", Severity.Success);

                if (OnProductUpdated.HasDelegate)
                {
                    await OnProductUpdated.InvokeAsync(updatedProductViewModel);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating price for product {ProductId}", Product.Id);
                Snackbar.Add($"Error updating price: {ex.Message}", Severity.Error);
            }
        }

        /// <summary>
        /// Applies a discount to the current product based on the specified discount percentage.
        /// </summary>
        /// <remarks>This method calls the product service to apply the discount, calculates the new price,
        /// displays a success notification, and notifies the parent component of the update. If an error occurs,
        /// it logs the error and displays an error notification.</remarks>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ApplyDiscount()
        {
            try
            {
                var discountedProductDto = await ProductService.ApplyDiscountToProduct(Product.Id, Product.DiscountUpdate.Discount);

                var updatedProductViewModel = new ProductViewModel
                {
                    Id = Product.Id,
                    Name = discountedProductDto.Name,
                    Price = discountedProductDto.DiscountedPrice,
                    LastUpdated = DateTime.UtcNow,
                    PriceUpdate = new PriceUpdateViewModel(),
                    DiscountUpdate = new DiscountViewModel()
                };

                Snackbar.Add($"Discount applied to {discountedProductDto.Name}. New price is {discountedProductDto.DiscountedPrice.ToString("C", _gbpCulture)}", Severity.Success);

                // Notify parent to handle the update - parent owns the state
                if (OnProductUpdated.HasDelegate)
                {
                    await OnProductUpdated.InvokeAsync(updatedProductViewModel);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error applying discount for product {ProductId}", Product.Id);
                Snackbar.Add($"Error applying discount: {ex.Message}", Severity.Error);
            }
        }

        /// <summary>
        /// Displays the price history for the current product in a dialog.
        /// </summary>
        /// <remarks>This method fetches the product's price history from the service and displays it in a
        /// modal dialog. If an error occurs during retrieval, it logs the error and displays an error
        /// notification.</remarks>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ShowPriceHistory()
        {
            try
            {
                var historyDto = await ProductService.GetProductHistoryAsync(Product.Id);
                var historyViewModel = historyDto.ToViewModel();

                var parameters = new DialogParameters { [nameof(PriceHistoryDialog.ProductHistory)] = historyViewModel };
                var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true };
                await DialogService.ShowAsync<PriceHistoryDialog>("Price History", parameters, options);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error fetching price history for product {ProductId}", Product.Id);
                Snackbar.Add($"Error fetching price history: {ex.Message}", Severity.Error);
            }
        }
    }
}
