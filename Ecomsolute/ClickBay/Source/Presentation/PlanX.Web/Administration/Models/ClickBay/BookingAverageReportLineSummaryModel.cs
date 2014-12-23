using PlanX.Web.Framework;

namespace PlanX.Admin.Models.ClickBay
{
    public partial class BookingAverageReportLineSummaryModel
    {
        [NopResourceDisplayName("Admin.SalesReport.Average.BookingStatus")]
        public string BookingStatus { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Average.SumTodayBookings")]
        public string SumTodayBookings { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Average.SumThisWeekBookings")]
        public string SumThisWeekBookings { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Average.SumThisMonthBookings")]
        public string SumThisMonthBookings { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Average.SumThisYearBookings")]
        public string SumThisYearBookings { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Average.SumAllTimeBookings")]
        public string SumAllTimeBookings { get; set; }
    }
}