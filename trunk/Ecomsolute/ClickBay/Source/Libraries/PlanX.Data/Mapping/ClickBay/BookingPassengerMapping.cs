using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
    public class BookingPassengerMapping : EntityTypeConfiguration<BookingPassenger>
    {
        public BookingPassengerMapping()
        {
            this.ToTable("BookingPassenger");
            this.HasKey(x => x.Id);
            this.HasRequired(x => x.Booking)
                .WithMany(b=>b.BookingPassengers)
                .HasForeignKey(b => b.BookingId);

            this.Ignore(x => x.PasserType);
                
        }
    }
}
