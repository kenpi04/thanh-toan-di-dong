using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
    public class BookingBaggageMapping:EntityTypeConfiguration<BookingBaggage>
    {
        public BookingBaggageMapping()
        {
            this.ToTable("BookingBaggage");
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.BookingInfoFlight)
                .WithMany(p => p.BookingBaggages)
                .HasForeignKey(b => b.BookingInfoFlightId);

            this.Ignore(x=>x.PassengerTypeEnum);
        }
    }
}
