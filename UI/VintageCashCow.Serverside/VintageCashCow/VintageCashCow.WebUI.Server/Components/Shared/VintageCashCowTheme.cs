using MudBlazor;

namespace VintageCashCow.WebUI.Server.Components.Shared
{
    /// <summary>
    /// Provides the custom theme configuration for the Vintage Cash Cow application.
    /// </summary>
    /// <remarks>
    /// This static class defines the visual styling and color palette used throughout the application,
    /// including primary and secondary colors, backgrounds, and semantic colors for success, warning, and error
    /// states. The theme follows the Vintage Cash Cow brand guidelines with a deep purple primary color and
    /// gold secondary color.
    /// </remarks>
    public static class VintageCashCowTheme
    {
        /// <summary>
        /// Gets the MudBlazor theme instance configured with the Vintage Cash Cow brand colors and styling.
        /// </summary>
        /// <value>
        /// A <see cref="MudTheme"/> object containing the light palette with custom colors for various UI elements
        /// including primary (#38157a - deep purple), secondary (#543099 - gold), and semantic colors for different
        /// states and components.
        /// </value>
        public static MudTheme Theme => new MudTheme()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#38157a", // Deep purple
                Secondary = "#543099", // Gold
                Background = "#F5F5F5", // Light gray
                Surface = "#FFFFFF", // White
                AppbarBackground = "#543099",
                AppbarText = "#FFFFFF",
                DrawerBackground = "#543099",
                DrawerText = "#FFFFFF",
                DrawerIcon = "#FFFFFF",
                Success = "#388E3C",
                Warning = "#FFA726",
                Error = "#C62828",
                Info = "#6C3483",
                TextPrimary = "#000000",
                TextSecondary = "#000000"
            }
        };
    }
}
