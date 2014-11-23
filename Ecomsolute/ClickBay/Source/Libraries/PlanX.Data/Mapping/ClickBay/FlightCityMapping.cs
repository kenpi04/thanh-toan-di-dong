using PlanX.Core.Domain.ClickBay;
using System.Data.Entity.ModelConfiguration;

namespace PlanX.Data.Mapping.ClickBay
{
   public  class FlightCityMapping:EntityTypeConfiguration<FlightCity>
    {
       public FlightCityMapping()
       {
           this.ToTable("FlightCity");
           this.HasKey(x => x.Id);
           this.HasRequired(x => x.Country).WithMany().HasForeignKey(x => x.CountryId);
       }
    }
}
