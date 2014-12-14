using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;
namespace PlanX.Data.Mapping.ClickBay
{
    public class BookingMapping : EntityTypeConfiguration<Booking>
    {
        public BookingMapping()
        {
            this.ToTable("Booking");
            this.HasKey(x => x.Id);
            this.HasRequired(x => x.BookingInfoFlight)
                .WithMany()
                .HasForeignKey(x => x.BookingInfoFlightToId).WillCascadeOnDelete(false);
            this.HasOptional(x => x.BookingInfoFlightReturn)
               .WithMany()
                .HasForeignKey(x => x.BookingInfoFlightReturnId).WillCascadeOnDelete(false);

            this.Ignore(x => x.PasserType);
            this.Ignore(x => x.BookingStatus);
            this.Ignore(x => x.PaymentStatus);
            this.Ignore(x => x.ContactStatus);
            //this.Ignore(x => x.BookingInfoFlight);
            //this.Ignore(x => x.BookingInfoFlightReturn);

        }
    }
}
