using Microsoft.AspNetCore.Components;
using MudBlazor;
using VintageCashCow.Application;
using VintageCashCow.Client.Mappers;
using VintageCashCow.Client.Models;

namespace VintageCashCow.Client.Shared;

/// <summary>
/// Component that provides action buttons for updating product prices, applying discounts, and viewing price history.
/// </summary>
/// <remarks>
/// This component displays three main actions:
/// <list type="bullet">
/// <item><description>Update Price - Allows users to set a new price for the product</description></item>
/// <item><description>Apply Discount - Allows users to apply a percentage-based discount</description></item>
/// <item><description>View History - Displays the price history in a modal dialog</description></item>
/// </list>
/// </remarks>
public partial class ProductActions
{
    /// <summary>
    /// Gets or sets the product service for managing product operations.
    /// </summary>
    /// <value>The injected <see cref="IProductService"/> instance.</value>
    [Inject] private IProductService ProductService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the snackbar service for displaying notifications.
    /// </summary>
    /// <value>The injected <see cref="ISnackbar"/> instance from MudBlazor.</value>
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    /// <summary>
    /// Gets or sets the dialog service for displaying modal dialogs.
    /// </summary>
    /// <value>The injected <see cref="IDialogService"/> instance from MudBlazor.</value>
    [Inject] private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the product to perform actions on.
    /// </summary>
    /// <value>The <see cref="ProductViewModel"/> instance representing the product.</value>
    [Parameter, EditorRequired] public ProductViewModel Product { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback invoked when the product is updated.
    /// </summary>
    /// <value>An <see cref="EventCallback{ProductViewModel}"/> that notifies parent components of updates.</value>
    [Parameter] public EventCallback<ProductViewModel> OnProductUpdated { get; set; }

    /// <summary>
    /// Updates the price of the current product.
    /// </summary>
    /// <remarks>
    /// This method sends a request to update the product price and displays a success or error message.
    /// If successful, it updates the local product model and notifies parent components.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdatePrice()
    {
        try
        {
            var updated = await ProductService.UpdateProductPriceAsync(Product.Id, Product.PriceUpdate.NewPrice);

            Product.Price = updated.Price;
            Product.LastUpdated = updated.LastUpdated;
            Product.PriceUpdate = new PriceUpdateViewModel();
            Product.DiscountUpdate = new DiscountViewModel();

            await OnProductUpdated.InvokeAsync(Product);
            Snackbar.Add($"Price updated to {updated.Price:C}", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Applies a percentage-based discount to the current product.
    /// </summary>
    /// <remarks>
    /// This method sends a request to apply the discount and displays a success or error message.
    /// If successful, it updates the local product price and notifies parent components.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ApplyDiscount()
    {
        try
        {
            var discounted = await ProductService.ApplyDiscountToProduct(Product.Id, Product.DiscountUpdate.Discount);

            Product.Price = discounted.DiscountedPrice;
            Product.LastUpdated = DateTime.UtcNow;
            Product.PriceUpdate = new PriceUpdateViewModel();
            Product.DiscountUpdate = new DiscountViewModel();

            await OnProductUpdated.InvokeAsync(Product);
            Snackbar.Add($"Discount applied. New price: {discounted.DiscountedPrice:C}", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Displays the price history for the current product in a modal dialog.
    /// </summary>
    /// <remarks>
    /// This method fetches the price history from the service and displays it in a <see cref="PriceHistoryDialog"/>.
    /// If an error occurs, an error message is displayed via the snackbar.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ShowPriceHistory()
    {
        try
        {
            var history = await ProductService.GetProductHistoryAsync(Product.Id);
            var parameters = new DialogParameters { [nameof(PriceHistoryDialog.ProductHistory)] = history.ToViewModel() };
            var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseOnEscapeKey = true };
            await DialogService.ShowAsync<PriceHistoryDialog>("Price History", parameters, options);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }
    }
}
