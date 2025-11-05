using MudBlazor;

namespace VintageCashCow.Client.Shared
{
    /// <summary>
    /// Provides the custom MudBlazor theme configuration for the Vintage Cash Cow application.
    /// </summary>
    /// <remarks>
    /// This class defines the color palette and visual style for the application,
    /// using a purple and gold color scheme.
    /// </remarks>
    public static class VintageCashCowTheme
    {
        /// <summary>
        /// Gets the custom MudBlazor theme instance.
        /// </summary>
        /// <value>A <see cref="MudTheme"/> instance configured with the application's color palette.</value>
        /// <remarks>
        /// The theme uses the following color scheme:
        /// <list type="bullet">
        /// <item><description>Primary: Deep purple (#38157a)</description></item>
        /// <item><description>Secondary: Purple (#543099)</description></item>
        /// <item><description>Background: Light gray (#F5F5F5)</description></item>
        /// <item><description>AppBar: Purple (#543099) with white text</description></item>
        /// </list>
        /// </remarks>
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
