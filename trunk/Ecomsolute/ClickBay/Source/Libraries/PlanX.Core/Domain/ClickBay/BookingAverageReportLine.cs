
namespace PlanX.Core.Domain.ClickBay
{
    public partial class BookingAverageReportLine
    {
        /// <summary>
        /// Gets or sets the count
        /// </summary>
        public int CountBookings { get; set; }
        
        /// <summary>
        /// Gets or sets the order total summary
        /// </summary>
        public decimal SumBookings { get; set; }
    }
}
