using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
    public class BookingInfoFlightMapping : EntityTypeConfiguration<BookingInfoFlight>
    {
        public BookingInfoFlightMapping()
        {
            this.ToTable("BookingInfoFlight");
            this.HasKey(x => x.Id);
            
        }
    }
}
