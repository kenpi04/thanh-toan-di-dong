using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;
namespace PlanX.Data.Mapping.ClickBay
{
   public class BookingInfoConditionMapping:EntityTypeConfiguration<BookingInfoCondition>
    {
       public BookingInfoConditionMapping()
       {
           this.ToTable("BookingInfoCondition");
           this.HasKey(x => x.Id);
           this.HasRequired(x => x.BookingInfoFlight)
               .WithMany(y => y.BookingInfoConditions)
               .HasForeignKey(x => x.BookingInfoFlightId);
         
       }
    }
}
