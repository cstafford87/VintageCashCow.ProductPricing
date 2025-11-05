using Microsoft.AspNetCore.Components;
using VintageCashCow.Application;
using VintageCashCow.Client.Mappers;
using VintageCashCow.Client.Models;

namespace VintageCashCow.Client.Pages;

/// <summary>
/// Main menu page component that displays and manages the product list.
/// </summary>
/// <remarks>
/// This component serves as the landing page for the application, showing all products
/// and allowing users to update prices, apply discounts, and view price history.
/// </remarks>
public partial class Menu
{
    /// <summary>
    /// Gets or sets the product service for managing product-related operations.
    /// </summary>
    /// <value>The injected <see cref="IProductService"/> instance.</value>
    [Inject] private IProductService ProductService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the list of products currently displayed on the page.
    /// </summary>
    private List<ProductViewModel> Products = [];

    /// <summary>
    /// Gets or sets a value indicating whether the component is currently loading data.
    /// </summary>
    private bool _isLoading = true;

    /// <summary>
    /// Initializes the component by loading all products from the service.
    /// </summary>
    /// <remarks>
    /// This method is called when the component is first initialized. It fetches all products
    /// from the API and converts them to view models for display.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var dtos = await ProductService.GetAllProductsAsync();
        Products = dtos.Select(p => p.ToViewModel()).ToList();
        _isLoading = false;
    }

    /// <summary>
    /// Handles product update events from child components.
    /// </summary>
    /// <remarks>
    /// This method is called when a child component updates a product. It finds the product
    /// in the list by ID and updates it with the new values, then triggers a UI refresh.
    /// </remarks>
    /// <param name="updated">The updated product view model containing the new values.</param>
    public void HandleProductUpdated(ProductViewModel updated)
    {
        var index = Products.FindIndex(p => p.Id == updated.Id);
        if (index >= 0) Products[index] = updated;
        StateHasChanged();
    }

    /// <summary>
    /// Gets a value indicating whether the component is currently loading data.
    /// </summary>
    /// <value><c>true</c> if loading; otherwise, <c>false</c>.</value>
    public bool IsLoading => _isLoading;
}
