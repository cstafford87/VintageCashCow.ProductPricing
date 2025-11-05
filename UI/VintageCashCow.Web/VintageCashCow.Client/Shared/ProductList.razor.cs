using Microsoft.AspNetCore.Components;
using System.Globalization;
using VintageCashCow.Client.Models;

namespace VintageCashCow.Client.Shared;

/// <summary>
/// Component that displays a list of products in a table format.
/// </summary>
/// <remarks>
/// This component renders products using MudBlazor's MudTable component and allows
/// users to interact with individual products through the ProductActions component.
/// </remarks>
public partial class ProductList
{
    /// <summary>
    /// Gets or sets the list of products to display.
    /// </summary>
    /// <value>A list of <see cref="ProductViewModel"/> instances representing the products.</value>
    [Parameter] public List<ProductViewModel> Products { get; set; } = [];

    /// <summary>
    /// Gets or sets the callback invoked when a product is updated.
    /// </summary>
    /// <value>An <see cref="EventCallback{ProductViewModel}"/> that notifies parent components of updates.</value>
    [Parameter] public EventCallback<ProductViewModel> OnProductUpdated { get; set; }

    /// <summary>
    /// Culture information for formatting currency values in GBP.
    /// </summary>
    private readonly CultureInfo _gbpCulture = new("en-GB");

    /// <summary>
    /// Handles product update events from child ProductActions components.
    /// </summary>
    /// <remarks>
    /// This method updates the product in the local list and propagates the update
    /// to parent components through the <see cref="OnProductUpdated"/> callback.
    /// </remarks>
    /// <param name="updated">The updated product view model.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleProductUpdated(ProductViewModel updated)
    {
        var index = Products.FindIndex(p => p.Id == updated.Id);
        if (index >= 0) Products[index] = updated;

        if (OnProductUpdated.HasDelegate)
            await OnProductUpdated.InvokeAsync(updated);

        StateHasChanged();
    }
}
