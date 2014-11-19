using System.Web.Mvc;
using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.Common
{
    public partial class UrlRecordListModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.System.SeNames.Name")]
        [AllowHtml]
        public string SeName { get; set; }
    }
}