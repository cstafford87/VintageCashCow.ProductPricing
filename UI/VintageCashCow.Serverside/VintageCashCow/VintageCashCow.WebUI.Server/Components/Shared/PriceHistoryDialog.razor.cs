using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;
using System.Linq;
using VintageCashCow.WebUI.Server.Models;

namespace VintageCashCow.WebUI.Server.Components.Shared
{
    /// <summary>
    /// Represents a dialog component that displays the price history of a product.
    /// </summary>
    /// <remarks>This partial class serves as the code-behind for the PriceHistoryDialog Razor component.
    /// It displays a product's historical prices in a table format, sorted by date in descending order.</remarks>
    public partial class PriceHistoryDialog
    {
        /// <summary>
        /// Gets or sets the MudBlazor dialog instance for controlling the dialog lifecycle.
        /// </summary>
        [CascadingParameter]
        public IMudDialogInstance? MudDialog { get; set; }

        /// <summary>
        /// Gets or sets the product history view model containing the product details and price history.
        /// </summary>
        [Parameter]
        public ProductHistoryViewModel? ProductHistory { get; set; }

        /// <summary>
        /// The culture info used for formatting currency values in British Pounds (GBP).
        /// </summary>
        private readonly CultureInfo _gbpCulture = new("en-GB");

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        public void Close() => MudDialog?.Close();

        /// <summary>
        /// Called when component parameters are set, ensuring the price history is sorted by date in descending order.
        /// </summary>
        /// <remarks>This method overrides the base implementation to sort the price history records before
        /// displaying them. The most recent prices appear first in the list.</remarks>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (ProductHistory?.PriceHistory is not null)
            {
                ProductHistory.PriceHistory = ProductHistory.PriceHistory.OrderByDescending(ph => ph.Date).ToList();

                StateHasChanged();
            }
        }
    }
}
