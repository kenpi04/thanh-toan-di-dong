
namespace PlanX.Core.Domain.ClickBay
{
    public class BookingAverageReportLineSummary
    {
        /// <summary>
        /// Gets or sets the order status
        /// </summary>
        public BookingStatus BookingStatus { get; set; }

        /// <summary>
        /// Gets or sets the sum today total
        /// </summary>
        public decimal SumTodayBookings { get; set; }

        /// <summary>
        /// Gets or sets the today count
        /// </summary>
        public int CountTodayBookings { get; set; }

        /// <summary>
        /// Gets or sets the sum this week total
        /// </summary>
        public decimal SumThisWeekBookings { get; set; }

        /// <summary>
        /// Gets or sets the this week count
        /// </summary>
        public int CountThisWeekBookings { get; set; }

        /// <summary>
        /// Gets or sets the sum this month total
        /// </summary>
        public decimal SumThisMonthBookings { get; set; }

        /// <summary>
        /// Gets or sets the this month count
        /// </summary>
        public int CountThisMonthBookings { get; set; }

        /// <summary>
        /// Gets or sets the sum this year total
        /// </summary>
        public decimal SumThisYearBookings { get; set; }

        /// <summary>
        /// Gets or sets the this year count
        /// </summary>
        public int CountThisYearBookings { get; set; }

        /// <summary>
        /// Gets or sets the sum all time total
        /// </summary>
        public decimal SumAllTimeBookings { get; set; }

        /// <summary>
        /// Gets or sets the all time count
        /// </summary>
        public int CountAllTimeBookings { get; set; }
    }
}
