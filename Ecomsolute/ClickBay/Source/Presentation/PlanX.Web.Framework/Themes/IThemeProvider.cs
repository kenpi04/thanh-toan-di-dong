using System.Collections.Generic;

namespace PlanX.Web.Framework.Themes
{
    public partial interface IThemeProvider
    {
        ThemeConfiguration GetThemeConfiguration(string themeName);

        IList<ThemeConfiguration> GetThemeConfigurations();

        bool ThemeConfigurationExists(string themeName);
    }
}
