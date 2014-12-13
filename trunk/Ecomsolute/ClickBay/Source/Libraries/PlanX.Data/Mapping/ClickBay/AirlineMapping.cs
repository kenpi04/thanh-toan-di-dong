using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
   public class AirlineMapping:EntityTypeConfiguration<Airline>
    {
       public AirlineMapping()
       {
           this.ToTable("Airlines");
           this.HasKey(x => x.Id);
       }
    }
}
