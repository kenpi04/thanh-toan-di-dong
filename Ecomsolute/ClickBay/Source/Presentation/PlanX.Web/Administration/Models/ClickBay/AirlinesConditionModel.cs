using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.ClickBay
{
    public class AirlinesConditionModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.ClickBay.AirlinesCondition.Field.AirlinesId")]
        public int AirlinesId { get; set; }
        [NopResourceDisplayName("Admin.ClickBay.AirlinesCondition.Field.ConditionName")]
        public string ConditionName { get; set; }
        [NopResourceDisplayName("Admin.ClickBay.AirlinesCondition.Field.ConditionDescription")]
        public string ConditionDescription { get; set; }
        [NopResourceDisplayName("Admin.ClickBay.AirlinesCondition.Field.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [NopResourceDisplayName("Admin.ClickBay.AirlinesCondition.Field.TicketType")]
        public string TicketType { get; set; }
        [NopResourceDisplayName("Admin.ClickBay.AirlinesCondition.Field.Deleted")]
        public bool Deleted { get; set; }
    }
}