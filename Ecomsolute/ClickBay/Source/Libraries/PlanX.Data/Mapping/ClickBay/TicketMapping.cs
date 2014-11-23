using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
   public class TicketMapping:EntityTypeConfiguration<Ticket>
    {
       public TicketMapping()
       {
           this.ToTable("")
       }
    }
}
