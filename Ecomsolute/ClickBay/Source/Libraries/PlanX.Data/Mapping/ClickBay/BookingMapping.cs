using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;
namespace PlanX.Data.Mapping.ClickBay
{
  public  class BookingMapping:EntityTypeConfiguration<Booking>
    {
      public BookingMapping()
      {
          this.ToTable("Booking");
          this.HasKey(x => x.Id);
          this.HasRequired(x => x.BookingInfoFlight)
              .WithMany()
              .HasForeignKey(x => x.BookingInfoFlightToId);
          this.HasOptional(x => x.BookingInfoFlightReturn)
              .WithMany()
              .HasForeignKey(x => x.BookingInfoFlightReturnId);
          this.Ignore(x => x.PasserType);
          
      }
    }
}
