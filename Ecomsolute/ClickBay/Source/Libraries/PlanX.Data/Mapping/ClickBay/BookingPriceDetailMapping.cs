using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
    public class BookingPriceDetailMapping : EntityTypeConfiguration<BookingPriceDetail>
    {
        public BookingPriceDetailMapping()
        {
            this.ToTable("BookingPriceDetail");
            this.HasKey(x => x.Id);
            this.HasRequired(x => x.BookingInfoFlight)
                .WithMany(x => x.BookingPriceDetails)
                .HasForeignKey(x => x.BookingInfoFlightId);
            
        }
    }
}
