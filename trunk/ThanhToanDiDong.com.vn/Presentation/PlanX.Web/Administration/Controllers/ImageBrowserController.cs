using System.IO;
using PlanX.Web.Framework.Controllers;
using PlanX.Web.Framework.UI.Editor;
using Telerik.Web.Mvc.UI;

//Original code can be found here http://demos.telerik.com/aspnet-mvc/razor/editor/imagetool
namespace PlanX.Admin.Controllers
{
    [AdminAuthorize]
    public partial class ImageBrowserController : EditorFileBrowserController
    {
        /// <summary>
        /// Gets the base paths from which content will be served.
        /// </summary>
        public override string[] ContentPaths
        {
            get
            {
                var path = Server.MapPath(NetAdvImageSettings.UploadPath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return new[] { NetAdvImageSettings.UploadPath };
            }
        }
    }
}
