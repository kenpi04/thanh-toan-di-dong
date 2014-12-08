using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
    public class TicketTypeMapping : EntityTypeConfiguration<TicketType>
    {
       public TicketTypeMapping()
       {
           this.ToTable("TicketType");
           this.HasKey(x => x.Id);
           
       }
    }
}
