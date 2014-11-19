using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.Logging
{
    public partial class ActivityLogTypeModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLogType.Fields.Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLogType.Fields.Enabled")]
        public bool Enabled { get; set; }
    }
}