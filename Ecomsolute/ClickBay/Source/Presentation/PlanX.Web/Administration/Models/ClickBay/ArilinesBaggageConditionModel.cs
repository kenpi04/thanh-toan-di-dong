using PlanX.Web.Framework;
using PlanX.Web.Framework.Mvc;

namespace PlanX.Admin.Models.ClickBay
{
    public class ArilinesBaggageConditionModel : BaseNopEntityModel
    {
        /// <summary>
        /// Id Airlines
        /// </summary>
        [NopResourceDisplayName("Admin.ClickBay.ArilinesBaggageCondition.Field.AirlinesId")]
        public int AirlinesId { get; set; }
        /// <summary>
        /// So kg hanh ly
        /// </summary>
        [NopResourceDisplayName("Admin.ClickBay.ArilinesBaggageCondition.Field.Baggage")]
        public int Baggage { get; set; }
        /// <summary>
        /// Phi cong them
        /// </summary>
        [NopResourceDisplayName("Admin.ClickBay.ArilinesBaggageCondition.Field.BaggageFee")]
        public decimal BaggageFee { get; set; }
        [NopResourceDisplayName("Admin.ClickBay.ArilinesBaggageCondition.Field.DisplayOrder")]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// La hanh ly xach tay
        /// </summary>
        [NopResourceDisplayName("Admin.ClickBay.ArilinesBaggageCondition.Field.IsHandLuggage")]
        public bool IsHandLuggage { get; set; }
        [NopResourceDisplayName("Admin.ClickBay.ArilinesBaggageCondition.Field.Deleted")]
        public bool Deleted { get; set; }
        /// <summary>
        /// La mien phi
        /// </summary>
        [NopResourceDisplayName("Admin.ClickBay.ArilinesBaggageCondition.Field.IsFree")]
        public bool IsFree { get; set; }
        /// <summary>
        /// Mo ta
        /// </summary>
        [NopResourceDisplayName("Admin.ClickBay.ArilinesBaggageCondition.Field.Description")]
        public string Description { get; set; }
    }
}