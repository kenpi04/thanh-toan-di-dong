using PlanX.Core.Plugins;
using Telerik.Web.Mvc.UI;

namespace PlanX.Web.Framework.Web
{
    public interface IAdminMenuPlugin : IPlugin
    {
        /// <summary>
        /// Authenticate a user (can he see this plugin menu item?)
        /// </summary>
        /// <returns></returns>
        bool Authenticate();

        /// <summary>
        /// Build menu item
        /// </summary>
        /// <param name="menuItemBuilder">Menu item builder</param>
        void BuildMenuItem(MenuItemBuilder menuItemBuilder);
    }
}
