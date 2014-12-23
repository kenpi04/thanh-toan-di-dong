using PlanX.Core.Configuration;

namespace PlanX.Core.Domain.ClickBay
{
    public class ClickBaySettings : ISettings
    {
        /// <summary>
        /// Phi cong them cho moi hanh khach
        /// </summary>
        public decimal PricePlusPerPassenger { get; set; }
        /// <summary>
        /// Giam gia cho moi hanh khach
        /// </summary>
        public decimal DiscountPerPassenger { get; set; }
    }
}
