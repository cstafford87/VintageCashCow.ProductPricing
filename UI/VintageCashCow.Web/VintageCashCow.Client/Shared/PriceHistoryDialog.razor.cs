using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;
using VintageCashCow.Client.Models;

namespace VintageCashCow.Client.Shared;

/// <summary>
/// Modal dialog component that displays the price history for a product.
/// </summary>
/// <remarks>
/// This component shows a table of historical prices sorted in descending order by date.
/// The price history is sorted only once when the component is first initialized.
/// </remarks>
public partial class PriceHistoryDialog
{
    /// <summary>
    /// Gets or sets the MudBlazor dialog instance for managing the dialog.
    /// </summary>
    /// <value>The cascaded <see cref="IMudDialogInstance"/> from MudBlazor, or <c>null</c> if not in a dialog context.</value>
    [CascadingParameter] public IMudDialogInstance? MudDialog { get; set; }

    /// <summary>
    /// Gets or sets the product history data to display.
    /// </summary>
    /// <value>The <see cref="ProductHistoryViewModel"/> containing the product's price history, or <c>null</c> if not set.</value>
    [Parameter] public ProductHistoryViewModel? ProductHistory { get; set; }

    /// <summary>
    /// Culture information for formatting currency values in GBP.
    /// </summary>
    private readonly CultureInfo _gbpCulture = new("en-GB");

    /// <summary>
    /// Flag indicating whether the component has been initialized and the price history sorted.
    /// </summary>
    private bool _isInitialized;

    /// <summary>
    /// Called when component parameters are set or changed.
    /// </summary>
    /// <remarks>
    /// This method sorts the price history in descending order by date the first time
    /// it's called. Subsequent calls are ignored to prevent re-sorting.
    /// </remarks>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (ProductHistory?.PriceHistory is not null && !_isInitialized)
        {
            ProductHistory.PriceHistory = SortPriceHistoryDescending(ProductHistory.PriceHistory);
            _isInitialized = true;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    /// <remarks>
    /// This method is called when the user clicks the Close button in the dialog.
    /// </remarks>
    public void Close() => MudDialog?.Close();

    /// <summary>
    /// Sorts the price history list in descending order by date.
    /// </summary>
    /// <param name="history">The list of price history entries to sort.</param>
    /// <returns>A new list containing the price history entries sorted by date in descending order.</returns>
    private List<PriceHistoryViewModel> SortPriceHistoryDescending(List<PriceHistoryViewModel> history) =>
        [.. history.OrderByDescending(ph => ph.Date)];
}
