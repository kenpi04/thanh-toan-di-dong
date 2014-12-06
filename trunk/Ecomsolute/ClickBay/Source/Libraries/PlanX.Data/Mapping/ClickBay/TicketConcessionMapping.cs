
using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
    public class TicketConcessionMapping : EntityTypeConfiguration<TicketConcession>
    {
        public TicketConcessionMapping()
        {
            this.ToTable("TicketConcession");
            this.HasKey(x => x.Id);
            
        }
    }
}
