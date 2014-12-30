using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
    public class BookingInfoFlightDetailMapping : EntityTypeConfiguration<BookingInfoFlightDetail>
    {
        public BookingInfoFlightDetailMapping()
        {
            this.ToTable("BookingInfoFlightDetail");
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.BookingInfoFlight)
                .WithMany(x => x.BookingInfoFlightDetails)
                .HasForeignKey(x => x.BookingInfoFlightId);
        }
    }
}
